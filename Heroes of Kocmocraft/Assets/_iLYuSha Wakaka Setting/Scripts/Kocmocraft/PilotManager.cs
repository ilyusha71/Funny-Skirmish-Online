/***************************************************************************
 * Pilot Manager
 * 飛行員管理
 * Last Updated: 2019/06/04
 *
 * v19.0606
 * 2. 提供Cockpit視角，隱藏玩家飛行員
 *
 * v19.0604
 * 1. 管理玩家選擇的飛行員
 ***************************************************************************/
using UnityEngine;

public class PilotManager : MonoBehaviour
{
    [Header ("Skin")]
    public GameObject[] dubi;
    private int countDubi, nowDubi = 0;
    private bool isCockpitView;

    private void Reset ()
    {
        countDubi = transform.childCount;
        dubi = new GameObject[countDubi];
        for (int i = 0; i < countDubi; i++)
        {
            dubi[i] = transform.GetChild (i).gameObject;
            dubi[i].SetActive (false);
        }
        dubi[0].SetActive (true);
    }
    public int ChangePilot ()
    {
        nowDubi = (int) Mathf.Repeat (++nowDubi, dubi.Length);
        for (int i = 0; i < dubi.Length; i++)
        {
            dubi[i].SetActive (false);
        }
        dubi[nowDubi].SetActive (!isCockpitView);
        return nowDubi;
    }
    // public void ShowPilot (bool show)
    // {
    //     dubi[nowDubi].SetActive (false);
    // }
}