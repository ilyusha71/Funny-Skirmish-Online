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
using Photon.Pun;

namespace Kocmoca
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
                SceneManager.LoadScene("FastLoading");
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
                {LobbyInfomation.PLAYER_LOADED_LEVEL, true}
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
            SceneManager.LoadScene(LobbyInfomation.SCENE_LOBBY);
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
            //CheckEndOfGame();
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            //if (changedProps.ContainsKey(AsteroidsGame.PLAYER_LIVES))
            //{
            //    CheckEndOfGame();
            //    return;
            //}

            if (!PhotonNetwork.IsMasterClient)
            {
                return;
            }

            if (changedProps.ContainsKey(LobbyInfomation.PLAYER_LOADED_LEVEL))
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
            SatelliteCommander.Instance.InitializeSatellite();

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

                if (p.CustomProperties.TryGetValue(LobbyInfomation.PLAYER_LOADED_LEVEL, out playerLoadedLevel))
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

        //private void CheckEndOfGame()
        //{
        //    bool allDestroyed = true;

        //    foreach (Player p in PhotonNetwork.PlayerList)
        //    {
        //        object lives;
        //        if (p.CustomProperties.TryGetValue(AsteroidsGame.PLAYER_LIVES, out lives))
        //        {
        //            if ((int)lives > 0)
        //            {
        //                allDestroyed = false;
        //                break;
        //            }
        //        }
        //    }

        //    if (allDestroyed)
        //    {
        //        if (PhotonNetwork.IsMasterClient)
        //        {
        //            StopAllCoroutines();
        //        }

        //        string winner = "";
        //        int score = -1;

        //        foreach (Player p in PhotonNetwork.PlayerList)
        //        {
        //            if (p.GetScore() > score)
        //            {
        //                winner = p.NickName;
        //                score = p.GetScore();
        //            }
        //        }

        //        StartCoroutine(EndOfGame(winner, score));
        //    }
        //}

        private void OnCountdownTimerIsExpired()
        {
            StartGame();
        }
    }
}
