using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HarmonicMotion : MonoBehaviour
{
    public Vector3 offset;
    public float period;
    Vector3 motion;
    float angle;
    private Vector3 ini;

    void Start()
    {
        ini = transform.position;
    }

	void Update ()
    {
        angle += period * Mathf.PI * 2 *Time.deltaTime;
        angle %= 2 * Mathf.PI;
        motion = offset * Mathf.Sin(angle);
        transform.position = ini + motion;
    }
}
