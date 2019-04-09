using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using Debug = UnityEngine.Debug;
using Photon.Pun;
using Photon.Realtime;

namespace Kocmoca
{
    public class PingRegion : MonoBehaviour, IConnectionCallbacks
    {
        public GameObject prefabRegion;
        private Dictionary<string, int> dataRegion = new Dictionary<string, int>();
        private bool showRegion = false;
        private LoadBalancingClient lbc;
        private Color32 colorVeryStrong = new Color32(0,255,30,255);
        private Color32 colorStrong = new Color32(209, 255, 69, 255);
        private Color32 colorMedium = new Color32(255,196,0,255);
        private Color32 colorWeak = new Color32(255,100,0,255);
        private Color32 colorVeryWeak = new Color32(255, 59, 59, 255);

        public void Start()
        {
            this.lbc = new LoadBalancingClient();
            this.lbc.AppId = PhotonNetwork.PhotonServerSettings.AppSettings.AppIdRealtime;
            this.lbc.AddCallbackTarget(this);
            this.lbc.ConnectToNameServer();
        }

        public void Update()
        {
            LoadBalancingClient client = this.lbc;
            if (client != null)
                client.Service();
            if (showRegion)
            {
                foreach (KeyValuePair<string, int> data in dataRegion)
                {
                    string region = data.Key;
                    int ping = data.Value;
                    GameObject item = Instantiate(prefabRegion, transform);
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
                        PhotonNetwork.PhotonServerSettings.AppSettings.FixedRegion = region;
                        //FindObjectOfType<GalaxyLobbyPanel>().OnLoginButtonClicked();
                    });
                }
                showRegion = false;
            }
        }

        public void OnConnected()
        {
        }

        public void OnConnectedToMaster()
        {
            Debug.Log("OnConnectedToMaster");
            this.lbc.OpJoinRandomRoom();    // joins any open room (no filter)
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
                case "eu": return "Europe";
                case "us": return "USA, East";
                case "usw": return "USA, West";
                case "cae": return "Canada, East";
                case "asia": return "Asia";
                case "jp": return "Japan";
                case "au": return "Australia";
                case "sa": return "South America";
                case "in": return "India";
                case "ru": return "Russia";
                case "rue": return "Russia, East";
                case "kr": return "South Korea";
                default: return "Unknown";
            }
        }
    }
}