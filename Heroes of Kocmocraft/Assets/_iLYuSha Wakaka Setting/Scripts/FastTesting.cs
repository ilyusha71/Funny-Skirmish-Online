using ExitGames.Client.Photon;
using Photon.Pun;
using System.Collections.Generic;

using UnityEngine;
using UnityEngine.UI;
using Debug = UnityEngine.Debug;

namespace Photon.Realtime.Demo
{
    public class FastTesting : MonoBehaviour, IConnectionCallbacks, IMatchmakingCallbacks, ILobbyCallbacks
    {
        public string AppId; // set in inspector

        private ConnectionHandler ch;
        public Text StateUiText;


        void Awake()
        {
            PhotonNetwork.AutomaticallySyncScene = true;
            PhotonNetwork.NetworkingClient.AddCallbackTarget(this);
        }
        public void Start()
        {
            //this.lbc = new LoadBalancingClient();
            //this.lbc.AppId = this.AppId;
            //this.lbc.AddCallbackTarget(this);
            //this.lbc.ConnectToNameServer();

            //this.ch = this.gameObject.GetComponent<ConnectionHandler>();
            //if (this.ch != null)
            //{
            //    this.ch.Client = this.lbc;
            //    this.ch.StartFallbackSendAckThread();
            //}



            PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime = "6790855d-98ce-4acd-b64f-688c4a354ccf"; // TODO: replace with your own AppId
            PhotonNetwork.NetworkingClient.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
            PhotonNetwork.NetworkingClient.NameServerHost = "ns.exitgames.com";
            PhotonNetwork.NetworkingClient.ConnectToNameServer();
            PhotonNetwork.LocalPlayer.NickName = "Wakaka";
            PhotonNetwork.NetworkingClient.ConnectToRegionMaster("jp");
        }

        public void Update()
        {
            //LoadBalancingClient client = this.lbc;
            //if (client != null)
            //{
            //    client.Service();


            //    Text uiText = this.StateUiText;
            //    string state = client.State.ToString();
            //    if (uiText != null && !uiText.text.Equals(state))
            //    {
            //        uiText.text = "State: " + state;
            //    }
            //}
        }


        public void OnConnected()
        {
        }

        public void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");
            PhotonNetwork.JoinRandomRoom();
            //this.lbc.OpJoinRandomRoom();    // joins any open room (no filter)
        }

        public void OnDisconnected(DisconnectCause cause)
        {
            Debug.Log("OnDisconnected(" + cause + ")");
        }

        public void OnCustomAuthenticationResponse(Dictionary<string, object> data)
        {
        }

        public void OnCustomAuthenticationFailed(string debugMessage)
        {
        }

        public void OnRegionListReceived(RegionHandler regionHandler)
        {
            Debug.Log("OnRegionListReceived");
            regionHandler.PingMinimumOfRegions(this.OnRegionPingCompleted, null);
        }

        public void OnRoomListUpdate(List<RoomInfo> roomList)
        {
        }

        public void OnLobbyStatisticsUpdate(List<TypedLobbyInfo> lobbyStatistics)
        {
        }

        public void OnJoinedLobby()
        {
            Debug.Log("OnJoinedLobby");
        }

        public void OnLeftLobby()
        {
        }

        public void OnFriendListUpdate(List<FriendInfo> friendList)
        {
        }

        public void OnCreatedRoom()
        {
        }

        public void OnCreateRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinedRoom()
        {
            Hashtable initialProps = new Hashtable() { { Kocmoca.LobbyInfomation.PLAYER_READY, false }, { Kocmoca.LobbyInfomation.PLAYER_DATA_KEY, Kocmoca.LobbyInfomation.PLAYER_DATA_VALUE } };
            PhotonNetwork.LocalPlayer.SetCustomProperties(initialProps);

            Debug.Log("OnJoinedRoom");
            int localType = PlayerPrefs.GetInt(Kocmoca.LobbyInfomation.PREFS_TYPE);
            int localSkin = PlayerPrefs.GetInt(Kocmoca.LobbyInfomation.PREFS_SKIN + localType);

            Hashtable propertiesKocmocraft = new Hashtable
            {
                { Kocmoca.LobbyInfomation.PROPERTY_TYPE, 18 },
                { Kocmoca.LobbyInfomation.PROPERTY_SKIN, 1 }
            };
            PhotonNetwork.SetPlayerCustomProperties(propertiesKocmocraft);

            //Hashtable propKocmocraft = new Hashtable
            //{
            //    { Kocmoca.LobbyInfomation.PROPERTY_SKIN, localSkin }
            //};
            //PhotonNetwork.SetPlayerCustomProperties(propKocmocraft);

            Hashtable props = new Hashtable
            {
                {Kocmoca.LobbyInfomation.PLAYER_LOADED_LEVEL, false}
            };
            PhotonNetwork.LocalPlayer.SetCustomProperties(props);
            PhotonNetwork.CurrentRoom.IsOpen = false;
            PhotonNetwork.CurrentRoom.IsVisible = false;

            PhotonNetwork.LoadLevel(Kocmoca.LobbyInfomation.SCENE_LOADING_ONLINE);
        }

        public void OnJoinRoomFailed(short returnCode, string message)
        {
        }

        public void OnJoinRandomFailed(short returnCode, string message)
        {
            Debug.Log("OnJoinRandomFailed");
            string roomName = "Room " + Random.Range(1000, 10000);

            RoomOptions options = new RoomOptions { MaxPlayers = 8 };

            PhotonNetwork.CreateRoom(roomName, options, null);
        }

        public void OnLeftRoom()
        {
        }


        /// <summary>A callback of the RegionHandler, provided in OnRegionListReceived.</summary>
        /// <param name="regionHandler">The regionHandler wraps up best region and other region relevant info.</param>
        private void OnRegionPingCompleted(RegionHandler regionHandler)
        {
            Debug.Log("OnRegionPingCompleted " + regionHandler.BestRegion);
            Debug.Log("RegionPingSummary: " + regionHandler.SummaryToCache);
        }
    }
}