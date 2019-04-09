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

namespace Kocmoca
{
    public class Loading : MonoBehaviour
    {
        public Image barLoading;
        public TextMeshProUGUI textProgress;
        private float progress;
        private float percent;
        private string levelName;

        void Awake()
        {
            levelName = PlayerPrefs.GetString(LobbyInfomation.PREFS_LOAD_SCENE);
            if (levelName == "")
                levelName = LobbyInfomation.SCENE_LOBBY;
        }

        void Start()
        {
            StartCoroutine(LoadScene());
        }

        IEnumerator LoadScene()
        {
            barLoading.fillAmount = 0;
            textProgress.text = "?";

            yield return null;

            //Begin to load the Scene you specify
            AsyncOperation asyncOperation = SceneManager.LoadSceneAsync(levelName);
            //Don't let the Scene activate until you allow it to
            asyncOperation.allowSceneActivation = false;
            //Debug.Log("Pro :" + asyncOperation.progress);
            //When the load is still in progress, output the Text and progress bar
            while (!asyncOperation.isDone)
            {
                //Output the current progress
                progress = asyncOperation.progress;

                // Check if the load has finished
                if (asyncOperation.progress >= 0.9f)
                {
                    //Change the Text to show the Scene is ready
                    progress = 1.0f;
                    //Wait to you press the space key to activate the Scene
                    if (percent > 0.99999f)
                        //Activate the Scene
                        asyncOperation.allowSceneActivation = true;
                }
                yield return null;
            }
        }

        private void Update()
        {
            percent = Mathf.Lerp(percent, progress, 0.1f);
            barLoading.fillAmount = percent;
            textProgress.text = "" + Mathf.RoundToInt(percent * 100);
        }
    }
}