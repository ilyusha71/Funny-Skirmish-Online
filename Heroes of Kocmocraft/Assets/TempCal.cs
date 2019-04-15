using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempCal : MonoBehaviour
{
    public GameObject lllsad;
    [Header("Formation Data")]
    public Transform[] kocmoWing; // 宇航聯隊 = 10 宇航團（10名玩家）
    public Transform[] kocmoGroup; // 宇航團 = 5 宇航中隊（玩家為長機，其餘僚機為Bot）
    public Transform[] kocmoSquadron; // 宇航中隊 = 1架主力宇航機 + 4架迷你特攻機
    private Vector3[] portalPos; // 中隊傳送點
    private Quaternion[] portalRot;
    void Start()
    {
        portalPos = new Vector3[100];
        portalRot = new Quaternion[100];
        for (int i = 0; i < 100; i++)
        {

            GameObject go = Instantiate(lllsad);
            go.name = "Wakaka " + i;
            // squadron = i%20;
            // group = (i/2)%10;
            // wing = i%2;
            Vector3 offset = kocmoGroup[(int)(i * 0.5f) % 10].localPosition;
            Debug.LogWarning(go.name + " off/ " + offset);
            portalPos[i] = kocmoGroup[(int)(i * 0.5f) % 10].InverseTransformPoint(kocmoSquadron[i / 20].position);
            Debug.LogWarning(go.name + " ori/ " + portalPos[i]);
            //portalPos[i] += offset;
            Debug.Log(go.name + " / " + portalPos[i]);
            portalPos[i] = kocmoWing[i % 2].TransformPoint(portalPos[i]);
            Debug.Log(go.name + " / " + portalPos[i]);
            portalRot[i] = kocmoWing[i % 2].rotation;
            go.transform.SetPositionAndRotation(portalPos[i], portalRot[i]);
        }
    }

}
