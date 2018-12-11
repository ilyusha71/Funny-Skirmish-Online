using UnityEngine;
using UnityEngine.UI;

public class MissionTimer : MonoBehaviour
{
    public Text textName;
    public Text textValue;
    private float missionStart;
    public float Timer
    {
        set
        {
            missionStart = value;
        }
        get
        {
            return missionStart;
        }
    }
}
