using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class TestAim : MonoBehaviour
{

    public Transform myTransform;
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

        myTransform = transform;
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

        for (int i = 0; i < 300; i++)
        {
            WakakaCalculateRollAndPitchAngles();
            WakakaCalculateRollAndPitchAngles2();
            WakakaCalculateRollAndPitchAngles3();

            //CalculateRollAndPitchAngles();
            //CalculatePitchAngles();
            //CalculateRollAngles();
            //CalculatePitchAngles2();
            //CalculateRollAngles2();
        }
        //if (Input.GetKeyDown(KeyCode.P))
        //{

        //    CalculateRollAndPitchAngles();
        //    CalculatePitchAngles();
        //    CalculateRollAngles();
        //}
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

    [Header("Pitch")]
    public float PitchAngle;
    public float pitchAngle;
    public float pitchAngle2;
    [Header("Roll")]
    public float RollAngle;
    public float rollAngle;
    public float rollAngle2;

    [Header("Other")]

    public float RollAngle2;
    public float RollAngleTest1;
    public float RollAngleTest2;

    public float RollAngleTest3;
    public float RollAngleTest4;


    public Vector3 flat;
    public Vector3 flat2;
    public Vector3 TForward;
    public Vector3 TUp;
    public Vector3 TRight;
    public int count = 1;

    public void WakakaCalculateRollAndPitchAngles()
    {
        var fwd = myTransform.forward;
        fwd.y = 0;
        fwd.Normalize(); // 先进行Normalize再Cross会比较快
        var right = Vector3.Cross(Vector3.up, fwd);
        // 计算 InverseTransformDirection + 反正切速度更快
        var localFlatRight = myTransform.InverseTransformDirection(right);
        RollAngle = -Mathf.Atan2(localFlatRight.y, localFlatRight.x) * Mathf.Rad2Deg;
    }
    public void WakakaCalculateRollAndPitchAngles2()
    {
        var fwd = myTransform.forward;
        fwd.y = 0;
        fwd.Normalize(); // 先进行Normalize再Cross会比较快
        var right = Vector3.Cross(Vector3.up, fwd);
        // 直接计算向量夹角（Angle效能极差，比使用反余弦还要多14%的耗时）
        rollAngle = Vector3.Angle(right, myTransform.right) * Mathf.Sign(myTransform.right.y);
    }
    public void WakakaCalculateRollAndPitchAngles3()
    {
        var fwd = myTransform.forward;
        fwd.y = 0;
        fwd.Normalize(); // 先进行Normalize再Cross会比较快
        var right = Vector3.Cross(Vector3.up, fwd);
        // 计算两向量反余弦得出夹角
        rollAngle2 = Mathf.Acos(Vector3.Dot(right, myTransform.right)) * Mathf.Sign(myTransform.right.y) * Mathf.Rad2Deg;
    }

    public void CalculateRollAndPitchAngles()
    {
        // Unity Standard Asset
        // Calculate roll & pitch angles
        // Calculate the flat forward direction (with no y component).
        var flatForward = myTransform.forward;
        flatForward.y = 0;
        // If the flat forward vector is non-zero (which would only happen if the plane was pointing exactly straight upwards)
        if (flatForward.sqrMagnitude > 0)
        {
            flatForward.Normalize();
            // calculate current pitch angle
            var localFlatForward = myTransform.InverseTransformDirection(flatForward);
            PitchAngle = Mathf.Atan2(localFlatForward.y, localFlatForward.z) * Mathf.Rad2Deg; // 这个值不正确
                                                                                              // calculate current roll angle
            var flatRight = Vector3.Cross(Vector3.up, flatForward);
            var localFlatRight = myTransform.InverseTransformDirection(flatRight);
            RollAngle = Mathf.Atan2(localFlatRight.y, localFlatRight.x) * Mathf.Rad2Deg;
        }
    }
    public void CalculateRollAngles()
    {
        var fwd = myTransform.forward;
        fwd.y = 0;
        //fwd *= Mathf.Sign(myTransform.up.y); // 投影面法向量修正，roll的范围将会变成-90~90
        var right = Vector3.Cross(Vector3.up, fwd).normalized;
        // 直接计算向量夹角（比使用反余弦还要多14%的耗时）
        rollAngle = Vector3.Angle(right, myTransform.right) * Mathf.Sign(myTransform.right.y);
        // 计算两向量反余弦得出夹角
        rollAngle2 = Mathf.Acos(Vector3.Dot(right, myTransform.right)) * Mathf.Sign(myTransform.right.y) * Mathf.Rad2Deg;
    }
    public void CalculateRollAngles2()
    {
        var fwd = myTransform.forward;
        fwd.y = 0;
        //fwd *= Mathf.Sign(myTransform.up.y); // 投影面法向量修正，roll的范围将会变成-90~90
        var right = Vector3.Cross(Vector3.up, fwd).normalized;
        // 直接计算向量夹角
        //rollAngle = Vector3.Angle(right, myTransform.right) * Mathf.Sign(myTransform.right.y);
        // 计算两向量反余弦得出夹角
        rollAngle2 = Mathf.Acos(Vector3.Dot(right, myTransform.right)) * Mathf.Sign(myTransform.right.y) * Mathf.Rad2Deg;
    }
    public void CalculatePitchAngles()
    {
        var right = myTransform.right;
        right.y = 0;
        right *= Mathf.Sign(myTransform.up.y);  // 投影面法向量修正，pitch的范围将会变成-90~90
        var fwd = Vector3.Cross(right, Vector3.up).normalized;
        // 直接计算向量夹角
        pitchAngle = Vector3.Angle(fwd, myTransform.forward) * -Mathf.Sign(myTransform.forward.y);
        // 计算两向量反余弦得出夹角
        //pitchAngle2 = Mathf.Acos(Vector3.Dot(fwd, myTransform.forward)) * -Mathf.Sign(myTransform.forward.y) * Mathf.Rad2Deg;
    }
    public void CalculatePitchAngles2()
    {
        var right = myTransform.right;
        right.y = 0;
        right *= Mathf.Sign(myTransform.up.y);  // 投影面法向量修正，pitch的范围将会变成-90~90
        var fwd = Vector3.Cross(right, Vector3.up).normalized;
        // 直接计算向量夹角
        //pitchAngle = Vector3.Angle(fwd, myTransform.forward) * -Mathf.Sign(myTransform.forward.y);
        // 计算两向量反余弦得出夹角
        pitchAngle2 = Mathf.Acos(Vector3.Dot(fwd, myTransform.forward)) * -Mathf.Sign(myTransform.forward.y) * Mathf.Rad2Deg;
    }

    private void FixedUpdate()
    {
        //mousePos.z = followCam.farClipPlane;
        //var targetPos = followCam.ScreenToWorldPoint(mousePos);
        //var localTarget = myTransform.InverseTransformPoint(targetPos);
        //var changeRoll = Mathf.Clamp(-Mathf.Atan2(localTarget.x, localTarget.z), -aoaYaw, aoaYaw);
        //var changePitch = Mathf.Clamp(-Mathf.Atan2(localTarget.y, localTarget.z), -aoaPitch, aoaPitch);
        //var changeYaw = changeRoll;

        //CalculateRollAndPitchAngles();
        //CalculatePitchAngles();
        //CalculateRollAngles();


        //TForward = transform.forward;
        //TUp = transform.up;
        //TRight = transform.right;

        //// Calculate the flat forward direction (with no y component).
        //var flatForward = transform.forward;
        //flatForward.y = 0;
        //// If the flat forward vector is non-zero (which would only happen if the plane was pointing exactly straight upwards)
        //if (flatForward.sqrMagnitude > 0)
        //{
        //    flatForward.Normalize();
        //    // calculate current pitch angle
        //    var localFlatForward = transform.InverseTransformDirection(flatForward);
        //    PitchAngle = Mathf.Atan2(localFlatForward.y, localFlatForward.z) * Mathf.Rad2Deg;
        //    // calculate current roll angle
        //    var flatRight = Vector3.Cross(Vector3.up, flatForward);

        //    var localFlatRight = transform.InverseTransformDirection(flatRight);
        //    flat = flatRight;
        //    RollAngle = Mathf.Atan2(localFlatRight.y, localFlatRight.x) * Mathf.Rad2Deg;

        //    RollAngle2 = Vector3.Angle(flatRight, transform.right) * Mathf.Sign(transform.right.y);


        //    //Vector3.Angle(Vector3.right, flatRight); // 计算Yaw 无正负 
        //}

        //var fwd = transform.forward;
        //fwd.y = 0;
        ////fwd *= Mathf.Sign(transform.up.y);
        //var right = Vector3.Cross(Vector3.up, fwd).normalized;
        //flat2 = right;
        //float roll = Vector3.Angle(right, transform.right) * Mathf.Sign(transform.right.y);
        //RollAngleTest1 = roll;
        //var localFlatRight2 = transform.InverseTransformDirection(right);
        //RollAngleTest2 = Mathf.Atan2(localFlatRight2.y, localFlatRight2.x) * Mathf.Rad2Deg;



        //if (Mathf.Abs(changeRoll) < 0.07f)
        {
            //Debug.Log(Vector3.Dot(transform.up, Vector3.right));
            //changeRoll = ;
            //var flatForward = transform.forward;
            //flatForward.y = 0;
            //// If the flat forward vector is non-zero (which would only happen if the plane was pointing exactly straight upwards)
            //if (flatForward.sqrMagnitude > 0)
            //{
            //    flatForward.Normalize();
            //    var flatRight = Vector3.Cross(Vector3.up, flatForward);
            //    var localFlatRight = transform.InverseTransformDirection(flatRight);
            //    changeRoll = Mathf.Atan2(localFlatRight.y, localFlatRight.x) * 0.5f;
            //}
        }
        //myRigidbody.rotation *= Quaternion.Euler(changePitch * pitch, -changeYaw * yaw, changeRoll * roll);
        //myRigidbody.velocity = (myRigidbody.rotation * Vector3.forward) * 90;

    }

    public int batch = 1;
    public void Ca1()
    {
        float startTime = Time.realtimeSinceStartup;
        for (int i = 0; i < count; i++)
        {
            var localTarget = Vector3.one;
            var changeRoll = Mathf.Clamp(-Mathf.Atan2(localTarget.x, localTarget.z), -aoaYaw, aoaYaw);
            var changePitch = Mathf.Clamp(-Mathf.Atan2(localTarget.y, localTarget.z), -aoaPitch, aoaPitch);
            var changePitch2 = Mathf.Clamp(-Mathf.Atan2(localTarget.y, localTarget.z), -aoaPitch, aoaPitch);
            var changePitch3 = Mathf.Clamp(-Mathf.Atan2(localTarget.y, localTarget.z), -aoaPitch, aoaPitch);
            var changePitch4 = Mathf.Clamp(-Mathf.Atan2(localTarget.y, localTarget.z), -aoaPitch, aoaPitch);

        }
        float endTime = Time.realtimeSinceStartup;
        Debug.Log("TotalCount:" + count + ",Direct Use Time:" + (endTime - startTime));

    }

    public void Ca2()
    {
        float startTime = Time.realtimeSinceStartup;
        NativeArray<Vector3> localTarget = new NativeArray<Vector3>(count, Allocator.TempJob);
        NativeArray<float> changeRoll = new NativeArray<float>(count, Allocator.TempJob);
        NativeArray<float> changePitch = new NativeArray<float>(count, Allocator.TempJob);
        NativeArray<float> changePitch2 = new NativeArray<float>(count, Allocator.TempJob);
        NativeArray<float> changePitch3 = new NativeArray<float>(count, Allocator.TempJob);
        NativeArray<float> changePitch4 = new NativeArray<float>(count, Allocator.TempJob);

        float startTime1 = Time.realtimeSinceStartup;
        for (int i = 0; i < count; i++)
        {
            //var targetPos = Vector3.one;
            //main[i] = transform;
            localTarget[i] = Vector3.one;
        }


        MyParallelJob jobData = new MyParallelJob();
        jobData.aoaYaw = aoaYaw;
        jobData.aoaPitch = aoaPitch;
        jobData.localTarget = localTarget;
        jobData.changeRoll = changeRoll;
        jobData.changePitch = changePitch;
        jobData.changePitch2 = changePitch2;
        jobData.changePitch3 = changePitch3;
        jobData.changePitch4 = changePitch4;

        // Schedule the job with one Execute per index in the results array and only 1 item per processing batch
        JobHandle handle = jobData.Schedule(count, batch);
        // Wait for the job to complete
        handle.Complete();
        float endTime = Time.realtimeSinceStartup;
        Debug.Log("TotalCount:" + count + ",Parallel Use Time:" + (endTime - startTime) + ",Init Use Time:" + (startTime1 - startTime));

        localTarget.Dispose();
        changeRoll.Dispose();
        changePitch.Dispose();
        changePitch2.Dispose();
        changePitch3.Dispose();
        changePitch4.Dispose();
    }
    [Unity.Burst.BurstCompile]
    public struct MyParallelJob : IJobParallelFor
    {
        public float aoaYaw;
        public float aoaPitch;
        [ReadOnly]
        public NativeArray<Vector3> localTarget;
        public NativeArray<float> changeRoll;
        public NativeArray<float> changePitch;
        public NativeArray<float> changePitch2;
        public NativeArray<float> changePitch3;
        public NativeArray<float> changePitch4;


        public void Execute(int i)
        {
            changeRoll[i] = Mathf.Clamp(-Mathf.Atan2(localTarget[i].x, localTarget[i].z), -aoaYaw, aoaYaw);
            changePitch[i] = Mathf.Clamp(-Mathf.Atan2(localTarget[i].y, localTarget[i].z), -aoaPitch, aoaPitch);
            changePitch2[i] = Mathf.Clamp(-Mathf.Atan2(localTarget[i].y, localTarget[i].z), -aoaPitch, aoaPitch);
            changePitch3[i] = Mathf.Clamp(-Mathf.Atan2(localTarget[i].y, localTarget[i].z), -aoaPitch, aoaPitch);
            changePitch4[i] = Mathf.Clamp(-Mathf.Atan2(localTarget[i].y, localTarget[i].z), -aoaPitch, aoaPitch);

        }
    }
}
