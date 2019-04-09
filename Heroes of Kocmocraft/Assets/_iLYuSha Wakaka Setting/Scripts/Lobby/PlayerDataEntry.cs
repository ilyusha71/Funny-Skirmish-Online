// --------------------------------------------------------------------------------------------------------------------
// <copyright file="PlayerListEntry.cs" company="Exit Games GmbH">
//   Part of: Asteroid Demo,
// </copyright>
// <summary>
//  Player List Entry
// </summary>
// <author>developer@exitgames.com</author>
// --------------------------------------------------------------------------------------------------------------------

using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using Photon.Pun.UtilityScripts;

namespace Kocmoca
{
    public class PlayerDataEntry : MonoBehaviour
    {
        [Header("UI References")]
        public Image imageMainColor;
        public Image imageTickColor;
        public TextMeshProUGUI textPlayerNumber;
        public TextMeshProUGUI textPlayerName;
        public Image PlayerReadyImage;
        private int ownerId;

        private void Awake()
        {
            int number = transform.GetSiblingIndex();
            imageMainColor.color = GeneralData.GetBaseColor(100 + number % 2);
            imageTickColor.color = GeneralData.GeTextColor(100 + number % 2);
            textPlayerNumber.color = GeneralData.GeTextColor(100 + number % 2);
            textPlayerName.color = GeneralData.GeTextColor(100 + number % 2);
            textPlayerNumber.text = number.ToString("00");
            textPlayerName.text = KocmocraftData.GetBotName(number);
        }

        #region UNITY

        public void OnEnable()
        {
            PlayerNumbering.OnPlayerNumberingChanged += OnPlayerNumberingChanged;
        }

        public void Start()
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == ownerId)
            {
                Hashtable initialProps = new Hashtable() { { LobbyInfomation.PLAYER_READY, false }, { LobbyInfomation.PLAYER_DATA_KEY, Kocmoca.LobbyInfomation.PLAYER_DATA_VALUE } };
                PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);
                PhotonNetwork.LocalPlayer.SetScore(0);
            }
        }

        public void OnDisable()
        {
            PlayerNumbering.OnPlayerNumberingChanged -= OnPlayerNumberingChanged;
        }

        #endregion

        public void Initialize(int playerId, string playerName)
        {
            ownerId = playerId;
            textPlayerName.text = playerName;
        }

        private void OnPlayerNumberingChanged()
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                if (p.ActorNumber == ownerId)
                {
                    int number = p.GetPlayerNumber();
                    if (number < 0) return;
                    transform.position = FindObjectOfType<GalaxyLobbyPanel>().BotData[number].position;

                    if (p.ActorNumber == PhotonNetwork.LocalPlayer.ActorNumber)
                    {
                        Color32 light = new Color32(197, 137, 255, 255);
                        imageMainColor.color = new Color32(37, 0, 93, 255);
                        imageTickColor.color = light;
                        textPlayerNumber.color = light;
                        textPlayerName.color = light;

                        FindObjectOfType<GalaxyLobbyPanel>().SetPlayerMarker(number);
                    }
                    else
                    {
                        imageMainColor.color = GeneralData.GetBaseColor(number % 2);
                        imageTickColor.color = GeneralData.GeTextColor(number % 2);
                        textPlayerNumber.color = GeneralData.GeTextColor(number % 2);
                        textPlayerName.color = GeneralData.GeTextColor(number % 2);
                    }
                    textPlayerNumber.text = number.ToString("00");
                }
            }
        }

        public void SetPlayerReady(bool playerReady)
        {
            PlayerReadyImage.enabled = playerReady;
        }
    }
}