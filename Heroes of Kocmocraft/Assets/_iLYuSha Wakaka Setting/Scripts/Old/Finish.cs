using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class Finish : MonoBehaviour
{
    public AudioClip[] clips;
    public GameObject[] metal;
    public Text[] textName;
    public Text[] textKill;

    public Text end;
    public Text textRank;

    public Text sF1;
    public Text sF2;
    public Text sKill;

    int final;
    int leader = 99;
    bool enterFlag;
    void Awake()
    {
        if (ArduinoController.Instance == null)
        {
            PlayerPrefs.SetString("sScene", SceneManager.GetActiveScene().name);
            SceneManager.LoadScene("Arduino Controller");
        }

        //if (MainTheme.instance == null)
        //{
        //    PlayerPrefs.SetString("sScene", SceneManager.GetActiveScene().name);
        //    SceneManager.LoadScene("Main Theme");
        //}
        //iniShutterPos = shutter[0].transform.localPosition;
        //minShutterPos = iniShutterPos + new Vector3(shutter[0].rect.width * 2, 0, 0);
    }

    void Start ()
    {
        // 讀取戰績
        string myName = PlayerPrefs.GetString("sName");
        int myKill = PlayerPrefs.GetInt("sKill");

        // 讀取排行榜
        string[] name = new string[5];
        int[] kill = new int[5];

        for (int i = 0; i < 5; i++)
        {
            // 表示前面排名被刷新了
            if (leader != 99)
            {
                name[i] = PlayerPrefs.GetString("sLeaderName" + (i - 1));
                kill[i] = PlayerPrefs.GetInt("sLeaderKill" + (i - 1));
                textName[i].text = name[i];
                textKill[i].text = "" + kill[i];
            }
            else
            {
                name[i] = PlayerPrefs.GetString("sLeaderName" + i);
                kill[i] = PlayerPrefs.GetInt("sLeaderKill" + i );
                textName[i].text = name[i];
                textKill[i].text = "" + kill[i];

                // 刷新排名
                if (myKill > kill[i])
                {
                    leader = i;

                    name[i] = myName;
                    kill[i] = myKill;
                    textName[i].text = name[i];
                    textKill[i].text = "" + kill[i];
                }
            }
        }
        textName[5].text = myName;
        textKill[5].text = "" + myKill;


        // 如果進入排行榜存檔
        if (leader != 99)
        {
            for (int i = 0; i < 5; i++)
            {
                PlayerPrefs.SetString("sLeaderName" + i, name[i]);
                PlayerPrefs.SetInt("sLeaderKill" + i, kill[i]);
            }
        }
        string rank;
        if (myKill >= 20)
            rank = "S+";
        else if (myKill >= 18)
            rank = "S";
        else if (myKill >= 15)
            rank = "S-";
        else if (myKill >= 12)
            rank = "A+";
        else if (myKill >= 10)
            rank = "A";
        else if (myKill >= 8)
            rank = "B";
        else if (myKill >= 5)
            rank = "C";
        else if (myKill >= 1)
            rank = "D";
        else
            rank = "E";
        textRank.text = rank;


        //Debug.Log(PlayerPrefs.GetInt("sTitle"));
        //sTitle.text = ((TypeTitle)PlayerPrefs.GetInt("sTitle")).ToString();
        //Debug.Log(PlayerPrefs.GetString("sName"));
        //sName.text = PlayerPrefs.GetString("sName");
        //sKill.text = "" + kill;


        // 讀取比數
        int m = PlayerPrefs.GetInt("sFaction");
        int f1 = PlayerPrefs.GetInt("sF1");
        int f2 = PlayerPrefs.GetInt("sF2");

        sF1.text = "" + f1;
        sF2.text = "" + f2;

        if (f1 > f2)
        {
            if (m == 0)
            {
                if (f1 - f2 >= 10)
                {
                    end.text = "絕對制空";
                    final = 1;
                }
                else
                {
                    end.text = "優勢制空";
                    final = 2;
                }
            }
            else
            {
                end.text = "領空失守";
                final = 4;
            }

        }
        else if (f1 < f2)
        {
            if (m == 1)
            {
                if (f2 - f1 >= 10)
                {
                    end.text = "絕對制空";
                    final = 1;
                }
                else
                {
                    end.text = "優勢制空";
                    final = 2;
                }
            }
            else
            {
                end.text = "領空失守";
                final = 4;
            }
        }
        else
        {
            end.text = "勢均力敵";
            final = 3;
        }

        if (final == 1)
        {
            GetComponent<AudioSource>().clip = clips[0];
            if (myKill >= 15)
                metal[0].SetActive(true);
            else if (myKill >= 12)
                metal[1].SetActive(true);
            else
                metal[2].SetActive(true);
        }
        else if (final == 2)
        {
            GetComponent<AudioSource>().clip = clips[0];
            if (myKill >= 12)
                metal[0].SetActive(true);
            else if (myKill >= 8)
                metal[1].SetActive(true);
            else
                metal[2].SetActive(true);
        }
        else if (final == 3)
        {
            GetComponent<AudioSource>().clip = clips[1];
            if (myKill >= 8)
                metal[0].SetActive(true);
            else if (myKill >= 5)
                metal[1].SetActive(true);
            else
                metal[2].SetActive(true);
        }
        else if (final == 4)
        {
            GetComponent<AudioSource>().clip = clips[2];

            if (myKill >= 8)
                metal[0].SetActive(true);
            else if (myKill >= 3)
                metal[1].SetActive(true);
            else
                metal[2].SetActive(true);
        }

        GetComponent<AudioSource>().Play();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Return))
        {
            SceneManager.LoadScene("Map Option");
            if (enterFlag)
                return;
            enterFlag = true;
            //ArduinoControllerX.instance.PlaySound(1);
        }

    }
}
