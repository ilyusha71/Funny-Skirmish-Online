using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName ="MyAudioClip",menuName = "Wakaka Tools/Create ScriptableObject")]
public class FCSAudioClip : ScriptableObject
{
    public AudioClip Turret;
    public AudioClip Rocket;
    public AudioClip Missile;

}
