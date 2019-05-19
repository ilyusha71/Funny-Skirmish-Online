using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Animation))]
public class Propeller : MonoBehaviour
{
    public Animation rotation;
    public AnimationClip Clip;
    public float Omega;
    public bool Reverse;


    private void Reset()
    {
        rotation = GetComponent<Animation>();
        rotation.clip = Clip;
        rotation.AddClip(Clip, Clip.name);
    }
    public void Power(float power)
    {
        rotation[Clip.name].speed = Reverse ? -Omega * power : Omega * power;
    }
}