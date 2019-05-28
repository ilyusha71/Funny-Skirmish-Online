using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAim : MonoBehaviour
{
    public RectTransform mk;
    public Transform ss;
    Camera followCam;
    Rigidbody myRigidbody;
    public Vector3 positionTarget;
    Quaternion mainRot;

    private Vector3 pivot; // Screen centre;
    Vector3 mousePos;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        followCam = Camera.main;
        pivot = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;
    }
    void Update()
    {
        mousePos = Vector3.ClampMagnitude(Input.mousePosition - pivot, Screen.height*0.37f);
        mk.anchoredPosition = mousePos;
        mousePos.z = 300;
        mousePos += pivot;
    }

    private void FixedUpdate()
    {
        Vector3 targetPos = followCam.ScreenToWorldPoint(mousePos);
        //ss.localPosition = mousePos;

        Vector3 relativePoint = this.transform.InverseTransformPoint(targetPos).normalized; // 计算相对位置的单位向量
        Debug.Log(relativePoint.ToString("F4"));
        myRigidbody.rotation *= Quaternion.Euler(-relativePoint.y * 2, relativePoint.x * 1, -relativePoint.x * 3);
        if(relativePoint.z>0.98f)
            myRigidbody.rotation *= Quaternion.Euler(-myRigidbody.rotation.eulerAngles.x, 0, -myRigidbody.rotation.eulerAngles.z);
        myRigidbody.velocity = (myRigidbody.rotation * Vector3.forward) * 30;
    }
}
