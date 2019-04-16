using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempAngle : MonoBehaviour
{
    public Transform Ori;
    public Transform target;
    public float angle;

    void Update()
    {
        angle = Vector3.Angle(target.position - Ori.position, Ori.forward);
    }
}
