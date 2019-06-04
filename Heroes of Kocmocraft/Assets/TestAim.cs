using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;
using UnityEngine.Jobs;

public class TestAim : MonoBehaviour
{
    Cinemachine.CinemachineBrain brain;
    Cinemachine.CinemachineComposer composer;
  public  Cinemachine.CinemachineVirtualCamera cam;

    public Transform trackpp;
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


    public float decayAngle;
    public float inverseAngle;
    public float terminalAngle;
    public float terminalAngle2;

    public float kappa;

    private void Start()
    {
        composer = cam.GetCinemachineComponent<Cinemachine.CinemachineComposer>();
        brain = Cinemachine.CinemachineCore.Instance.FindPotentialTargetBrain(cam);

        myRigidbody = GetComponent<Rigidbody>();
        followCam = Camera.main;
        range = Screen.height * 0.47f;
        pivot = new Vector3(Screen.width * 0.5f, Screen.height * 0.5f);
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Confined;

        aoaYaw = 16 * Mathf.Deg2Rad;
        aoaPitch = 9 * Mathf.Deg2Rad;

        decayAngle = 9 * Mathf.Deg2Rad;
        inverseAngle = decayAngle * 0.75f;
        terminalAngle = decayAngle *0.5f;
        terminalAngle2 = terminalAngle * 3;
        //kappa = Mathf.PI / (2 * (autoLevelRange - inverseAngle));
        //ccc = vvv.GetCinemachineComponent<Cinemachine.CinemachineComposer>();


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
        CalculatePitch();
        CalculatePitch2();
        CalculatePitch3();
        //for (int i = 0; i < 300; i++)
        //{
        //    WakakaCalculateRollAndPitchAngles();
        //    WakakaCalculateRollAndPitchAngles2();
        //    WakakaCalculateRollAndPitchAngles3();

        //    //CalculateRollAndPitchAngles();
        //    //CalculatePitchAngles();
        //    //CalculateRollAngles();
        //    //CalculatePitchAngles2();
        //    //CalculateRollAngles2();
        //}
        //if (Input.GetKeyDown(KeyCode.P))
        //{

        //    CalculateRollAndPitchAngles();
        //    CalculatePitchAngles();
        //    CalculateRollAngles();
        //}
    }

    public float changeYaw;
    public float changeRoll;
    public float sin;
    public float signRoll;
    public float signChange;

    public float angle;
    public float rad;
    public float log;

