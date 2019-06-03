/*****************************************************************************
 * Fire Control System
 * 射控系統
 * Last Updated: 2019/04/20
 * 
 * v19.0603
 * 1. FireControlSystem細分為Turret、Rocket與Missile三個子類
 * 2. 直接從對應的ScriptableObject取值
 * 
 * v19.0420
 * 1. Weapon Launcher -> Weapon System -> Fire Control System
 * 2. 進行發射器的初始化與彈藥管理
 * 3. 使用ModuleData进行快速初始化
*****************************************************************************/

/***************************************************************************
 * Missile Fire Control System (MFCS)
 * 導彈火控系統
 * Last Updated: 2019/06/03
 * 
 * v19.0603
 * 1. 繼承FireControlSystem的獨立MissileFireControlSystem
 * 2. 直接從Missile取值
 * 3. 射擊音效使用物件池管理，由RPC通知與Ammo一同生成
 ***************************************************************************/
using Photon.Pun;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

namespace Kocmoca
{
    public class FireControlSystem : MonoBehaviour
    {
        public OnboardRadar radar;
        protected PhotonView myPhotonView;
        protected bool isLocalControUnit;
        protected float nextRound;
        protected Transform target;
        protected int targetNumber;
        protected int kocmoNumber;

#if UNITY_EDITOR
        public virtual void Preset(KocmocraftModule module) { }
#endif
        public virtual Vector3 ActiveHardpoint(PhotonView portPhotonView, int number, bool isLocal) { return Vector3.zero; }
        public virtual Vector3[] ActiveTurret(PhotonView portPhotonView, int number, bool isLocal) { return new Vector3[] { Vector3.zero }; }
        public virtual void Shoot() { }

        //private void Update()
        //{
        //    if (!isLocal)
        //        return;

        //    if (isReloading)
        //    {
        //        reloadingProcess = ((1 / timeReload) * (nextTimeAmmoLoaded - Time.time));
        //        if (Time.time >= nextTimeAmmoLoaded)
        //        {
        //            isReloading = false;
        //            countAmmo += 1;
        //        }
        //    }
        //    else
        //    {
        //        if (countAmmo < maxAmmoCapacity)
        //        {
        //            isReloading = true;
        //            nextTimeAmmoLoaded = Time.time + timeReload;
        //        }
        //    }
        //}


        //public Vector3 GetMuzzlePosition(int muzzle)
        //{
        //    return myTransform.TransformPoint(launcher[muzzle]);
        //}
    }
}