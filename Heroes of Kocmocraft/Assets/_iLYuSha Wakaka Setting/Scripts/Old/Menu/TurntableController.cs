using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurntableController : MonoBehaviour
{
    public Transform turntable;
    public Transform[] iconAircraft;
    public Transform[] tempPos; // 0 = 4th quadrant, 1 = 1st quadrant, etc.
    Quaternion targetRotation;
    bool moving;

	void Update ()
    {
        if (moving)
            turntable.rotation = Quaternion.Lerp(turntable.rotation, targetRotation, Time.deltaTime * 5);
    }

    public void Initial(int countOption, int nowOption)
    {
        for (int i = 0; i < countOption; i++)
        {
            if (i != nowOption)
            {
                iconAircraft[i].position = tempPos[2].position;
                iconAircraft[i].rotation = Quaternion.Euler(0, 0, 180);
            }
        }
        iconAircraft[nowOption].position = tempPos[3].position;
        iconAircraft[nowOption].rotation = Quaternion.Euler(0, 0, -90);
        targetRotation = turntable.rotation * Quaternion.Euler(0, 0, 90);
        moving = true;
    }

    public void Next(int countOption, int nowOption, int lastOption)
    {
        for (int i = 0; i < countOption; i++)
        {
            if (i != nowOption)
            {
                iconAircraft[i].position = tempPos[2].position;
                iconAircraft[i].rotation = Quaternion.Euler(0, 0, 180);
            }
        }
        iconAircraft[lastOption].position = tempPos[0].position;
        iconAircraft[lastOption].rotation = Quaternion.Euler(0, 0, 0);
        iconAircraft[nowOption].position = tempPos[3].position;
        iconAircraft[nowOption].rotation = Quaternion.Euler(0, 0, -90);
        targetRotation = turntable.rotation * Quaternion.Euler(0, 0, 90);
        moving = true;
    }

    public void Previous(int countOption, int nowOption, int lastOption)
    {
        for (int i = 0; i < countOption; i++)
        {
            if (i != nowOption)
            {
                iconAircraft[i].position = tempPos[2].position;
                iconAircraft[i].rotation = Quaternion.Euler(0, 0, 180);
            }
        }
        iconAircraft[lastOption].position = tempPos[0].position;
        iconAircraft[lastOption].rotation = Quaternion.Euler(0, 0, 0);
        iconAircraft[nowOption].position = tempPos[1].position;
        iconAircraft[nowOption].rotation = Quaternion.Euler(0, 0, 90);
        targetRotation = turntable.rotation * Quaternion.Euler(0, 0, -90);
        moving = true;
    }
}
