/***************************************************************************
 * Rocket Fire Control System (RFCS)
 * 導彈火控系統
 * Last Updated: 2019/06/03
 * 
 * v19.0603
 * 1. 繼承FireControlSystem的獨立RocketFireControlSystem
 * 2. 直接從Rocket取值
 * 3. 射擊音效使用物件池管理，由RPC通知與Ammo一同生成
 ***************************************************************************/
using Photon.Pun;
using UnityEngine;

namespace Kocmoca
{
    public class RocketFireControlSystem : FireControlSystem
    {
        public Turret turret;
        private Vector3 hardpointPos;
        public int countAmmo;


#if UNITY_EDITOR
        public override void Preset(KocmocraftModule module)
        {
            turret = module.turret;
            radar = GetComponentInParent<OnboardRadar>();
            hardpointPos = transform.GetChild(0).localPosition;
        }
#endif
        public override Vector3 ActiveHardpoint(PhotonView portPhotonView, int number, bool isLocal)
        {
            myPhotonView = portPhotonView;
            kocmoNumber = number;
            isLocalControUnit = isLocal; ;
            return hardpointPos;
        }
        public override void Shoot()
        {
            if (countAmmo > 0 && Time.time > nextRound)
            {
                countAmmo--;
                nextRound = Time.time + turret.fireRate;
                target = radar.nearestTarget;
                targetNumber = target ? target.GetComponent<AvionicsSystem>().kocmoNumber : 0;
                myPhotonView.RPC("RockeLaunch", RpcTarget.AllViaServer, kocmoNumber, targetNumber);
            }
        }
    }
}