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
}
