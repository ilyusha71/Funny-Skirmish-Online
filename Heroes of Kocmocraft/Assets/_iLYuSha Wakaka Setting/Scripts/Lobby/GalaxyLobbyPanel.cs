using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using ExitGames.Client.Photon;
using Photon.Pun;
using Photon.Realtime;
using DG.Tweening;

namespace Kocmoca
{
    public enum LobbyState
    {
        Portal = 0,
        Moving,
        Login,
        Lobby,
        Hangar,
        Operation,
        Controller,
        Escape,


        Show,
        newEvent,       
    }
    public partial class GalaxyLobbyPanel : MonoBehaviourPunCallbacks
    {
        public LobbyState lobbyState = LobbyState.Portal;

        [Header("External Scripts")]
        public PortalController Portal;

        [Header("Connection Status")]
        public TextMeshProUGUI ConnectionStatusText;
        public TextMeshProUGUI RegionPingText;
        public TextMeshProUGUI SearchText;
        private string serverRegion;

        [Header("Region Panel")]
        public Transform listRegion; 
        public GameObject prefabRegion;
        private Dictionary<string, int> dataRegion = new Dictionary<string, int>();
        private bool showRegion = false;
        private readonly Color32 colorVeryStrong = new Color32(0, 255, 30, 255);
        private readonly Color32 colorStrong = new Color32(209, 255, 69, 255);
        private readonly Color32 colorMedium = new Color32(255, 196, 0, 255);
        private readonly Color32 colorWeak = new Color32(255, 100, 0, 255);
        private readonly Color32 colorVeryWeak = new Color32(255, 59, 59, 255);

        [Header("Login Panel")]
        public GameObject LoginPanel;
        public TMP_InputField PlayerNameInput;
        private readonly string connectionStatusMessage = "[Connection Status]\n";

        [Header("Selection Panel")]
        public GameObject SelectionPanel;

        [Header("Create Room Panel")]
        public GameObject CreateRoomPanel;

        public InputField RoomNameInputField;
        public InputField MaxPlayersInputField;

        [Header("Join Random Room Panel")]
        public GameObject JoinRandomRoomPanel;

        [Header("Room List Panel")]
        public GameObject RoomListPanel;

        public GameObject RoomListContent;
        public GameObject RoomListEntryPrefab;

        [Header("Inside Room Panel")]
        public GameObject InsideRoomPanel;

        public Button StartGameButton;
        public GameObject PlayerDataEntryPrefab;

        private Dictionary<string, RoomInfo> cachedRoomList;
        private Dictionary<string, GameObject> roomListEntries;
        private Dictionary<int, GameObject> playerListEntries;

        [Header("Data")]
        public Transform[] iconBot;
        public Transform PlayerDataList;
        public Transform[] BotData;
        private Vector3 invisiblePos = new Vector3(999, 10000, -1);
        public Toggle toggleReady;
        public TextMeshProUGUI textReady;
        private bool isPlayerReady;

        #region UNITY

        private void Awake()
        {
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "8d3d0446-b335-47c1-ab69-7aa9e6878c10"; // TODO: replace with your own AppId

//#if CHINA
//    PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime  = "8d3d0446-b335-47c1-ab69-7aa9e6878c10"; // TODO: replace with your own AppId
//#else
//            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "6790855d-98ce-4acd-b64f-688c4a354ccf"; // TODO: replace with your own AppId
//#endif

            Portal.OnShutterPressedUp += Connect;
            PhotonNetwork.AutomaticallySyncScene = true;
            cachedRoomList = new Dictionary<string, RoomInfo>();
            roomListEntries = new Dictionary<string, GameObject>();
            this.SetActivePanel("Close all panel");
            PlayerNameInput.gameObject.SetActive(false);
            Initialize();
        }

        void Connect()
        {
            lobbyState = LobbyState.Moving;
            if (!PhotonNetwork.IsConnected)
            {
                //PhotonNetwork.NetworkingClient.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
                PhotonNetwork.NetworkingClient.AddCallbackTarget(this);
                MoveToLogin();
                //RefreshRegion();                
            }
            else
                MoveToLobby();
        }

        public void ConnectChinaServer()
        {
            SearchText.enabled = true;
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "8d3d0446-b335-47c1-ab69-7aa9e6878c10"; // TODO: replace with your own AppId
            PhotonNetwork.NetworkingClient.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
            PhotonNetwork.NetworkingClient.NameServerHost = "ns.photonengine.cn";
            RefreshRegion();
            PhotonNetwork.NetworkingClient.ConnectToNameServer();

        }

        public void ConnectInternationServer()
        {
            SearchText.enabled = true;
            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "6790855d-98ce-4acd-b64f-688c4a354ccf"; // TODO: replace with your own AppId
            PhotonNetwork.NetworkingClient.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
            PhotonNetwork.NetworkingClient.NameServerHost = "ns.exitgames.com";
            RefreshRegion();
            PhotonNetwork.NetworkingClient.ConnectToNameServer();
        }

