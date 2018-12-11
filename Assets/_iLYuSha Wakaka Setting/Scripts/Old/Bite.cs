using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bite : MonoBehaviour
{
    public Transform left;
    public Transform right;

    public float maxAngle;
    Quaternion iniRotation;
    public Quaternion[] maxRotation = new Quaternion[2];
    bool open;

    void Awake()
    {
        iniRotation = left.localRotation;
        maxRotation[0] = left.localRotation * Quaternion.Euler(0, 0, -maxAngle);
        maxRotation[1] = right.localRotation * Quaternion.Euler(0, 0, maxAngle);
    }

    void Update ()
    {
        if (open)
        {
            left.localRotation = Quaternion.Lerp(left.localRotation, maxRotation[0], Time.deltaTime * 0.7f);
            right.localRotation = Quaternion.Lerp(right.localRotation, maxRotation[1], Time.deltaTime * 0.7f);
            if (Mathf.Abs(left.localRotation.eulerAngles.z - maxRotation[0].eulerAngles.z) < 4.0f)
            {
                open = false;
            }
        }
        else
        {
            left.localRotation = Quaternion.Lerp(left.localRotation, iniRotation, Time.deltaTime * 7.0f);
            right.localRotation = Quaternion.Lerp(right.localRotation, iniRotation, Time.deltaTime * 7.0f);
            if (Mathf.Abs(left.localRotation.eulerAngles.z - iniRotation.eulerAngles.z) < 2.0f)
            {
                open = true;
            }
        }
	}
}
