using System.Collections;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

namespace AirSupremacy
{
    public class MapOption : MenuOption
    {
        private PortalController shutter;
        bool switchStage;
        bool enterFlag;

        void Awake()
        {
            //if (WakakaController.instance == null)
            //{
            //    PlayerPrefs.SetInt("lastScene", SceneManager.GetActiveScene().buildIndex + 100);
            //    SceneManager.LoadScene("Wakaka Controller");
            //}

            //if (MainTheme.instance == null)
            //{
            //    PlayerPrefs.SetInt("lastScene", SceneManager.GetActiveScene().buildIndex + 100);
            //    SceneManager.LoadScene("Main Theme");
            //}

            toggleOption = GetComponentsInChildren<Toggle>();
            countOption = toggleOption.Length;
            nowOption = PlayerPrefs.GetInt(GeneralInfo.saveMap);
            shutter = FindObjectOfType<PortalController>();
            shutter.OnShutterPressedUp += SwitchOption;
        }

        void Start()
        {
            //WakakaController.ChangeMode(ControlMode.General);
            StartCoroutine(Delay());
        }
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(0.7f);
            switchStage = true;
            //shutter.Shot();
        }

        IEnumerator BtnPlay()
        {
            //shutter.Shot();
            yield return new WaitForSeconds(1.7f);
            SceneManager.LoadScene("Aircraft Option");
        }

        void Update()
        {
            if (switchStage)
            {
                if (Input.GetKeyDown(KeyCode.Return))
                {
                    if (enterFlag)
                        return;

                    enterFlag = true;
                    PlayerPrefs.SetInt(GeneralInfo.saveMap, nowOption);
                    StartCoroutine(BtnPlay());
                }

                if (Input.GetKeyDown(KeyCode.D))
                    NextOption();
                if (Input.GetKeyDown(KeyCode.A))
                    PreviousOption();
                if (Input.GetKeyDown(KeyCode.W))
                    PreviousOption();
                if (Input.GetKeyDown(KeyCode.S))
                    NextOption();


                // 快速进入控制
                if (Input.GetKeyDown(KeyCode.F3))
                {
                    if (MainTheme.instance)
                        MainTheme.instance.GoodBye();
                    PlayerPrefs.SetInt(GeneralInfo.saveMap, nowOption);
                    PlayerPrefs.SetInt(GeneralInfo.saveAircraft, -99);
                    SceneManager.LoadScene(((Stage)PlayerPrefs.GetInt(GeneralInfo.saveMap)).ToString());
                }
            }
        }
        override internal void NextOption()
        {
            nowOption++;
            nowOption = (int)Mathf.Repeat(nowOption, countOption);
            //shutter.Shot();
        }
        override internal void PreviousOption()
        {
            nowOption--;
            nowOption = (int)Mathf.Repeat(nowOption, countOption);
            //shutter.Shot();
        }
        override internal void SwitchOption()
        {
            toggleOption[nowOption].isOn = true;
            if (enterFlag)
            {
                toggleOption[nowOption].isOn = false;
                GetComponent<AudioSource>().Play();
            }
        }
    }
}
