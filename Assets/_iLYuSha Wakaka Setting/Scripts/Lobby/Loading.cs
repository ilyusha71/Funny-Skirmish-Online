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
    public class Loading : MonoBehaviourPun
    {
        public Image barLoading;
        public TextMeshProUGUI textProgress;

        void Start()
        {
            StartCoroutine(LoadScene());
        }

        IEnumerator LoadScene()
        {
            barLoading.fillAmount = 0;
            textProgress.text = "Loading...";

            yield return null;

            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(PlayerPrefs.GetString(LobbyInfomation.PREFS_LOAD_SCENE));
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;
            //Debug.Log("Pro :" + asyncOperation.progress);
            //When the load is still in progress, output the Text and progress bar
            while (!asyncOperation.isDone)
            {
                //Output the current progress
                barLoading.fillAmount = asyncOperation.progress;
                textProgress.text = string.Format("{0:0.00%}", asyncOperation.progress);

                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    //Change the Text to show the Scene is ready
                    barLoading.fillAmount = 1.0f;
                    textProgress.text = "100%";
                    //Wait to you press the space key to activate the Scene
                    //if (Input.GetKeyDown(KeyCode.Space))
                        //Activate the Scene
                        asyncOperation.allowSceneActivation = true;
                }

                yield return null;
            }
        }
    }
}