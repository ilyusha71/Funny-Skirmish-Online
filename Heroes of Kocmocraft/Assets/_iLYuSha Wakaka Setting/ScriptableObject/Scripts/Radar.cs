using UnityEngine;

[CreateAssetMenu(fileName = "Radar Data", menuName = "KocmocA Data/Create Radar Data")]
[System.Serializable]
public class Radar : ScriptableObject
{
    public  string type;
    public  string description;
    public  int radius;
    public  int radiusSqr;
    public  int angle;
    public  float range;

    public void Calculate()
    {
        radiusSqr = radius * radius;
        range = Mathf.Cos(angle * 0.5f * Mathf.Deg2Rad);
    }
}