    public Cinemachine.CinemachineVirtualCamera vvv;
    public Cinemachine.CinemachineComposer ccc;
    private void FixedUpdate()
    {
        Vector3 tp = brain.OutputCamera.WorldToScreenPoint(composer.TrackedPoint);
        trackpp.position = tp;

        mousePos.z = followCam.farClipPlane;
        var targetPos = followCam.ScreenToWorldPoint(mousePos);
        var localTarget = myTransform.InverseTransformPoint(targetPos);

        var changePitch = Mathf.Clamp(-Mathf.Atan2(localTarget.y, localTarget.z), -aoaPitch, aoaPitch);
         changeYaw = Mathf.Clamp(-Mathf.Atan2(localTarget.x, localTarget.z), -aoaYaw, aoaYaw);


        var fwd = myTransform.forward;
        fwd.y = 0;
        //fwd *= Mathf.Sign(myTransform.up.y); // 投影面法向量修正，roll的范围将会变成-90~90
        fwd.Normalize(); // 先进行Normalize再Cross会比较快
        var right = Vector3.Cross(Vector3.up, fwd);
        // 计算 InverseTransformDirection + 反正切速度更快
        var localFlatRight = myTransform.InverseTransformDirection(right);
        var rollAngle = Mathf.Atan2(localFlatRight.y, localFlatRight.x);
        //var yawAbs = Mathf.Abs(changeYaw);
        signRoll = Mathf.Sign(rollAngle);
        signChange = Mathf.Sign(changeYaw);

        // changeRoll为机翼翻滚值，会因changePitch与changeYaw修正           
        // 机翼自动归正，当changePitch与changeYaw逐渐变小皆进入autoLevelRange的范围
        if (Mathf.Abs(changeYaw) <= decayAngle && Mathf.Abs(changePitch) <= decayAngle)
        {
            // 分为三个阶段
            // 周期 1.00 T = decayAngle，0.75 T = inverseAngle，0.50 T = terminalAngle
            if (Mathf.Abs(changeYaw) < inverseAngle)
            {
                //if (Mathf.Abs(changeYaw) < terminalAngle)
                //    changeRoll = 0.0123456789f;// Mathf.Abs(rollAngle / Mathf.PI) * -Mathf.Sign(changeYaw);
                //// 第二阶段：反转点inverseAngle为T=3/4
                //else
                    changeRoll = decayAngle * Mathf.Cos(changeYaw * Mathf.PI / (terminalAngle*3) + Mathf.PI) * -Mathf.Sign(rollAngle); // 正确    
                //changeRoll = Mathf.Clamp(changeRoll, -inverseAngle, inverseAngle);

            }
            // 第一阶段：衰退阶段为Cosine函数75%~100%，周期与振幅均为autoLevelRange，decayPeriod为半周期
            else
                changeRoll = decayAngle * Mathf.Cos(changeYaw * Mathf.PI / terminalAngle) * Mathf.Sign(changeYaw); // 正确

            // 第二阶段：反转平缓为一余弦函数，随着changeYaw持续减小，changeRoll会从最大反转角逐渐趋于0
            // 振幅为inverseAngle
            //else
            //    changeRoll = (rollAngle * Mathf.Cos(changeYaw * Mathf.PI / inverseAngle)) * Mathf.Sign(changeYaw);

            ////changeRoll = atYaw* Mathf.Sign(rollAngle);

            //    changeRoll = (changeRoll * 2 - atYaw * Mathf.Sign(changeRoll)) * Mathf.Sign(rollAngle);
            //changeRoll = (changeRoll * 2 - atYaw * Mathf.Sign(changeRoll));
        }
        else
            changeRoll = changeYaw;
        myRigidbody.rotation *= Quaternion.Euler(changePitch * pitch, -changeYaw * yaw, changeRoll * roll);
        myRigidbody.velocity = (myRigidbody.rotation * Vector3.forward) * 90;



        //Vector2 guessPos = myTransform.TransformPoint( new Vector3(0, 10, 0));
        //Vector2 go = followCam.WorldToScreenPoint(guessPos);
        //Debug.LogWarning(go.ToString("F4"));

        //Vector2 go2 = followCam.WorldToScreenPoint(vvv.m_LookAt.position + ccc.m_TrackedObjectOffset);
        //Debug.Log(go2.ToString("F4"));
        //Debug.LogWarning(myTransform.position.ToString("F4"));
        //Debug.Log(ccc.m_TrackedObjectOffset.ToString("F4"));
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
    //public float rollAngle;
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

    public void CalculatePitch()
    {
        var fwd = myTransform.forward;
        fwd.y = 0;
        fwd.Normalize(); // 先进行Normalize再Cross会比较快
        // 计算 InverseTransformDirection + 反正切速度更快
        var localFlatForward = myTransform.InverseTransformDirection(fwd);
        PitchAngle = Mathf.Atan2(localFlatForward.y, localFlatForward.z) * Mathf.Rad2Deg; // 这个值不正确
    }
    public void CalculatePitch2()
    {
        var right = myTransform.right;
        right.y = 0;
        right *= Mathf.Sign(myTransform.up.y);  // 投影面法向量修正，pitch的范围将会变成-90~90
        var fwd = Vector3.Cross(right, Vector3.up).normalized;
        // 直接计算向量夹角
        pitchAngle = Vector3.Angle(fwd, myTransform.forward) * -Mathf.Sign(myTransform.forward.y);
    }
    public void CalculatePitch3()
    {
        var right = myTransform.right;
        right.y = 0;
        right *= Mathf.Sign(myTransform.up.y);  // 投影面法向量修正，pitch的范围将会变成-90~90
        var fwd = Vector3.Cross(right, Vector3.up).normalized;
        // 计算两向量反余弦得出夹角
        pitchAngle2 = Mathf.Acos(Vector3.Dot(fwd, myTransform.forward)) * -Mathf.Sign(myTransform.forward.y) * Mathf.Rad2Deg;
    }

    /***************************************************************************
     * Roll的角度定义范围
     * 0. Unity定义为 -180到180，右翻滚0到-180，左翻滚0到+180
     * 1. 正向水平面，以Vector.up为正向的面，值域为-180到+180
     *      右翻滚（顺时针）为0到-180
     *      左翻滚（逆时针）为0到+180
     * 2. 轴向水平面，以Vector.up与Vector.down作为正反两水平面，值域为-90到+90
     *      右翻滚（顺时针）为0到-90
     *      左翻滚（逆时针）为0到+90
     *      因此正向水平面的-90到-180（右翻滚）会等于轴向水平面的+90到0（相当于反向水平面的左翻滚）
     * 
    ***************************************************************************/
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
        rollAngle2 = Vector3.Angle(right, myTransform.right) * Mathf.Sign(myTransform.right.y);
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
        rollAngle2 = Vector3.Angle(right, myTransform.right) * Mathf.Sign(myTransform.right.y);
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


    public Vector3 now;
    public Vector3 last;

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
