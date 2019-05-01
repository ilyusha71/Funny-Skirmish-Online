using Kocmoca;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class TempCal : MonoBehaviour
{
    public AudioClip soundRailgun;
    float TurretCount = 2;
    float TurretCountX = 4;
    float OP1;
    float OP2;
    float OP3;
    public GameObject lllsad;
    [Header("Formation Data")]
    public Transform[] kocmoWing; // 宇航聯隊 = 10 宇航團（10名玩家）
    public Transform[] kocmoGroup; // 宇航團 = 5 宇航中隊（玩家為長機，其餘僚機為Bot）
    public Transform[] kocmoSquadron; // 宇航中隊 = 1架主力宇航機 + 4架迷你特攻機
    private Vector3[] portalPos; // 中隊傳送點
    private Quaternion[] portalRot;


    public Text QQQQQ1516;

    string xxx;
    int QQ;

    ModuleData data;


    void Start()
    {
        data = KocmocaData.MinionArmor;
        xxx = KocmocaData.MinionArmor.RadarName;
        QQ = KocmocaData.MinionArmor.MaxLockDistance;
    }
    private void Update()
    {
        for (int k = 0; k < 100; k++)
        {
            TTT();
        }



    }


    void TestA()
    {
        Send2(xxx, QQ);
    }
    void TestB()
    {
        Send(data.RadarName, data.MaxLockDistance);

    }
    void Send(string text, int trange)
    {
        QQQQQ1516.text = trange.ToString() + text;
    }
    void Send2(string text, int trange)
    {
        QQQQQ1516.text = trange + text;
    }

    void A1()
    {
        OP1 += TurretCount;
        OP1 += TurretCountX;
    }
    void A2()
    {
        OP2 += KocmocaData.MinionArmor.TurretCount;
        OP2 += KocmocaData.RedBullEnergy.TurretCount;

    }
    void A3()
    {
        OP3 += KocmocaData.KocmocraftData[0].TurretCount;
        OP3 += KocmocaData.KocmocraftData[1].TurretCount;

    }
    void D1()
    {
        KocmocaData.MinionArmor.FireSound = soundRailgun;
        KocmocaData.RedBullEnergy.FireSound = soundRailgun;
        KocmocaData.VladimirPutin.FireSound = soundRailgun;
        KocmocaData.PaperAeroplane.FireSound = soundRailgun;
        KocmocaData.Cuckoo.FireSound = soundRailgun;
        KocmocaData.BulletBill.FireSound = soundRailgun;
        KocmocaData.TimeMachine.FireSound = soundRailgun;
        KocmocaData.KirbyStar.FireSound = soundRailgun;
        KocmocaData.AceKennel.FireSound = soundRailgun;
        KocmocaData.ScorpioRouge.FireSound = soundRailgun;
        KocmocaData.nWidia.FireSound = soundRailgun;
        KocmocaData.FastFoodMan.FireSound = soundRailgun;
        KocmocaData.ReindeerTransport.FireSound = soundRailgun;
        KocmocaData.PolarisExpress.FireSound = soundRailgun;
        KocmocaData.AncientFish.FireSound = soundRailgun;
        KocmocaData.PapoyUnicorn.FireSound = soundRailgun;
        KocmocaData.PumpkinGhost.FireSound = soundRailgun;
        KocmocaData.BoundyHunterMKII.FireSound = soundRailgun;
        KocmocaData.InuitEagle.FireSound = soundRailgun;
        KocmocaData.GrandLisboa.FireSound = soundRailgun;

    }
    void D2()
    {
        for (int i = 0; i < 20; i++)
        {
            KocmocaData.KocmocraftData[i].FireSound = soundRailgun;
        }
    }

    void D3()
    {
        for (int i = 0; i < 20; i++)
        {
            //KocmocaData.KOC[i].FireSound = soundRailgun;
        }
    }

    void Same(int portNumber)
    {
        int fac = portNumber % 2;
        for (int i = 0; i < 2; i++)
        {
            if (i == portNumber % 2)
                TempStatic.instance.arrayOne[portNumber / 2] = transform;
            else
                TempStatic.instance.arrayTwo[portNumber / 2] = transform;
        }
    }
    void TTT()
    {
        for (int i = 0; i < 100; i++)
        {
            Same(i);
            SameMain(i);
            CA(i);
            CAMain(i);
        }
    }
    void CA(int portNumber)
    {
        int fac = portNumber % 2;

        for (int i = 0; i < 2; i++)
        {
            if (i == fac)
                TempStatic.instance.arrayOne[portNumber / 2] = transform;
            else
                TempStatic.instance.arrayTwo[portNumber / 2] = transform;
        }
    }

    void SameMain(int portNumber)
    {
        int fac = portNumber % 2;
        TempStatic.instance.AddSearchList(portNumber, transform);
    }
    void CAMain(int portNumber)
    {
        int fac = portNumber % 2;
        TempStatic.instance.AddSearchListCA(fac, portNumber / 2, transform);
    }
}
