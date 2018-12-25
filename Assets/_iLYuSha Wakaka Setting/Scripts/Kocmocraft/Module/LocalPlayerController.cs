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
        private AvionicsSystem myAvionicsSystem;
        private OnboardRadar myOnboardRadar;
        private FireControlSystem myLaserFCS;
        private FireControlSystem myRocketFCS;
        private FireControlSystem myMissileFCS;
        [Header("Modular Parameter")]
        private bool useAfterBurner;
        public bool Active { get; set; } = true; // NET Framwork 4.6 其他功能限制控制用
        [Header("Key Setting")]
        public KeyCode KEY_ActiveAfterburner;
        public KeyCode KEY_SwtichView;
        public KeyCode KEY_LockOn;
        public KeyCode KEY_LaserShoot;
        public KeyCode KEY_RocketLaunch;
        public KeyCode KEY_MissileLaunch;

        private void OnEnable()
        {
            Controller.OnHotKeyChanged += OnHotKeyChanged;
        }
        private void OnDisable()
        {
            Controller.OnHotKeyChanged -= OnHotKeyChanged;
        }
        void OnHotKeyChanged()
        {
            Debug.LogWarning("Hot Key Changed");
            KEY_ActiveAfterburner = Controller.KEY_Afterburner;
            KEY_SwtichView = Controller.KEY_CockpitView;
            KEY_LockOn = Controller.KEY_LockOn;
            KEY_LaserShoot = Controller.KEY_Laser;
            KEY_RocketLaunch = Controller.KEY_Rocket;
            KEY_MissileLaunch = Controller.KEY_Missile;
        }
        void Start()
        {
            // Dependent Components
            myAvionicsSystem = GetComponent<AvionicsSystem>();
            myOnboardRadar = GetComponent<OnboardRadar>();
            myLaserFCS = GetComponentsInChildren<FireControlSystem>()[0];
            myRocketFCS = GetComponentsInChildren<FireControlSystem>()[1];
            myMissileFCS = GetComponentsInChildren<FireControlSystem>()[2];
            // Camera Setting
            MouseLock.MouseLocked = true;
            OnHotKeyChanged();
        }
        void Update()
        {
            if (!myAvionicsSystem || !Active)
                return;
            Operation();
        }
        void Operation()
        {
            myAvionicsSystem.AxisControl(new Vector2(Controller.roll, Controller.pitch));
            myAvionicsSystem.TurnControl(Controller.yaw);
            myAvionicsSystem.SpeedControl(Controller.throttle, useAfterBurner);

            if (Input.GetKey(KEY_ActiveAfterburner)) useAfterBurner = true;
            else if (Input.GetKeyUp(KEY_ActiveAfterburner)) useAfterBurner = false;
            if (Input.GetKeyDown(KEY_SwtichView)) SatelliteCommander.Instance.Observer.SwitchView();
            if (Input.GetKeyDown(KEY_LockOn)) myOnboardRadar.ManualLockOn();
            if (Input.GetKey(KEY_LaserShoot)) myLaserFCS.Shoot();
            if (Input.GetKey(KEY_RocketLaunch)) myRocketFCS.Shoot();
            if (Input.GetKey(KEY_MissileLaunch)) myMissileFCS.Shoot();
        }
    }
}