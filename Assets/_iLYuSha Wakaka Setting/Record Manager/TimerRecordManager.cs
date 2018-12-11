/**************************************************************************************** 
 * Wakaka Studio 2017
 * Author: iLYuSha Dawa-mumu Wakaka Kocmocovich Kocmocki KocmocA
 * Package: Timer Record Manager
 * Tools: Unity 5.6
 * Last Updated: 2017/11/15
 ****************************************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class TimerRecordManager : GameRecordManager
{
    public Transform blockMission;
    public GameObject missionType;
    public GameObject missionTimer;
    private string saveRecord;
    private string saveDate;
    private float timeStart, timeEnd;
    private string RankColor(int rank)
    {
        return TextCustom.TextGoldColor(rank+". ");
    }
    private int rankSize = 27;
    private int timerSize = 37;
    private Dictionary<string, MissionTimer> listMissionTimer = new Dictionary<string, MissionTimer>();

    void Awake()
    {
        saveRecord = SceneManager.GetActiveScene().name + "Timer";
        saveDate = SceneManager.GetActiveScene().name + "Date";
        LoadingData();
    }
    private void Update()
    {
        if(Input.GetKeyDown(KeyCode.F12))
            recordCanvas.SetActive(!recordCanvas.activeSelf);
        if (Input.GetKeyDown(KeyCode.Escape) || Input.GetKeyDown(KeyCode.Q))
            StartCoroutine(GetComponent<Save>().Screenshot(recordCanvas));

        if (timeEnd == 0)
        {
            if (timeStart != 0)
                textRealtime.text = MinuteTransform((int)(Time.time - timeStart), timerSize);
            else
                textRealtime.text = "---";
        }
        else
            textRealtime.text = MinuteTransform((int)timeEnd, timerSize);

        if (Input.GetKey(KeyCode.K))
        {
            if (Input.GetKeyDown(KeyCode.Alpha1))
                DeleteRecord(0);
            else if (Input.GetKeyDown(KeyCode.Alpha2))
                DeleteRecord(1);
            else if (Input.GetKeyDown(KeyCode.Alpha3))
                DeleteRecord(2);
            else if (Input.GetKeyDown(KeyCode.Alpha4))
                DeleteRecord(3);
            else if (Input.GetKeyDown(KeyCode.Alpha5))
                DeleteRecord(4);
            else if (Input.GetKeyDown(KeyCode.Alpha6))
                DeleteRecord(5);
            else if (Input.GetKeyDown(KeyCode.Alpha7))
                DeleteRecord(6);
            else if (Input.GetKeyDown(KeyCode.Alpha8))
                DeleteRecord(7);
            else if (Input.GetKeyDown(KeyCode.Alpha9))
                DeleteRecord(8);
        }
    }
    void OnApplicationQuit()
    {
        ScreenCapture.CaptureScreenshot(
            "Screenshot " +
            DateTime.Now.Year + "." +
            DateTime.Now.Month + "." +
            DateTime.Now.Day + " - " +
            DateTime.Now.Hour + "." +
            DateTime.Now.Minute + "." +
            DateTime.Now.Second + ".png");
    }
    string MinuteTransform(int time)
    {
        int min = Mathf.FloorToInt(time / 60.0f);
        int sec = time % 60;
        return min + " min " + sec + " sec";
    }
    string MinuteTransform(int time, int size)
    {
        int min = Mathf.FloorToInt(time / 60.0f);
        int sec = time % 60;
        return min + TextCustom.TextSize(size, " min ") +sec + TextCustom.TextSize(size, " sec");
    }
    void LoadingData()
    {
        textRecord.text = "";
        for (int i = 0; i < 9; i++)
        {
            int sec = PlayerPrefs.GetInt(saveRecord + i);
            string date = PlayerPrefs.GetString(saveDate + i);
            loadRecord[i] = sec;
            loadDate[i] = date;
            if (sec != 0 && sec != iniRecord)
                textRecord.text += RankColor(i + 1) + MinuteTransform(sec,rankSize)+ date + "\n";
            else
            {
                loadRecord[i] = iniRecord;
                textRecord.text += RankColor(i + 1) + "---\n";
            }
        }
        newRecord = new int[9];
        newDate = new string[9];
    }
    public void AddMissionType(string nameType)
    {
        GameObject go = Instantiate(missionType);
        go.transform.SetParent(blockMission);
        go.transform.localScale = new Vector3(1, 1, 1);
        go.GetComponentInChildren<Text>().text = nameType;
    }
    public void AddMission(string nameMission)
    {
        GameObject go = Instantiate(missionTimer);
        go.transform.SetParent(blockMission);
        go.transform.localScale = new Vector3(1, 1, 1);
        MissionTimer timer = go.GetComponent<MissionTimer>();
        timer.textName.text = nameMission;
        timer.textValue.text = "---";
        listMissionTimer.Add(nameMission, timer);
    }
    public void GameStart()
    {
        timeStart = Time.time;
    }
    public void MissionStart(string nameMission)
    {
        listMissionTimer[nameMission].Timer = Time.time;
    }
    public void MissionRecord(string nameMission)
    {
        float start = listMissionTimer[nameMission].Timer;
        listMissionTimer[nameMission].textValue.text = MinuteTransform((int)(Time.time-start),timerSize);
    }
    public void GameFinish()
    {
        textRecord.text = "";
        timeEnd = Time.time - timeStart;
        bool setRecord = false;
        for (int i = 0; i < 9; i++)
        {
            // 更新舊紀錄
            if (setRecord)
            {
                newRecord[i] = loadRecord[i - 1];
                newDate[i] = loadDate[i - 1];
                UpdateRecord(i);
            }
            else
            {
                // 刷新紀錄
                if (timeEnd < loadRecord[i])
                {
                    setRecord = true;
                    newRecord[i] = (int)timeEnd;
                    dateNow = TextCustom.TextColor("yellow",
                        TextCustom.TextSize(37,
                        " --- " +
                        DateTime.Now.Year + "." +
                        DateTime.Now.Month + "." +
                        DateTime.Now.Day + " - " +
                        DateTime.Now.Hour + ":" +
                        DateTime.Now.Minute + ":" +
                        DateTime.Now.Second));
                    newDate[i] =dateNow;
                    SetRecord(i);
                }
                else
                {
                    newRecord[i] = loadRecord[i];
                    newDate[i] = loadDate[i];
                    UpdateRecord(i);
                }
            }
        }
    }
    void UpdateRecord(int index)
    {
        if (newRecord[index] != iniRecord)
        {
            PlayerPrefs.SetInt(saveRecord + index, newRecord[index]);
            PlayerPrefs.SetString(saveDate + index, newDate[index]);
            textRecord.text += RankColor(index + 1) + MinuteTransform(newRecord[index],rankSize) + newDate[index] + "\n";
        }
        else
            textRecord.text += RankColor(index + 1) + "---\n";
    }
    void SetRecord(int index)
    {
        PlayerPrefs.SetInt(saveRecord + index, newRecord[index]);
        PlayerPrefs.SetString(saveDate + index, newDate[index]);
        textRecord.text += RankColor(index + 1) + TextCustom.TextColor("lime", MinuteTransform(newRecord[index],timerSize)) + newDate[index] + "\n";
    }
    void DeleteRecord(int index)
    {
        LoadingData();
        for (int i = 0; i < 8; i++)
        {
            if (i < index)
            {
                newRecord[i] = loadRecord[i];
                newDate[i] = loadDate[i];
            }
            else
            {
                newRecord[i] = loadRecord[i + 1];
                newDate[i] = loadDate[i + 1];
            }
            PlayerPrefs.SetInt(saveRecord + i, newRecord[i]);
            PlayerPrefs.SetString(saveDate + i, newDate[i]);
        }
        // 最後一筆記錄刪除
        PlayerPrefs.DeleteKey(saveRecord + 8);
        PlayerPrefs.DeleteKey(saveDate + 8);
        LoadingData();
    }
}
