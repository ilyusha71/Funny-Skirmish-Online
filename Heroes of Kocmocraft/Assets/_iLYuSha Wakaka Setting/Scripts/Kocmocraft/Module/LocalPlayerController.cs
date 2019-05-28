/***************************************************************************
 * Local Player Controller
 * 本地玩家控制器
 * Last Updated: 2018/11/07
 * Description:
 * 1. 新增Key Setting事件
 ***************************************************************************/
using UnityEngine;

namespace Kocmoca
{
    public class LocalPlayerController : MonoBehaviour
    {
        // Dependent Components
        private KocmocraftManager myAvionicsSystem;
        private OnboardRadar myOnboardRadar;
        private FireControlSystem myLaserFCS;
        private FireControlSystem myRocketFCS;
        private FireControlSystem myMissileFCS;
        [Header("Modular Parameter")]
        private bool useAfterBurner;
        public bool Active { get; set; } = true; // NET Framwork 4.6 其他功能限制控制用

        void Start()
        {
            // Dependent Components
            myAvionicsSystem = GetComponentInChildren<KocmocraftManager>();
            myOnboardRadar = GetComponentInChildren<OnboardRadar>();
            myLaserFCS = GetComponentsInChildren<FireControlSystem>()[0];
            myRocketFCS = GetComponentsInChildren<FireControlSystem>()[1];
            myMissileFCS = GetComponentsInChildren<FireControlSystem>()[2];
            // Camera Setting
            MouseLock.MouseLocked = false;
        }

        int reverse = 1;
        void Update()
        {// TEMP
            if (Input.GetKeyDown(KeyCode.F7))
                reverse *= -1;
            if (!myAvionicsSystem || !Active)
                return;

            Operation();
        }
        void Operation()
        {
            //myAvionicsSystem.ControlRollAndPitch(new Vector2(Controller.roll, reverse * Controller.pitch));
            //myAvionicsSystem.ControlYaw(Controller.yaw);
            myAvionicsSystem.ControlThrottle(Controller.throttle);

            if (Input.GetKeyDown(Controller.KEYBOARD_CockpitView) || Input.GetKeyDown(Controller.XBOX360_CockpitView))
                SatelliteCommander.Instance.Observer.SwitchView();
            if (Input.GetKeyDown(Controller.KEYBOARD_LockOn) || Input.GetKeyDown(Controller.XBOX360_LockOn))
                myOnboardRadar.ManualLockOn();
            if (Input.GetKey(Controller.KEYBOARD_Laser))
                myLaserFCS.Shoot();
            if (Input.GetKey(Controller.KEYBOARD_Rocket))
                myRocketFCS.Shoot();
            if (Input.GetKey(Controller.KEYBOARD_Missile))
                myMissileFCS.Shoot();
        }
    }
}