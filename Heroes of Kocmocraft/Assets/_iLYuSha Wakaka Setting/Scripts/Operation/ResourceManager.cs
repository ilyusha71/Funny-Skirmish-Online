using UnityEngine;

namespace Kocmoca
{
    public class ResourceManager : MonoBehaviour
    {
        public static ResourceManager instance;
        [Header("Ammo Prefab")]
        public GameObject BigKocmoLaser;
        public GameObject prefabKocmoLaser;
        public GameObject prefabKocmoRocket;
        public GameObject prefabKocmoMissile;
        [Header("FX Prefab")]
        public GameObject prefabHitSpark;
        public GameObject prefabHitDown;
        public GameObject prefabHitGround;
        public GameObject prefabHitWater;

        [Header("Take Off Sound Effects")]
        public AudioClip soundTakeOff;
        [Header("Hit Effects")]
        public AudioClip soundHitSelf;
        [Header("Onboard Radar Sound Effects")]
        public AudioClip soundLockOn;
        [Header("Warning Alarm Sound Effects")]
        public AudioClip soundRadarLocked;
        public AudioClip soundMissileLocked;
        [Header("FCS Sound Effects")]
        public AudioClip soundLaser;
        public AudioClip soundRocket;
        public AudioClip soundMissile;

        // OPD
        public ObjectPoolData[] listAmmoOPD = new ObjectPoolData[3];
        public ObjectPoolData listAmmoBS = new ObjectPoolData();
        public ObjectPoolData hitSpark = new ObjectPoolData();
        public static ObjectPoolData hitDown = new ObjectPoolData();
        public static ObjectPoolData hitGround = new ObjectPoolData();
        public static ObjectPoolData hitWater = new ObjectPoolData();

        private void Awake()
        {
            instance = this;
        }

        private void Start()
        {
            LoadAmmo();
        }

        private void LoadAmmo()
        {
            listAmmoOPD[0] = ObjectPoolManager.Instance.CreateObjectPool(prefabKocmoLaser, KocmoLaserCannon.countPerBatch, KocmoLaserCannon.maxPoorInventory);
            listAmmoOPD[1] = ObjectPoolManager.Instance.CreateObjectPool(prefabKocmoRocket, KocmoRocketLauncher.countPerBatch, KocmoRocketLauncher.maxPoorInventory);
            listAmmoOPD[2] = ObjectPoolManager.Instance.CreateObjectPool(prefabKocmoMissile, KocmoMissileLauncher.countPerBatch, KocmoMissileLauncher.maxPoorInventory);
            listAmmoBS = ObjectPoolManager.Instance.CreateObjectPool(BigKocmoLaser, 5, 100);
            hitSpark = ObjectPoolManager.Instance.CreateObjectPool(prefabHitSpark, 5, 500);
            hitDown  = ObjectPoolManager.Instance.CreateObjectPool(prefabHitDown, 1, 50);
            hitGround = ObjectPoolManager.Instance.CreateObjectPool(prefabHitGround, 1, 100);
            hitWater = ObjectPoolManager.Instance.CreateObjectPool(prefabHitWater, 1, 50);

        }

        public void BatchClone()
        {
            ObjectPoolManager.Instance.Clone(listAmmoOPD[0]);
            ObjectPoolManager.Instance.Clone(listAmmoOPD[1]);
            ObjectPoolManager.Instance.Clone(listAmmoOPD[2]);
            ObjectPoolManager.Instance.Clone(listAmmoBS);
            ObjectPoolManager.Instance.Clone(hitSpark);
            ObjectPoolManager.Instance.Clone(hitDown);
            ObjectPoolManager.Instance.Clone(hitGround);
            ObjectPoolManager.Instance.Clone(hitWater);

        }
    }
}