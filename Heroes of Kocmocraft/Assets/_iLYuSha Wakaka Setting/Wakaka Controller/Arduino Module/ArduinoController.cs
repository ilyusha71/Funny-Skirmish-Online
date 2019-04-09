/**************************************************************************************** 
 * Wakaka Studio 2017
 * Author: iLYuSha Dawa-mumu Wakaka Kocmocovich Kocmocki KocmocA
 * Project: 0escape Medieval - Arduino Controller
 * Tools: Unity 5.6 + Arduino Mega2560
 * Version: Arduino Module v3.6c
 * Last Updated: 2017/11/28 - 更新資源釋放
 ****************************************************************************************/
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.IO.Ports;
using System.Threading;
using System;

public class ArduinoController : MonoBehaviour
{
    public static ArduinoController Instance { get; private set; }
    // 序列阜與執行緒
    private static readonly string PORT_SETTING = "Port Setting";
    private static readonly string BAUD_SETTING = "Baud Setting";
    private static readonly int[] valueBaud = new int[] { 9600, 4800, 9600, 19200, 38400 };
    private bool isReady, isConnected, isAborting; // 是否設定Port，是否已連接，是否正在斷開
    private int timeBoot, timeUnload;
    private float bufferDestroy = 60, countdownDestroy, timeDestroy; // 2018.11.07 自動銷毀
    private static SerialPort connectorArduino;
    public string SerialPort { get; protected set; } = "COM9";
    public int SerialRate { get; protected set; } = 9600;
    private Thread arduinoThread;

    // 熱鍵控制
    public int timesPressESC;
    public int timesPressDelete;

    [Header("UI - Arduino Serial Port Setting")]
    public GameObject panelSetting;
    public GameObject listPort;
    public GameObject listRate;
    Toggle[] optionPort;
    Toggle[] optionRate;
    public Text textTime;
    public Text textDestroyCD;

    [Header("Messages")]
    public MessageBox msgBox;
    public static Queue<string> queueMsg = new Queue<string>();
    public static Queue<string[]> queueCommand = new Queue<string[]>();
    static string arduinoMsg;
    static string[] commands;

    [Header("Project Setting")]
    [SerializeField]
    public static int commandsCount;
    [SerializeField]
    public static bool msgQueueCombine = false; // 訊息處理合併

    void Awake()
    {
        if (Instance == null) Instance = this;
        DontDestroyOnLoad(this);
        SceneManager.LoadScene(PlayerPrefs.GetInt("MainScene"));

        Initialize();
    }

    void Initialize()
    {
        int indexCom = PlayerPrefs.GetInt(PORT_SETTING);
        int indexRate = PlayerPrefs.GetInt(BAUD_SETTING);

        // Initialize
        optionPort = listPort.GetComponentsInChildren<Toggle>();
        optionPort[indexCom].isOn = true;
        optionRate = listRate.GetComponentsInChildren<Toggle>();
        optionRate[indexRate].isOn = true;
        panelSetting.SetActive(false);

        SerialPort = "COM" + indexCom;
        SerialRate = valueBaud[indexRate];
        isReady = indexCom == 0 ? false : true;

        timeUnload = 600;
        timeDestroy = Time.time + bufferDestroy;
    }

    public void ChangePort(int com)
    {
        if (optionPort[com].isOn)
        {
            SerialPort = "COM" + com;
            PlayerPrefs.SetInt(PORT_SETTING, com);
            msgBox.Keyword("<color=lime>重設Serial Port為 </color>" + SerialPort);
        }
    }

    public void ChangeRate(int baud)
    {
        if (optionRate[baud].isOn)
        {
            SerialRate = valueBaud[baud];
            PlayerPrefs.SetInt(BAUD_SETTING, baud);
            msgBox.Keyword("<color=lime>重設Serial Baud為 </color>" + SerialRate);
        }
    }

    void Start()
    {
        if (isReady) ConnectArduino();
    }

    private void Update()
    {
        InputControl();

        if (Time.time > timeUnload)
        {
            timeUnload += 60;
            Resources.UnloadUnusedAssets();
            GC.Collect();
        }

        // Msg Box
        if (!msgQueueCombine)
        {
            int countMsg = queueMsg.Count;
            for (int i = 0; i < countMsg; i++) { msgBox.AddNewMsg(queueMsg.Dequeue().Replace("Wakaka/", "")); }
        }

        if (isConnected)
        {
            if (arduinoThread.IsAlive)
                timeDestroy = Time.time + bufferDestroy;
            else
            {
                if (isAborting)
                {
                    isAborting = false;
                    isConnected = false;
                    msgBox.Keyword("<color=lime>Arduino中斷完成</color> at " + (int)Time.time);
                }
                else
                    msgBox.Keyword("<color=lime>執行緒中止</color> at " + (int)Time.time);
            }
        }
        else
        {
            AutoDestroyCountDown();
        }
        textTime.text = timeBoot + "\n0\n" + (int)Time.time;
    }

    public void ArduinoMsg(string msg)
    {
        msgBox.AddNewMsg(msg.Replace("Wakaka/", ""));
    }

