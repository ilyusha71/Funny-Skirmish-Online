/**************************************************************************************** 
 * Wakaka Studio 2017
 * Author: iLYuSha Dawa-mumu Wakaka Kocmocovich Kocmocki KocmocA
 * Package: Game Record Manager
 * Tools: Unity 5.6
 * Last Updated: 2017/11/15
 ****************************************************************************************/
using UnityEngine;
using UnityEngine.UI;

public class GameRecordManager : MonoBehaviour
{
    public GameObject recordCanvas;
    public Text textRecord, textRealtime;
    public int iniRecord = 86400;
    internal int[] loadRecord = new int[9];
    internal int[] newRecord = new int[9];
    internal string[] loadDate = new string[9];
    internal string[] newDate = new string[9];
    internal string dateNow;
}
