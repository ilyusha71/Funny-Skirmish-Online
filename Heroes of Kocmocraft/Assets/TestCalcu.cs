using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestCalcu : MonoBehaviour
{
    Vector3 mousePos;
    Vector3 relativePoint;
    Camera followCam;

    Vector3 qqq =  new Vector3(999, 999, 999);
    private void Awake()
    {
        followCam = Camera.main;
    }

    private void Update()
    {
        for (int i = 0; i < 18000; i++)
        {
            Mouse();
            Mouse2();
            //Mouse3();
        }
    }

    void Mouse()
    {
        mousePos = Vector3.one + Vector3.one + Vector3.one + Vector3.one + Vector3.one + Vector3.one;
        //mousePos = followCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, followCam.farClipPlane));
        //relativePoint = this.transform.InverseTransformPoint(mousePos).normalized; // 计算相对位置的单位向量
    }

    void Mouse2()
    {
        Vector3 mousePos2 = Vector3.one + Vector3.one + Vector3.one + Vector3.one + Vector3.one + Vector3.one;
        //Vector3 mousePos2 = new Vector3(9, 4, 3);
        //mousePos2= followCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, followCam.farClipPlane));
        //Vector3 relativePoint2 = this.transform.InverseTransformPoint(mousePos2).normalized; // 计算相对位置的单位向量
    }
    void Mouse3()
    {
        transform.position = qqq;
    }
}