        void RefreshRegion()
        {
            dataRegion.Clear();
            Button[] btn = listRegion.GetComponentsInChildren<Button>();
            int count = btn.Length;
            for (int i = 0; i < count; i++)
            {
                Destroy(btn[i].gameObject);
            }
        }

        private void Update()
        {
            ConnectionStatusText.text = connectionStatusMessage + PhotonNetwork.NetworkClientState;
            if (PhotonNetwork.IsConnected)
                RegionPingText.text = string.Format("[Server] {0}\n{1} ms", serverRegion, PhotonNetwork.GetPing());
            else
                RegionPingText.text = "[Server] Disconnect\n";

            if (showRegion)
            {
                SearchText.enabled = false;
                foreach (KeyValuePair<string, int> data in dataRegion)
                {
                    string region = data.Key;
                    int ping = data.Value;
                    GameObject item = Instantiate(prefabRegion, listRegion);
                    Button btnRegion = item.GetComponent<Button>();
                    TextMeshProUGUI textRegion = item.GetComponentsInChildren<TextMeshProUGUI>()[0];
                    TextMeshProUGUI textPing = item.GetComponentsInChildren<TextMeshProUGUI>()[1];
                    Image iconPing = item.GetComponentsInChildren<Image>()[1];
                    textRegion.text = GetRegionName(region);
                    textPing.text = ping + " ms";
                    if (ping < 70)
                    {
                        iconPing.color = colorVeryStrong;
                        iconPing.fillAmount = 1.0f;
                    }
                    else if (ping < 100)
                    {
                        iconPing.color = colorStrong;
                        iconPing.fillAmount = 0.8f;
                    }
                    else if (ping < 170)
                    {
                        iconPing.color = colorMedium;
                        iconPing.fillAmount = 0.6f;
                    }
                    else if (ping < 250)
                    {
                        iconPing.color = colorWeak;
                        iconPing.fillAmount = 0.4f;
                    }
                    else
                    {
                        iconPing.color = colorVeryWeak;
                        iconPing.fillAmount = 0.2f;
                    }
                    btnRegion.onClick.AddListener(() =>
                    {
                        serverRegion = GetRegionName(region);
                        OnLoginButtonClicked(region);
                    });
                }
                showRegion = false;
            }

            if (lobbyState == LobbyState.Lobby)
                Command();
        }

        private string Number2Scheme(int number)
        {
            int alphaOrder = number % 26;
            int alphaOrder2 = number % (Random.Range(7, 27));
            return "" + (char)(alphaOrder + 65) + (char)(alphaOrder2 + 65)  + number % 100;
        }

        #endregion

        #region PUN CALLBACKS

        public override void OnConnectedToMaster()
        {
            if (lobbyState == LobbyState.Login)
                MoveToLobby();
            else // 离开房间也会触发OnConnectedToMaster，必须触发OnConnectedToMaster才能重新进行PUN的动作
            {
                lobbyState = LobbyState.Lobby;
                SetActivePanel("SelectionPanel");
            }
        }

        public override void OnRegionListReceived(RegionHandler regionHandler)
        {
            Debug.Log("Galaxy OnRegionListReceived");
            regionHandler.PingMinimumOfRegions(this.OnRegionPingCompleted, null);
        }

        private void OnRegionPingCompleted(RegionHandler regionHandler)
        {
            int countRegion = regionHandler.EnabledRegions.Count;
            for (int i = 0; i < countRegion; i++)
            {
                dataRegion.Add(regionHandler.EnabledRegions[i].Code, regionHandler.EnabledRegions[i].Ping);
            }
            showRegion = true;
            Debug.Log("OnRegionPingCompleted " + regionHandler.BestRegion);
            Debug.Log("RegionPingSummary: " + regionHandler.SummaryToCache);
        }

        private string GetRegionName(string region)
        {
            switch (region)
            {
                case "eu": return "欧洲地区";
                case "us": return "美国东部";
                case "usw": return "美国西部";
                case "cae": return "加拿大东部";
                case "asia": return "亚洲地区";
                case "jp": return "日本";
                case "au": return "澳大利亚";
                case "sa": return "南美地区";
                case "in": return "印度";
                case "ru": return "俄罗斯";
                case "rue": return "远东地区";
                case "kr": return "南韩";
                case "cn": return "中国";
                default: return "未知区域";
            }
        }

        public override void OnRoomListUpdate(List<RoomInfo> roomList)
        {
            ClearRoomListView();

            UpdateCachedRoomList(roomList);
            UpdateRoomListView();
        }

        public override void OnLeftLobby()
        {
            cachedRoomList.Clear();

            ClearRoomListView();
        }

