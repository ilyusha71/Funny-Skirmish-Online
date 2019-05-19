using UnityEngine;

[CreateAssetMenu(fileName = "Engine Data", menuName = "KocmocA Data/Create Engine Data")]
[System.Serializable]
public class Engine : ScriptableObject
{
    public AudioClip sound;
    public float maxVolume, minVolume, maxPitch, minPitch;
}