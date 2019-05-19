using UnityEngine;

[CreateAssetMenu(fileName = "Dubi Data", menuName = "KocmocA Data/Create Dubi Data")]
[System.Serializable]
public class Dubi : ScriptableObject
{
    public string pilot;
    [TextArea(3, 7)]
    public string resume;
    public int height;
}