        public override void OnCreateRoomFailed(short returnCode, string message)
        {
            SetActivePanel(SelectionPanel.name);
        }

        public override void OnJoinRoomFailed(short returnCode, string message)
        {
            SetActivePanel(SelectionPanel.name);
        }

        public override void OnJoinRandomFailed(short returnCode, string message)
        {
            string roomName = "Room " + Random.Range(1000, 10000);

            RoomOptions options = new RoomOptions { MaxPlayers = 8 };

            PhotonNetwork.CreateRoom(roomName, options, null);
        }

        public override void OnJoinedRoom()
        {
            Hashtable skin = new Hashtable
            {
                { LobbyInfomation.PLAYER_SKIN_OPTION, localSkin }
            };
            PhotonNetwork.SetPlayerCustomProperties(skin);

            Hashtable propKocmocraft = new Hashtable
            {
                { "KocmocraftType", 6 }
            };
            PhotonNetwork.SetPlayerCustomProperties(propKocmocraft);

            SetActivePanel(InsideRoomPanel.name);

            if (playerListEntries == null)
            {
                playerListEntries = new Dictionary<int, GameObject>();
            }

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                GameObject entry = Instantiate(PlayerDataEntryPrefab, PlayerDataList);
                entry.transform.position = invisiblePos;
                entry.GetComponent<PlayerDataEntry>().Initialize(p.ActorNumber, p.NickName);

                object isPlayerReady;
                if (p.CustomProperties.TryGetValue(Kocmoca.LobbyInfomation.PLAYER_READY, out isPlayerReady))
                {
                    entry.GetComponent<PlayerDataEntry>().SetPlayerReady((bool)isPlayerReady);
                }

                playerListEntries.Add(p.ActorNumber, entry);
            }

            StartGameButton.gameObject.SetActive(CheckPlayersReady());

            Hashtable props = new Hashtable
            {
                {LobbyInfomation.PLAYER_LOADED_LEVEL, false}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
        }

