/***************************************************************************
 * Kocmocraft Mech Droid
 * 宇航機技工機器人
 * Last Updated: 2018/09/22
 * Description:
 * 1. Damage Manager -> Hull Manager -> Kocmocraft Mech Droid
 * 2. 管理能量、機甲、護盾與傷害
 ***************************************************************************/
using UnityEngine;

namespace Kocmoca
{
    public class AvionicsSystem : MonoBehaviour
    {
        [Header("Dependent Components")]
        private Rigidbody myRigidbody;
        private Engine myEngine;
        [Header("Modular Parameter")]
        public Data dataEnergy;
        public Data dataSpeed;
        private float valueSpeedCruise;
        private float valueSpeedHigh;
        private bool isCharge;

        [Header("Constant")]
        public float RotationSpeed = 50.0f;// Turn Speed
        public float SpeedPitch = 2;// rotation X
        public float SpeedRoll = 3;// rotation Z
        public float SpeedYaw = 1;// rotation Y
        public float DampingTarget = 10.0f;// rotation speed to facing to a target
        public bool AutoPilot = false;// if True this plane will follow a target automatically
        [HideInInspector]
        public bool SimpleControl = true;// set true is enabled casual controling
        [HideInInspector]
        public bool FollowTarget = false;
        [HideInInspector]
        public Vector3 PositionTarget = Vector3.zero;// current target position
        [HideInInspector]
        private Vector3 positionTarget = Vector3.zero;
        public Quaternion mainRot = Quaternion.identity;
        [HideInInspector]
        public float roll = 0;
        [HideInInspector]
        public float pitch = 0;
        [HideInInspector]
        public float yaw = 0;
        public Vector2 LimitAxisControl = new Vector2(2, 1);// limited of axis rotation magnitude
        public bool FixedX;
        public bool FixedY;
        public bool FixedZ;
        public float Mess = 30;
        public bool DirectVelocity = true;// if true this riggidbody will not receive effect by other force.
        public float DampingVelocity = 5;

        public void Initialize(int type)
        {
            // Dependent Components
            myRigidbody = GetComponent<Rigidbody>();
            myEngine = GetComponentInChildren<Engine>();
            // Modular Parameter
            dataEnergy = new Data { Max = KocmocraftData.MaxEnergy[type], Value = KocmocraftData.MaxEnergy[type] };
            dataSpeed = new Data { Max = KocmocraftData.AfterburnerSpeed[type], Value = 0 };
            valueSpeedCruise = KocmocraftData.CruiseSpeed[type];
            valueSpeedHigh = valueSpeedCruise * 1.1f;        
        }

        void Start()
        {
            mainRot = this.transform.rotation;
        }

        void FixedUpdate()
        {
            if (!myRigidbody)
                return;

            Quaternion AddRot = Quaternion.identity;
            Vector3 velocityTarget = Vector3.zero;

            // 转向控制

            if (AutoPilot)
            {
                if (myRigidbody.angularVelocity.magnitude > 3)
                    myRigidbody.Sleep();

                // if auto pilot
                if (FollowTarget)
                {
                    // rotation facing to the positionTarget
                    positionTarget = Vector3.Lerp(positionTarget, PositionTarget, Time.fixedDeltaTime * DampingTarget);
                    Vector3 relativePoint = this.transform.InverseTransformPoint(positionTarget).normalized;
                    mainRot = Quaternion.LookRotation(positionTarget - this.transform.position);
                    myRigidbody.rotation = Quaternion.Lerp(myRigidbody.rotation, mainRot, Time.fixedDeltaTime * (RotationSpeed * 0.005f) * SpeedYaw);
                    myRigidbody.rotation *= Quaternion.Euler(-relativePoint.y * SpeedPitch * 0.3f, 0, -relativePoint.x * SpeedRoll * 0.5f);

                }
                velocityTarget = (myRigidbody.rotation * Vector3.forward) * (dataSpeed.Value);
            }
            else
            {
                // axis control by input
                AddRot.eulerAngles = new Vector3(pitch, yaw, -roll);
                mainRot *= AddRot;

                if (SimpleControl)
                {
                    Quaternion saveQ = mainRot;

                    Vector3 fixedAngles = new Vector3(mainRot.eulerAngles.x, mainRot.eulerAngles.y, mainRot.eulerAngles.z);

                    if (FixedX)
                        fixedAngles.x = 1;
                    if (FixedY)
                        fixedAngles.y = 1;
                    if (FixedZ)
                        fixedAngles.z = 1;

                    saveQ.eulerAngles = fixedAngles;


                    mainRot = Quaternion.Lerp(mainRot, saveQ, Time.fixedDeltaTime * 2);
                }

                myRigidbody.rotation = Quaternion.Lerp(myRigidbody.rotation, mainRot, Time.fixedDeltaTime * RotationSpeed);
                //if (remote)
                //    Debug.Log("NxtRot2: " + Time.frameCount + " / " + myRigidbody.rotation.eulerAngles);

                velocityTarget = (myRigidbody.rotation * Vector3.forward) * (dataSpeed.Value);
            }
            // add velocity to the riggidbody

            if (DirectVelocity)
            {
                myRigidbody.velocity = velocityTarget;
            }
            else
            {
                myRigidbody.velocity = Vector3.Lerp(myRigidbody.velocity, velocityTarget, Time.fixedDeltaTime * DampingVelocity);
            }
            yaw = Mathf.Lerp(yaw, 0, Time.deltaTime);


            dataEnergy.Value = Mathf.Clamp(dataEnergy.Value + Time.deltaTime * 71, 0, dataEnergy.Max);
            if (dataEnergy.Value <= 10)
                isCharge = true;
            else if (dataEnergy.Value > 300)
                isCharge = false;
        }

        // Input function. ( roll and pitch)
        public void AxisControl(Vector2 axis)
        {
            if (SimpleControl)
            {
                LimitAxisControl.y = LimitAxisControl.x;
            }
            // Debug.Log(axis.x);
            roll = Mathf.Lerp(roll, Mathf.Clamp(axis.x, -LimitAxisControl.x, LimitAxisControl.x) * SpeedRoll, Time.deltaTime);
            pitch = Mathf.Lerp(pitch, Mathf.Clamp(axis.y, -LimitAxisControl.y, LimitAxisControl.y) * SpeedPitch, Time.deltaTime);
        }
        // Input function ( yaw) 
        public void TurnControl(float turn)
        {
            yaw += turn * Time.deltaTime * SpeedYaw;
        }
        public void SpeedControl(float throttle, bool useAfterBurner)
        {
            if (isCharge)
                useAfterBurner = false;
            if (throttle > 0)
            {
                if (useAfterBurner)
                {
                    dataSpeed.Value = Mathf.Lerp(dataSpeed.Value, dataSpeed.Max, Time.deltaTime * (0.73f * throttle));
                    dataEnergy.Value = Mathf.Clamp(dataEnergy.Value - Time.deltaTime * 163, 0, dataEnergy.Max);
                }
                else
                    dataSpeed.Value = Mathf.Lerp(dataSpeed.Value, valueSpeedHigh, Time.deltaTime * (0.73f * throttle));
            }
            else if (throttle < 0)
                dataSpeed.Value = Mathf.Lerp(dataSpeed.Value, 0, Time.deltaTime * (0.73f * -throttle));
            else
                dataSpeed.Value = Mathf.Lerp(dataSpeed.Value, valueSpeedCruise, Time.deltaTime);

            myEngine.Power(dataSpeed.Percent);
        }
    }

}