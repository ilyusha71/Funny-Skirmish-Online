using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TestAim : MonoBehaviour
{
    public Transform mk;
    public Transform ss;
    Camera followCam;
    Rigidbody myRigidbody;
    public Vector3 positionTarget;
    Quaternion mainRot;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        followCam = Camera.main;
    }
    void Update()
    {

        //positionTarget = Vector3.Lerp(positionTarget, followCam.ViewportToWorldPoint(new Vector3(followCam.ScreenToViewportPoint(Input.mousePosition).x, followCam.ScreenToViewportPoint(Input.mousePosition).y, followCam.farClipPlane)), Time.fixedDeltaTime * 10);
        //Vector3 targetPos = followCam.ViewportToWorldPoint(new Vector3(followCam.ScreenToViewportPoint(Input.mousePosition).x, followCam.ScreenToViewportPoint(Input.mousePosition).y, followCam.farClipPlane));
        Vector3 mousePos = followCam.ScreenToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, followCam.farClipPlane));
        mk.position = Input.mousePosition;

        //ss.localPosition = positionTarget;
        //Vector2 screenPoint = followCam.WorldToScreenPoint(targetPos);


        Vector3 relativePoint = this.transform.InverseTransformPoint(mousePos).normalized; // 计算相对位置的单位向量

        //mainRot = Quaternion.LookRotation(positionTarget - this.transform.position);
        //myRigidbody.rotation = Quaternion.Lerp(myRigidbody.rotation, mainRot, Time.fixedDeltaTime * (50 * 0.0005f) * 1);
        myRigidbody.rotation *= Quaternion.Euler(-relativePoint.y * 2, relativePoint.x * 1, -relativePoint.x * 3- myRigidbody.rotation.z);
        //float rollax = myRigidbody.rotation.z;
        //if (rollax != 0f)
        //{
        //    myRigidbody.rotation *= Quaternion.Euler(0, 0, -rollax);
        //}
        //根据单位向量分配 Pitch与 Roll的转动量
        //velocityTarget = (myRigidbody.rotation * Vector3.forward) * (dataSpeed.Value);



        //ss.localPosition = cam.ViewportToWorldPoint(new Vector3(Input.mousePosition.x, Input.mousePosition.y, cam.farClipPlane));
        //ss.localPosition = cam.ViewportToWorldPoint(new Vector3(cam.ScreenToViewportPoint(Input.mousePosition).x,cam.ScreenToViewportPoint(Input.mousePosition).y, cam.farClipPlane));


    }
}
