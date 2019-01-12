/***************************************************************************
 * Fire Control System
 * 射控系統
 * Last Updated: 2018/10/12
 * Description:
 * 1. Weapon Launcher -> Weapon System -> Fire Control System
 * 2. 進行發射器的初始化與彈藥管理
 ***************************************************************************/
using Photon.Pun;
using UnityEngine;

namespace Kocmoca
{
    public class FireControlSystem : MonoBehaviour
    {
        // Dependent Components
        private Transform myTransform;
        private PhotonView myPhotonView;
        private OnboardRadar myOnboardRadar;
        private AudioSource myAudioSource;
        // Kocmonaut Data
        private int kocmonautNumber;
        [SerializeField]
        private bool isLocal; // Local Player or Local Bot
        // FCS Data Loading
        private FireControlSystemType typeFCS = FireControlSystemType.Unknown;
        private float timeReload; // 飛彈重載耗時
        public int maxAmmoCapacity { get; set; } // 最大載彈量
        private float rateFire; // 開火速率
        private AudioClip shootingSound;
        // FSC Realtime Info
        public int countAmmo { get; set; }
        private int countLauncher;
        private Vector3[] launcher; // 砲口
        private int turnFire; // 砲管輪射
        private int currentLauncher;
        private float nextTimeShoot;
        // Ammo Loading State
        private bool isReloading;
        public float reloadingProcess { get; set; } // 剩餘百分比
        private float nextTimeAmmoLoaded;
        // Target Info
        [Header("Target Info")]
        public Transform target;
        public int targetNumber;
        private Vector3 expectedTargetPosition;
        private Vector3 expectedTargetDirection;

        public void Initialize(int type, int number,bool isLocal)
        {
            // Dependent Components
            myTransform = transform;
            myPhotonView = myTransform.root.GetComponent<PhotonView>();
            myOnboardRadar = myTransform.root.GetComponent<OnboardRadar>();
            myAudioSource = GetComponent<AudioSource>();
            // Kocmonaut Data
            kocmonautNumber = number;
            this.isLocal = isLocal;
            // FCS Data Loading
            typeFCS = WeaponData.GetFCS(myTransform.name);
            if (typeFCS == FireControlSystemType.Unknown) Debug.LogError("No FCS");
            LoadFireControlSystemData();
            InitializeLauncher();
        }
        void LoadFireControlSystemData()
        {
            switch (typeFCS)
            {
                case FireControlSystemType.Laser:
                    maxAmmoCapacity = 999;
                    rateFire = KocmoLaserCannon.rateFire;
                    shootingSound = ResourceManager.instance.soundLaser;
                    break;
                case FireControlSystemType.Rocket:
                    timeReload = KocmoRocketLauncher.timeReload;
                    maxAmmoCapacity = KocmoRocketLauncher.maxAmmoCapacity;
                    rateFire = KocmoRocketLauncher.rateFire;
                    countAmmo = 2;
                    shootingSound = ResourceManager.instance.soundRocket;
                    break;
                case FireControlSystemType.Missile:
                    timeReload = KocmoMissileLauncher.timeReload;
                    maxAmmoCapacity = KocmoMissileLauncher.maxAmmoCapacity;
                    rateFire = KocmoMissileLauncher.rateFire;
                    countAmmo = 1;
                    shootingSound = ResourceManager.instance.soundMissile;
                    break;
            }
        }
        public void InitializeLauncher()
        {
            Transform[] obj = GetComponentsInChildren<Transform>();
            countLauncher = (obj.Length - 1) * 2; // GetComponentsInChildren取得陣列數量包含父物件
            launcher = new Vector3[countLauncher];
            for (int i = 0; i < countLauncher; i++)
            {
                launcher[i] = obj[Mathf.CeilToInt((i + 1) * 0.5f)].localPosition;
                if (i % 2 == 1)
                    launcher[i].x = -launcher[i].x;
            }
            turnFire = Mathf.RoundToInt(countLauncher * 0.5f);
        }

        private void Update()
        {
            if (!isLocal)
                return;

            if (isReloading)
            {
                reloadingProcess = ((1 / timeReload) * (nextTimeAmmoLoaded - Time.time));
                if (Time.time >= nextTimeAmmoLoaded)
                {
                    isReloading = false;
                    countAmmo += 1;
                }
            }
            else
            {
                if (countAmmo < maxAmmoCapacity)
                {
                    isReloading = true;
                    nextTimeAmmoLoaded = Time.time + timeReload;
                }
            }
        }

        public void Shoot()
        {
            countAmmo = (maxAmmoCapacity == 999) ? maxAmmoCapacity : countAmmo;
            if (countAmmo > 0 && Time.time > nextTimeShoot)
            {
                nextTimeShoot = Time.time + rateFire;
                countAmmo--;
                LauncherControl();
            }
        }
        void LauncherControl()
        {
            currentLauncher++;
            currentLauncher = (int)Mathf.Repeat(currentLauncher, countLauncher);

            target = typeFCS == FireControlSystemType.Laser? myOnboardRadar.targetAutoAim: myOnboardRadar.targetRadarLockOn;
            targetNumber = target ? target.GetComponent<KocmocraftManager>().Number : 0;

            for (int t = 0; t < turnFire; t++)
            {
                switch (typeFCS)
                {
                    case FireControlSystemType.Laser:
                        myPhotonView.RPC("LaserShoot", RpcTarget.AllViaServer, currentLauncher, kocmonautNumber, targetNumber, Random.Range(-KocmoLaserCannon.projectileSpread, KocmoLaserCannon.projectileSpread));
                        break;
                    case FireControlSystemType.Rocket:
                        myPhotonView.RPC("RockeLaunch", RpcTarget.AllViaServer, currentLauncher, kocmonautNumber, targetNumber);
                        break;
                    case FireControlSystemType.Missile:
                        myPhotonView.RPC("MissileLaunch", RpcTarget.AllViaServer, currentLauncher, kocmonautNumber, targetNumber);
                        break;
                }

                if (t == 0)
                    myAudioSource.PlayOneShot(shootingSound, 0.73f);

                currentLauncher += 2;
                currentLauncher = (int)Mathf.Repeat(currentLauncher, countLauncher);
            }
        }
        public Vector3 GetMuzzlePosition(int muzzle)
        {
            return myTransform.TransformPoint(launcher[muzzle]);
        }
    }
}