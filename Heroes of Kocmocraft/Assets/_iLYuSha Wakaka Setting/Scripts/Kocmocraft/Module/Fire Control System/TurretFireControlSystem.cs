/***************************************************************************
 * Turret Fire Control System (TFCS)
 * 機炮火控系統
 * Last Updated: 2019/06/03
 * 
 * v19.0603
 * 1. 繼承FireControlSystem的獨立TurretFireControlSystem
 * 2. 直接從Turret取值
 * 3. 射擊音效使用物件池管理，由RPC通知與Ammo一同生成
 ***************************************************************************/
using Photon.Pun;
using UnityEngine;

namespace Kocmoca
{
    public class TurretFireControlSystem : FireControlSystem
    {
        public Turret turret;
        private int currentTurret;
        private int countTurret;
        private Vector3[] turretPos;
        private int turnFire;

#if UNITY_EDITOR
        public override void Preset(KocmocraftModule module)
        {
            turret = module.turret;
            radar = GetComponentInParent<OnboardRadar>();
            Transform[] turretsPoint = GetComponentsInChildren<Transform>();
            countTurret = (turretsPoint.Length - 1) * 2; // GetComponentsInChildren always include parent components
            turretPos = new Vector3[countTurret];
            for (int i = 0; i < countTurret; i++)
            {
                turretPos[i] = turretsPoint[Mathf.CeilToInt((i + 1) * 0.5f)].localPosition; // 1,1,2,2,3,3...
                if (i % 2 == 1)
                    turretPos[i].x = -turretPos[i].x; // 1,-1,2,-2,3,-3...
            }
            turnFire = Mathf.RoundToInt(countTurret * 0.5f);
        }
#endif
        public override Vector3[] ActiveTurret(PhotonView portPhotonView, int number, bool isLocal)
        {
            myPhotonView = portPhotonView;
            kocmoNumber = number;
            isLocalControUnit = isLocal; ;
            return turretPos;
        }
        public override void Shoot()
        {
            if (Time.time > nextRound)
            {
                nextRound = Time.time + turret.fireRate;
             TurretControl();

                switch (turret.repeatingCount)
                {
                    case 1: TurretControl(); break;
                    case 2: Invoke("TurretControl", 0.3f); break;
                    case 3:
                        Invoke("TurretControl", 0.12f);
                        Invoke("TurretControl", 0.24f);
                        break;
                }
            }
        }
        public void TurretControl()
        {
            currentTurret++;
            currentTurret = (int)Mathf.Repeat(currentTurret, countTurret);
            target = radar.autoAimTarget;
            targetNumber = target ? target.GetComponent<AvionicsSystem>().kocmoNumber : 0;

            // volley on one side
            for (int t = 0; t < turnFire; t++)
            {
                myPhotonView.RPC("TurretFire", RpcTarget.AllViaServer, currentTurret, kocmoNumber, targetNumber, Random.Range(-turret.maxProjectileSpread, turret.maxProjectileSpread));
                currentTurret += 2;
                currentTurret = (int)Mathf.Repeat(currentTurret, countTurret);
            }

            // 改物件池 與子彈一起
            //if (t == 0)
            //    m_AudioSource.PlayOneShot(fireSound, 0.73f);
        }
    }
}