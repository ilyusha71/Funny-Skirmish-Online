/***************************************************************************
 * Camera Tracking System
 * 攝影機追踪系统
 * Last Updated: 2019/01/07
 * Description:
 * 1. 提供摄影机进行XY双轴旋转与Z轴缩放视角
 ***************************************************************************/
using UnityEngine;

public class CameraTrackingSystem : MonoBehaviour
{
    [Header("Camera Tracking System")]
    public Transform pivot;
    public Transform axisY;
    public Transform axisX;
    public Transform fixedX; // 避免360~0度产生的错误
    public Transform slider;
    [SerializeField] private float identity = -90; // X 偏转
    [SerializeField] private float highAngle = 0; // 最高俯仰角
    [SerializeField] private float lowAngle = 0; // 最低俯仰角
    [SerializeField] private  float nearView = 0;
    [SerializeField] private  float farView = 0;
    [Header("Realtime Tracking Parameter")]
    [SerializeField] protected float valueRotY;
    [SerializeField] protected float valueRotX;
    [SerializeField] protected float valuePosZ;

    protected void InitializeTrackingSystem()
    {
        fixedX.localRotation = Quaternion.Euler(identity, 0, 0);
    }

    protected void Control()
    {
        if (Input.GetKey(KeyCode.Mouse1))
        {
            valueRotY += Input.GetAxis("Mouse X") * 2;
            valueRotX -= Input.GetAxis("Mouse Y") * 2;
            valueRotX = Mathf.Clamp(valueRotX, lowAngle, highAngle);
        }
        axisY.localRotation = Quaternion.Euler(0, valueRotY, 0);
        axisX.localRotation = Quaternion.Euler(valueRotX, 0, 0);
        valuePosZ = Mathf.Lerp(valuePosZ, valuePosZ+ 10 * Input.GetAxis("Mouse ScrollWheel"),0.5f);
        valuePosZ = Mathf.Clamp(valuePosZ, farView, nearView);
        slider.localPosition = new Vector3(slider.localPosition.x, slider.localPosition.y, valuePosZ);
    }

    protected void ReturnToZero()
    {
        axisY.localRotation = Quaternion.identity;
        axisX.localRotation = Quaternion.Euler(-identity, 0, 0);
        slider.localPosition = Vector3.zero;
        slider.localRotation = Quaternion.identity;
    }
}
