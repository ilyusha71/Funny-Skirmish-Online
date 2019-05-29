using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

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
    public float range;

    public float aoaYaw;
    public float aoaPitch;

    private void Start()
    {
        myRigidbody = GetComponent<Rigidbody>();
        followCam = Camera.main;
        range = Screen.height * 0.37f;
        pivot = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        aoaYaw = Mathf.Tan(7 * Mathf.Deg2Rad);
        aoaPitch = Mathf.Tan(7 * Mathf.Deg2Rad);
    }


    public enum ControlCore
    {
        LocalPlayer = 0,
        LocalBot = -1,
        RemotePlayer = 100,
        RemoteBot = 99,
    }
    private ControlCore core = ControlCore.LocalPlayer;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.L))
            range +=50;
        if (Input.GetKeyDown(KeyCode.K))
            range -= 50;


        if (core == ControlCore.LocalPlayer)
        {
            mousePos = Vector3.ClampMagnitude(Input.mousePosition - pivot, range) + pivot;
            mk.position = mousePos; // HUD

        }



    }

    //public float AngleYaw;
    //public float AnglePitch;

    //public float targetAngleYaw;
    //public float targetAnglePitch;
    //public float changeRoll;
    //public float changePitch;
    //public float changeYaw;

    public float roll=2;
    public float pitch=3;
    public float yaw=1;

    // float PitchAngle;
    // float RollAngle;
    //public float RollAngle2;



    private void FixedUpdate()
    {
        Ca1();
        Ca2();

        //myRigidbody.rotation *= Quaternion.Euler(changePitch * pitch, -changeYaw * yaw, changeRoll * roll);
        //myRigidbody.velocity = (myRigidbody.rotation * Vector3.forward) * 140;
    }

    public void Ca1()
    {
        for (int i = 0; i < count; i++)
        {
            mousePos.z = followCam.farClipPlane;
            var targetPos = followCam.ScreenToWorldPoint(mousePos);
            var localTarget = transform.InverseTransformPoint(targetPos);
            var changeRoll = Mathf.Clamp(-Mathf.Atan2(localTarget.x, localTarget.z), -aoaYaw, aoaYaw);
            var changePitch = Mathf.Clamp(-Mathf.Atan2(localTarget.y, localTarget.z), -aoaPitch, aoaPitch);
            var changeYaw = changeRoll;
            if (Mathf.Abs(changeRoll) < 0.07f)
            {
                var flatForward = transform.forward;
                flatForward.y = 0;
                // If the flat forward vector is non-zero (which would only happen if the plane was pointing exactly straight upwards)
                if (flatForward.sqrMagnitude > 0)
                {
                    flatForward.Normalize();
                    var flatRight = Vector3.Cross(Vector3.up, flatForward);
                    var localFlatRight = transform.InverseTransformDirection(flatRight);
                    changeRoll = Mathf.Atan2(localFlatRight.y, localFlatRight.x) * roll * 0.1f;
                }
            }
        }
    }
    public static int count = 100;
    public Transform[] main = new Transform[count];

    void JobSetting()
    {
        for (int i = 0; i < count; i++)
        {
            main[i] = transform;
            var targetPos = followCam.ScreenToWorldPoint(mousePos);
            var localTarget = transform.InverseTransformPoint(targetPos);
        }
    }
    public void Ca2()
    {
        for (int i = 0; i < 100; i++)
        {
            mousePos.z = followCam.farClipPlane;
            var targetPos = followCam.ScreenToWorldPoint(mousePos);
            var localTarget = transform.InverseTransformPoint(targetPos);
        }

        NativeArray<float> result = new NativeArray<float>(1000, Allocator.TempJob);

        MyParallelJob jobData = new MyParallelJob();
        jobData.localTarget = localTarget;
        jobData.result = result;

        // Schedule the job with one Execute per index in the results array and only 1 item per processing batch
        JobHandle handle = jobData.Schedule(result.Length, 100);
        // Wait for the job to complete
        handle.Complete();

    }
    [Unity.Burst.BurstCompile]
    public struct MyParallelJob : IJobParallelFor
    {
        public float aoaPitch;
        [ReadOnly]
        public NativeArray<Vector3> localTarget;
        public NativeArray<float> result;

        public void Execute(int i)
        {
            result[i] = Mathf.Clamp(-Mathf.Atan2(localTarget[i].y, localTarget[i].z), -aoaPitch, aoaPitch);
        }
    }
}
