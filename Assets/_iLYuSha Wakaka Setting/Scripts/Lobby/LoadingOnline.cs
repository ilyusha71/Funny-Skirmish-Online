/***************************************************************************
 * Loading
 * 讀取腳本
 * Last Updated: 2018/11/03
 * Description:
 * 1. 進度條
        參考：https://docs.unity3d.com/ScriptReference/AsyncOperation-allowSceneActivation.html
 * 2. 物件池移至目標場景
 *     參考：https://docs.unity3d.com/ScriptReference/SceneManagement.SceneManager.MoveGameObjectToScene.html
 ***************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;
using Photon.Pun;
using Photon.Realtime;
using Hashtable = ExitGames.Client.Photon.Hashtable;

namespace Kocmoca
{
    public class LoadingOnline : MonoBehaviourPun
    {
        public Image barLoading;
        public TextMeshProUGUI textProgress;
        private float progress;
        private float percent;

        private readonly float intervalBatch = 0.05f; // 間隔時間
        private readonly int countBatch = 100; // 克隆批數
        private float orderBatch; // 批次序列
        public float percentBatch { get { return (orderBatch / countBatch); } } // 克隆批次百分比
        bool wait=false;

        private void Awake()
        {
            if (!PhotonNetwork.IsConnected)
                SceneManager.LoadScene(LobbyInfomation.SCENE_LOBBY);
        }
        void Start()
        {
            StartCoroutine(LoadScene());
        }

        IEnumerator LoadScene()
        {
            // Set the current Scene to be able to unload it later
            Scene currentScene = SceneManager.GetActiveScene();
            PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { { LobbyInfomation.PLAYER_LOADING, false } });
            GameObject camera = Camera.main.gameObject;
            barLoading.fillAmount = 0;
            textProgress.text = "?";

            yield return null;

            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(LobbyInfomation.SCENE_OPERATION, LoadSceneMode.Additive);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;
            //When the load is still in progress, output the Text and progress bar
            while (!asyncOperation.isDone)
            {
                //Output the current progress
                progress = (asyncOperation.progress + percentBatch) * 0.5f;

                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    if (orderBatch < countBatch)
                    {
                        ResourceManager.instance.BatchClone();
                        yield return new WaitForSeconds(intervalBatch);
                        orderBatch++;
                    }
                    else
                    {
                        //Change the Text to show the Scene is ready
                        progress = 1.0f;
                        PhotonNetwork.LocalPlayer.SetCustomProperties(new Hashtable() { { LobbyInfomation.PLAYER_LOADING, true } });
                        if (percent > 0.99999f && CheckPlayersLoading())
                        {
                            wait = true;
                            Destroy(camera);
                            //Activate the Scene
                            asyncOperation.allowSceneActivation = true;
                        }
                    }
                }
                yield return null;
            }
            // Move the GameObject (you attach this in the Inspector) to the newly loaded Scene
            SceneManager.MoveGameObjectToScene(ObjectPoolManager.Instance.gameObject, SceneManager.GetSceneByName(LobbyInfomation.SCENE_OPERATION));
            // Unload the previous Scene
            SceneManager.UnloadSceneAsync(currentScene);
        }

        bool CheckPlayersLoading()
        {
            foreach (Player p in PhotonNetwork.PlayerList)
            {
                object isPlayerLoading;
                if (p.CustomProperties.TryGetValue(LobbyInfomation.PLAYER_LOADING, out isPlayerLoading))
                {
                    if (!(bool)isPlayerLoading)
                        return false;
                }
                else
                    return false;
            }
            return true;
        }

        private void Update()
        {
            if (wait) return;
            percent = Mathf.Lerp(percent, progress, 0.1f);
            barLoading.fillAmount = percent;
            textProgress.text = "" + Mathf.RoundToInt(percent * 100);
        }
    }
}