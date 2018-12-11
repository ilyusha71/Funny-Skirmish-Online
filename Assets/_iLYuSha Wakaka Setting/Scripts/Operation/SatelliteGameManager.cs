// --------------------------------------------------------------------------------------------------------------------
// <copyright file="AsteroidsGameManager.cs" company="Exit Games GmbH">
//   Part of: Asteroid demo
// </copyright>
// <summary>
//  Game Manager for the Asteroid Demo
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using System.Collections;

using UnityEngine;
using UnityEngine.UI;

using Photon.Realtime;
using Photon.Pun.UtilityScripts;
using Hashtable = ExitGames.Client.Photon.Hashtable;
using UnityEngine.SceneManagement;

namespace Photon.Pun.Demo.Asteroids
{
    public class SatelliteGameManager : MonoBehaviourPunCallbacks
    {
        public static SatelliteGameManager Instance { get; private set; }

        public Text InfoText;

        public GameObject[] AsteroidPrefabs;

        #region UNITY

        public void Awake()
        {
            Instance = this;
            if(!PhotonNetwork.IsConnected)
                SceneManager.LoadScene(Kocmoca.LobbyInfomation.SCENE_LOBBY);
            PhotonNetwork.AutomaticallySyncScene = false;
        }

        public override void OnEnable()
        {
            base.OnEnable();

            CountdownTimer.OnCountdownTimerHasExpired += OnCountdownTimerIsExpired;
        }

        public void Start()
        {
            InfoText.text = "Waiting for other players...";

            Hashtable props = new Hashtable
            {
                {Kocmoca.LobbyInfomation.PLAYER_LOADED_LEVEL, true}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        public override void OnDisable()
        {
            base.OnDisable();

            CountdownTimer.OnCountdownTimerHasExpired -= OnCountdownTimerIsExpired;
        }

        #endregion

        #region COROUTINES

        private IEnumerator SpawnAsteroid()
        {
            while (true)
            {
                yield return new WaitForSeconds(Random.Range(AsteroidsGame.ASTEROIDS_MIN_SPAWN_TIME, AsteroidsGame.ASTEROIDS_MAX_SPAWN_TIME));

                Vector2 direction = Random.insideUnitCircle;
                Vector3 position = Vector3.zero;

                if (Mathf.Abs(direction.x) > Mathf.Abs(direction.y))
                {
                    // Make it appear on the left/right side
                    position = new Vector3(Mathf.Sign(direction.x) * Camera.main.orthographicSize * Camera.main.aspect, 0, direction.y * Camera.main.orthographicSize);
                }
                else
                {
                    // Make it appear on the top/bottom
                    position = new Vector3(direction.x * Camera.main.orthographicSize * Camera.main.aspect, 0, Mathf.Sign(direction.y) * Camera.main.orthographicSize);
                }

                // Offset slightly so we are not out of screen at creation time (as it would destroy the asteroid right away)
                position -= position.normalized * 0.1f;


                Vector3 force = -position.normalized * 1000.0f;
                Vector3 torque = Random.insideUnitSphere * Random.Range(500.0f, 1500.0f);
                object[] instantiationData = { force, torque, true };

                PhotonNetwork.InstantiateSceneObject("BigAsteroid", position, Quaternion.Euler(Random.value * 360.0f, Random.value * 360.0f, Random.value * 360.0f), 0, instantiationData);
            }
        }

        private IEnumerator EndOfGame(string winner, int score)
        {
            float timer = 5.0f;

            while (timer > 0.0f)
            {
                InfoText.text = string.Format("Player {0} won with {1} points.\n\n\nReturning to login screen in {2} seconds.", winner, score, timer.ToString("n2"));

                yield return new WaitForEndOfFrame();

                timer -= Time.deltaTime;
            }

            PhotonNetwork.LeaveRoom();
        }

        #endregion

        #region PUN CALLBACKS

        public override void OnDisconnected(DisconnectCause cause)
        {
            //UnityEngine.SceneManagement.SceneManager.LoadScene("DemoAsteroids-LobbyScene");
        }

        public override void OnLeftRoom()
        {
            //PhotonNetwork.Disconnect();
            PhotonNetwork.LocalPlayer.CustomProperties.Remove(PlayerNumbering.RoomPlayerIndexedProp);
            SceneManager.LoadScene(Kocmoca.LobbyInfomation.SCENE_LOBBY);
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {
                //StartCoroutine(SpawnAsteroid());
            }
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            CheckEndOfGame();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (changedProps.ContainsKey(AsteroidsGame.PLAYER_LIVES))
            {
                CheckEndOfGame();
                return;
            }

            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            if (changedProps.ContainsKey(Kocmoca.LobbyInfomation.PLAYER_LOADED_LEVEL))
            {
                if (CheckAllPlayerLoadedLevel())
                {
                    Hashtable props = new Hashtable
                    {
                        {CountdownTimer.CountdownStartTime, (float) PhotonNetwork.Time}
                    };
                    PhotonNetwork.CurrentRoom.SetCustomProperties(props);
                }
            }
        }

        #endregion

        private void StartGame()
        {
            Kocmoca.SatelliteCommander.Instance.InitializeSatellite();

            if (PhotonNetwork.IsMasterClient)
            {
                //StartCoroutine(SpawnAsteroid());
            }
        }

        private bool CheckAllPlayerLoadedLevel()
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object playerLoadedLevel;

                if (p.CustomProperties.TryGetValue(Kocmoca.LobbyInfomation.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
                {
                    if ((bool)playerLoadedLevel)
                    {
                        continue;
                    }
                }

                return false;
            }

            return true;
        }

        private void CheckEndOfGame()
        {
            bool allDestroyed = true;

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object lives;
                if (p.CustomProperties.TryGetValue(AsteroidsGame.PLAYER_LIVES, out lives))
                {
                    if ((int)lives > 0)
                    {
                        allDestroyed = false;
                        break;
                    }
                }
            }

            if (allDestroyed)
            {
                if (PhotonNetwork.IsMasterClient)
                {
                    StopAllCoroutines();
                }

                string winner = "";
                int score = -1;

                foreach (Player p in PhotonNetwork.PlayerList)
                {
                    if (p.GetScore() > score)
                    {
                        winner = p.NickName;
                        score = p.GetScore();
                    }
                }

                StartCoroutine(EndOfGame(winner, score));
            }
        }

        private void OnCountdownTimerIsExpired()
        {
            StartGame();
        }
    }
}