        public override void OnLeftRoom()
        {
            this.SetActivePanel("Close all panel");

            foreach (GameObject entry in playerListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            playerListEntries.Clear();
            playerListEntries = null;
            iconBot[20].position = invisiblePos;
            toggleReady.isOn = false;
            textReady.text = "Ready";
        }

        public override void OnPlayerEnteredRoom(Player newPlayer)
        {
            GameObject entry = Instantiate(PlayerDataEntryPrefab);
            entry.transform.SetParent(PlayerDataList);
            entry.transform.localScale = Vector3.one;
            entry.transform.position = invisiblePos;
            entry.GetComponent<PlayerDataEntry>().Initialize(newPlayer.ActorNumber, newPlayer.NickName);

            playerListEntries.Add(newPlayer.ActorNumber, entry);

            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        public override void OnPlayerLeftRoom(Player otherPlayer)
        {
            Destroy(playerListEntries[otherPlayer.ActorNumber].gameObject);
            playerListEntries.Remove(otherPlayer.ActorNumber);

            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        public override void OnMasterClientSwitched(Player newMasterClient)
        {
            if (PhotonNetwork.LocalPlayer.ActorNumber == newMasterClient.ActorNumber)
            {
                StartGameButton.gameObject.SetActive(CheckPlayersReady());
            }
        }

        public override void OnPlayerPropertiesUpdate(Player targetPlayer, Hashtable changedProps)
        {
            if (playerListEntries == null)
            {
                playerListEntries = new Dictionary<int, GameObject>();
            }

            GameObject entry;
            if (playerListEntries.TryGetValue(targetPlayer.ActorNumber, out entry))
            {
                object isPlayerReady;
                if (changedProps.TryGetValue(Kocmoca.LobbyInfomation.PLAYER_READY, out isPlayerReady))
                {
                    entry.GetComponent<PlayerDataEntry>().SetPlayerReady((bool)isPlayerReady);
                }
            }

            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        public override void OnDisconnected(DisconnectCause cause)
        {
            Debug.LogWarning("Galaxy Lobby: OnDisconnected(" + cause + ")");
            serverRegion = "";
            MoveToLogin();
            //RefreshRegion();
            //UnityEngine.SceneManagement.SceneManager.LoadScene(LobbyInfomation.SCENE_LOBBY);
        }

        #endregion

        #region UI CALLBACKS

        public void OnBackButtonClicked()
        {
            SetActivePanel(SelectionPanel.name);
        }

        public void OnCreateRoomButtonClicked()
        {
            string roomName = RoomNameInputField.text;
            roomName = (roomName.Equals(string.Empty)) ? "Room " + Random.Range(1000, 10000) : roomName;

            byte maxPlayers;
            byte.TryParse(MaxPlayersInputField.text, out maxPlayers);
            maxPlayers = (byte)Mathf.Clamp(maxPlayers, 2, 8);

            RoomOptions options = new RoomOptions { MaxPlayers = maxPlayers };

            PhotonNetwork.CreateRoom(roomName, options, null);
        }

        public void OnJoinRandomRoomButtonClicked()
        {
            SetActivePanel(JoinRandomRoomPanel.name);

            PhotonNetwork.JoinRandomRoom();
        }

        public void OnLeaveGameButtonClicked()
        {
            PhotonNetwork.LeaveRoom();
        }

        public void OnLoginButtonClicked(string region)
        {
            string playerName = PlayerNameInput.text == ""? "iLYuSha " + Number2Scheme(Random.Range(1000, 10000)) : PlayerNameInput.text;

            if (!playerName.Equals(""))
            {
                this.SetActivePanel("Close all panel");
                PhotonNetwork.LocalPlayer.NickName = playerName;
                PhotonNetwork.NetworkingClient.ConnectToRegionMaster(region);
            }
            else
            {
                Debug.LogError("Player Name is invalid.");
            }
        }

        public void OnRoomListButtonClicked()
        {
            if (!PhotonNetwork.InLobby)
            {
                PhotonNetwork.JoinLobby();
            }
            SetActivePanel(RoomListPanel.name);

        }

        public void OnStartGameButtonClicked()
        {
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            PhotonNetwork.LoadLevel(LobbyInfomation.SCENE_LOADING_ONLINE);
        }

        public void OnReadyButtonClicked()
        {
            isPlayerReady = !isPlayerReady;
            textReady.text = isPlayerReady ? "Cancel" : "Ready";

            Hashtable props = new Hashtable() { { Kocmoca.LobbyInfomation.PLAYER_READY, isPlayerReady } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);

            if (PhotonNetwork.IsMasterClient)
                LocalPlayerPropertiesUpdated();
        }

        #endregion

        private bool CheckPlayersReady()
        {
            if (!PhotonNetwork.IsMasterClient)
            {
                return false;
            }

            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object isPlayerReady;
                if (p.CustomProperties.TryGetValue(LobbyInfomation.PLAYER_READY, out isPlayerReady))
                {
                    if (!(bool)isPlayerReady)
                    {
                        return false;
                    }
                }
                else
                {
                    return false;
                }
            }

            return true;
        }

        private void ClearRoomListView()
        {
            foreach (GameObject entry in roomListEntries.Values)
            {
                Destroy(entry.gameObject);
            }

            roomListEntries.Clear();
        }

        public void LocalPlayerPropertiesUpdated()
        {
            StartGameButton.gameObject.SetActive(CheckPlayersReady());
        }

        public void SetActivePanel(string activePanel)
        {
            LoginPanel.SetActive(activePanel.Equals(LoginPanel.name));
            SelectionPanel.SetActive(activePanel.Equals(SelectionPanel.name));
            CreateRoomPanel.SetActive(activePanel.Equals(CreateRoomPanel.name));
            JoinRandomRoomPanel.SetActive(activePanel.Equals(JoinRandomRoomPanel.name));
            RoomListPanel.SetActive(activePanel.Equals(RoomListPanel.name));    // UI should call OnRoomListButtonClicked() to activate this
            InsideRoomPanel.SetActive(activePanel.Equals(InsideRoomPanel.name));
        }

        private void UpdateCachedRoomList(List<RoomInfo> roomList)
        {
            foreach (RoomInfo info in roomList)
            {
                // Remove room from cached room list if it got closed, became invisible or was marked as removed
                if (!info.IsOpen || !info.IsVisible || info.RemovedFromList)
                {
                    if (cachedRoomList.ContainsKey(info.Name))
                    {
                        cachedRoomList.Remove(info.Name);
                    }

                    continue;
                }

                // Update cached room info
                if (cachedRoomList.ContainsKey(info.Name))
                {
                    cachedRoomList[info.Name] = info;
                }
                // Add new room info to cache
                else
                {
                    cachedRoomList.Add(info.Name, info);
                }
            }
        }

        private void UpdateRoomListView()
        {
            foreach (RoomInfo info in cachedRoomList.Values)
            {
                GameObject entry = Instantiate(RoomListEntryPrefab);
                entry.transform.SetParent(RoomListContent.transform);
                entry.transform.localScale = Vector3.one;
                entry.GetComponent<Photon.Pun.Demo.Asteroids.RoomListEntry>().Initialize(info.Name, (byte)info.PlayerCount, info.MaxPlayers);
                roomListEntries.Add(info.Name, entry);
            }
        }

        public void SetPlayerMarker(int index)
        {
            iconBot[20].position = iconBot[index].position;
            iconBot[20].GetComponentInChildren<TextMeshProUGUI>().text = index.ToString("00");
        }        
    }
}