    #region Arduino
    public void ConnectArduino()
    {
        if (isConnected)
        {
            msgBox.Keyword("<color=lime>已連接Arduino</color> " + connectorArduino.PortName + " " + connectorArduino.BaudRate);
            return;
        }
        if (isAborting)
        {
            msgBox.Keyword("<color=lime>請等待Arduino完成中斷後再重新連接</color>");
            return;
        }

        msgBox.Keyword("<color=lime>開始連接Arduino</color> " + SerialPort + " " + SerialRate);
        connectorArduino = new SerialPort(SerialPort, SerialRate);
        try
        {
            connectorArduino.Open();
            arduinoThread = new Thread(new ThreadStart(GetArduino));
            arduinoThread.Start();
            timeBoot = (int)Time.time;
            textTime.text = timeBoot + "\n---\n" + timeBoot;
            connectorArduino.WriteLine("R");
            msgBox.Keyword("<color=lime>開始接收訊號</color>");
            isConnected = true;
        }
        catch (Exception ex)
        {
            msgBox.Keyword("<color=lime>連接失敗</color>");
            Debug.LogWarning("Start Error : " + ex);
        }
    }

    private void GetArduino()
    {
        while (arduinoThread.IsAlive && !isAborting)
        {
            if (connectorArduino.IsOpen)
            {
                try
                {
                    bool emptyMsg = true;
                    arduinoMsg = connectorArduino.ReadLine();
                    Debug.LogWarning(arduinoMsg);
                    if (string.IsNullOrEmpty(arduinoMsg)) Debug.LogWarning("empty");
                    string check = arduinoMsg.Split('/')[0];
                    string msg = arduinoMsg.Replace("Wakaka/", "");
                    commands = msg.Split('/');
                    int countCommands = commands.Length;
                    if (countCommands == commandsCount)
                    {
                        if (check == "Wakaka" || check == "Reset")
                        {
                            for (int i = 1; i < countCommands; i++)
                            {
                                if (commands[i] != "")
                                    emptyMsg = false;
                            }
                            if (!emptyMsg)
                            {
                                queueMsg.Enqueue(msg);
                                queueCommand.Enqueue(commands);
                            }
                        }
                    }
                }
                catch (System.Exception ex)
                {
                    Debug.Log("Run Error : " + ex);
                }
            }
        }
    }

    public void DisconnectArduino()
    {
        if (!isConnected)
        {
            msgBox.Keyword("<color=lime>尚未與Arduino完成連接，無須中斷</color>");
            return;
        }
        msgBox.Keyword("<color=lime>開始中斷Arduino的連接</color> " + connectorArduino.PortName + " " + connectorArduino.BaudRate);
        if (arduinoThread != null)
        {
            if (arduinoThread.IsAlive)
            {
                isAborting = true;
                connectorArduino.Close();
                Thread.Sleep(1000);
                arduinoThread.Abort();
                msgBox.Keyword("<color=lime>執行緒狀態：</color> " + arduinoThread.IsAlive);
                Debug.Log("Thread isAlive ? " + arduinoThread.IsAlive);
            }
            else
            {
                msgBox.Keyword("<color=lime>執行緒已中止</color>");
                Debug.Log("Aborting thread failed");
            }
        }
    }

    public void RebootArduino()
    {
        DisconnectArduino();
        msgBox.Keyword("<color=lime>已暫時與Arduino斷開，將於3秒後重新連結</color>");
        StartCoroutine(Restart());
    }

    IEnumerator Restart()
    {
        yield return new WaitForSeconds(3.0f);
        ConnectArduino();
    }

    void OnApplicationQuit()
    {
        connectorArduino.Close();
        Thread.Sleep(1000);
        arduinoThread.Abort();
        Quit();
    }

    public void Quit()
    {
        Application.Quit();
    }

    void AutoDestroyCountDown()
    {
        countdownDestroy = timeDestroy - Time.time;
        textDestroyCD.text = countdownDestroy.ToString("0.0");
        if (countdownDestroy < 0)
        {
            if (!isAborting)
                DisconnectArduino();
            if (arduinoThread != null)
            {
                if (!arduinoThread.IsAlive)
                {
                    Debug.LogWarning("Kill Thread");
                    Destroy(gameObject);
                }
            }
            else
            {
                Debug.LogWarning("No Thread");
                Destroy(gameObject);
            }
        }
    }
    #endregion

    void InputControl()
    {
        // 開啟Arduino監控窗
        if (Input.GetKeyDown(KeyCode.F10) || Input.GetKeyDown(KeyCode.KeypadMinus))
            panelSetting.SetActive(!panelSetting.activeSelf);

        // 重啟遊戲
        if (Input.GetKey(KeyCode.Escape))
        {
            timesPressESC++;
            if (timesPressESC > 150)
            {
                if (arduinoThread != null) if (!arduinoThread.IsAlive) connectorArduino.Write("R");
                msgBox.Keyword("<color=lime>遊戲重新啟動</color>");
                timesPressESC = 0;
                SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
            }
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
            timesPressESC = 0;

        // 刪除設定
        if (Input.GetKey(KeyCode.Delete))
        {
            timesPressDelete++;
            if (timesPressDelete > 150)
            {
                PlayerPrefs.DeleteKey(PORT_SETTING);
                PlayerPrefs.DeleteKey(BAUD_SETTING);
                optionPort[0].isOn = true;
                optionRate[0].isOn = true;
                msgBox.Keyword("<color=lime>已刪除設定檔</color>");
            }
        }
        else if (Input.GetKeyUp(KeyCode.Escape))
            timesPressDelete = 0;
    }
}