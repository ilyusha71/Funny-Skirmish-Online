/***************************************************************************
 * Onboard Radar
 * 機載雷達
 * Last Updated: 2018/10/21
 * Description:
 * 1. Radar System -> Onboard Radar
 * 2. 尋找與標示敵我航機
 * 3. 輔助FCS鎖定目標
 ***************************************************************************/
using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Kocmoca
{
    public class OnboardRadar : MonoBehaviour
    {
        [Header("Dependent Components")]
        private Transform myTransform;
        private PhotonView myPhotonView;
        [Header("Kocmonaut Data")]
        private bool isLocalPlayer;
        public int kocmonautNumber;
        [Header("Modular Parameter")]
        public List<Transform> listFriendAircrafts = new List<Transform>();
        public List<Transform> listFoeAircrafts = new List<Transform>();
        private Vector3 myPosition;
        [Header("Onboard Radar")]
        public Transform[] targetOnboard;
        private const int maxtOnboardCount = 10;
        private int countOnboard = 0;
        // Search
        private Vector3 scanDiff;
        private float scanDistanceSqr;
        private float scanDirection;
        // Target
        private Vector3 targetDiff;
        private float targetDistanceSqr;
        private float targetDirection;
        [Header("Fire-control Radar")]
        public Transform targetNearest;
        //private float targetNearestDistance;
        private float targetNearestDirection;
        private float minLockTime;
        private float nextLockTime;
        [Header("Target")]
        public Transform targetTrack; // 僅用於Bot跟隨目標使用
        public Transform targetLocked;

        [Header("Auto Aim")] // 自動瞄準系統（適用Player與Bot）
        public Transform targetAutoAim; // 自動瞄準在機炮範圍內最接近中心的目標
        // 機炮範圍根據宇航機的KocmoLaser決定
        // 開火時機炮只會射擊Auto Aim目標，不會射擊RadarLockOn目標，除非兩者目標相同

        [Header("Radar Lock On")] // 雷達鎖定系統（適用Player與Bot）
        public Transform targetRadarLockOn; // 符合機載

        public int MaxSearchRadiusSqr;
        public int MaxLockDistanceSqr;
        public float MaxSearchAngle;
        public float MaxLockAngle;
        public float MaxAutoAim;

        public void Initialize(Core core, int faction, Type type, int number)
        {
            // Dependent Components
            myTransform = transform;
            myPhotonView = GetComponent<PhotonView>();
            // Modular Parameter
            isLocalPlayer = core == Core.LocalPlayer ? true : false;
            kocmonautNumber = number;
            listFriendAircrafts = SatelliteCommander.Instance.factionData[faction].listFriend;
            listFoeAircrafts = SatelliteCommander.Instance.factionData[faction].listFoe;

            targetOnboard = new Transform[maxtOnboardCount];
            minLockTime = 0.73f;

            switch (type)
            {
                case Type.MinionArmor:
                    MaxSearchRadiusSqr = ShortRangeRadar.MaxSearchRadiusSqr;
                    MaxLockDistanceSqr = ShortRangeRadar.MaxLockDistanceSqr;
                    MaxSearchAngle = ShortRangeRadar.MaxSearchAngle;
                    MaxLockAngle = ShortRangeRadar.MaxLockAngle;
                    MaxAutoAim = ShortRangeRadar.MaxAutoAim;
                    break;
                case Type.RedBullEnergy:
                    MaxSearchRadiusSqr = ShortRangeRadar.MaxSearchRadiusSqr;
                    MaxLockDistanceSqr = ShortRangeRadar.MaxLockDistanceSqr;
                    MaxSearchAngle = ShortRangeRadar.MaxSearchAngle;
                    MaxLockAngle = ShortRangeRadar.MaxLockAngle;
                    MaxAutoAim = ShortRangeRadar.MaxAutoAim;
                    break;
                case Type.VladimirPutin:
                    MaxSearchRadiusSqr = LongRangeRadar.MaxSearchRadiusSqr;
                    MaxLockDistanceSqr = LongRangeRadar.MaxLockDistanceSqr;
                    MaxSearchAngle = LongRangeRadar.MaxSearchAngle;
                    MaxLockAngle = LongRangeRadar.MaxLockAngle;
                    MaxAutoAim = KocmoMegaRailgun.MaxAutoAimRange;
                    break;
                //case Type.PaperAeroplane:
                //    MaxSearchRadiusSqr = ExtremelyLongRangeRadar.MaxSearchRadiusSqr;
                //    MaxLockDistanceSqr = ExtremelyLongRangeRadar.MaxLockDistanceSqr;
                //    MaxSearchAngle = ExtremelyLongRangeRadar.MaxSearchAngle;
                //    MaxLockAngle = ExtremelyLongRangeRadar.MaxLockAngle;
                //    MaxAutoAim = DevilTenderGazer.MaxAutoAimRange;
                //    break;
                case Type.Cuckoo:
                    MaxSearchRadiusSqr = VeryLongRangeRadar.MaxSearchRadiusSqr;
                    MaxLockDistanceSqr = VeryLongRangeRadar.MaxLockDistanceSqr;
                    MaxSearchAngle = VeryLongRangeRadar.MaxSearchAngle;
                    MaxLockAngle = VeryLongRangeRadar.MaxLockAngle;
                    MaxAutoAim = DevilTenderGazer.MaxAutoAimRange;
                    break;
                case Type.BulletBill:
                    MaxSearchRadiusSqr = ShortRangeRadar.MaxSearchRadiusSqr;
                    MaxLockDistanceSqr = ShortRangeRadar.MaxLockDistanceSqr;
                    MaxSearchAngle = ShortRangeRadar.MaxSearchAngle;
                    MaxLockAngle = ShortRangeRadar.MaxLockAngle;
                    MaxAutoAim = ShortRangeRadar.MaxAutoAim;
                    break;
                case Type.TimeMachine:
                    MaxSearchRadiusSqr = VeryLongRangeRadar.MaxSearchRadiusSqr;
                    MaxLockDistanceSqr = VeryLongRangeRadar.MaxLockDistanceSqr;
                    MaxSearchAngle = VeryLongRangeRadar.MaxSearchAngle;
                    MaxLockAngle = VeryLongRangeRadar.MaxLockAngle;
                    MaxAutoAim = VeryLongRangeRadar.MaxAutoAim;
                    break;
                case Type.AceKennel:
                    MaxSearchRadiusSqr = UltraWideRangeRadar.MaxSearchRadiusSqr;
                    MaxLockDistanceSqr = UltraWideRangeRadar.MaxLockDistanceSqr;
                    MaxSearchAngle = UltraWideRangeRadar.MaxSearchAngle;
                    MaxLockAngle = UltraWideRangeRadar.MaxLockAngle;
                    MaxAutoAim = UltraWideRangeRadar.MaxAutoAim;
                    break;
                case Type.KirbyStar:
                    MaxSearchRadiusSqr = UltraWideRangeRadar.MaxSearchRadiusSqr;
                    MaxLockDistanceSqr = UltraWideRangeRadar.MaxLockDistanceSqr;
                    MaxSearchAngle = UltraWideRangeRadar.MaxSearchAngle;
                    MaxLockAngle = UltraWideRangeRadar.MaxLockAngle;
                    MaxAutoAim = UltraWideRangeRadar.MaxAutoAim;
                    break;
                case Type.nWidia:
                    MaxSearchRadiusSqr = LongRangeRadar.MaxSearchRadiusSqr;
                    MaxLockDistanceSqr = LongRangeRadar.MaxLockDistanceSqr;
                    MaxSearchAngle = LongRangeRadar.MaxSearchAngle;
                    MaxLockAngle = LongRangeRadar.MaxLockAngle;
                    MaxAutoAim = LongRangeRadar.MaxAutoAim;
                    break;
                //case Type.FastFoodMan:
                //    MaxSearchRadiusSqr = ShortRangeRadar.MaxSearchRadiusSqr;
                //    MaxLockDistanceSqr = ShortRangeRadar.MaxLockDistanceSqr;
                //    MaxSearchAngle = ShortRangeRadar.MaxSearchAngle;
                //    MaxLockAngle = ShortRangeRadar.MaxLockAngle;
                //    MaxAutoAim = ShortRangeRadar.MaxAutoAim;
                //    break;
                //case Type.PolarisExpress:
                //    MaxSearchRadiusSqr = LongRangeRadar.MaxSearchRadiusSqr;
                //    MaxLockDistanceSqr = LongRangeRadar.MaxLockDistanceSqr;
                //    MaxSearchAngle = LongRangeRadar.MaxSearchAngle;
                //    MaxLockAngle = LongRangeRadar.MaxLockAngle;
                //    MaxAutoAim = KocmoMegaRailgun.MaxAutoAimRange;
                //    break;
                case Type.PapoyUnicorn:
                    MaxSearchRadiusSqr = ExtremelyLongRangeRadar.MaxSearchRadiusSqr;
                    MaxLockDistanceSqr = ExtremelyLongRangeRadar.MaxLockDistanceSqr;
                    MaxSearchAngle = ExtremelyLongRangeRadar.MaxSearchAngle;
                    MaxLockAngle = ExtremelyLongRangeRadar.MaxLockAngle;
                    MaxAutoAim = DevilTenderGazer.MaxAutoAimRange;
                    break;
                case Type.PumpkinGhost:
                    MaxSearchRadiusSqr = VeryLongRangeRadar.MaxSearchRadiusSqr;
                    MaxLockDistanceSqr = VeryLongRangeRadar.MaxLockDistanceSqr;
                    MaxSearchAngle = VeryLongRangeRadar.MaxSearchAngle;
                    MaxLockAngle = VeryLongRangeRadar.MaxLockAngle;
                    MaxAutoAim = KocmoMegaRailgun.MaxAutoAimRange;
                    break;
                case Type.GrandLisboa:
                    MaxSearchRadiusSqr = MediumRangeRadar.MaxSearchRadiusSqr;
                    MaxLockDistanceSqr = MediumRangeRadar.MaxLockDistanceSqr;
                    MaxSearchAngle = MediumRangeRadar.MaxSearchAngle;
                    MaxLockAngle = MediumRangeRadar.MaxLockAngle;
                    MaxAutoAim = MediumRangeRadar.MaxAutoAim;
                    break;
                default:
                    MaxSearchRadiusSqr = RadarParameter.maxSearchRadiusSqr;
                    MaxLockDistanceSqr = RadarParameter.maxLockDistanceSqr;
                    MaxSearchAngle = RadarParameter.maxSearchAngle;
                    MaxLockAngle = RadarParameter.maxLockAngle;
                    MaxAutoAim = KocmoLaserCannon.maxFireAngle;
                    break;
            }
        }
        private void Update()
        {
            myPosition = myTransform.position;
            if (isLocalPlayer) SearchFriend();
            SearchFoe();
            RadarWarningEmitter();
        }
        void SearchFriend()
        {
            SatelliteCommander.Instance.ResetOnboardRadarRadar();
            HeadUpDisplayManager.Instance.NewResetOnboardRadarRadar();
            int countFriend = listFriendAircrafts.Count;
            for (int i = 0; i < countFriend; i++)
            {
                if (listFriendAircrafts[i])
                {
                    if (listFriendAircrafts[i] != myTransform)
                    {
                        scanDiff = listFriendAircrafts[i].position - myPosition;
                        if (Vector3.SqrMagnitude(scanDiff) <= MaxSearchRadiusSqr &&
                            Vector3.Dot(scanDiff.normalized, myTransform.forward) >= MaxSearchAngle)
                            HeadUpDisplayManager.Instance.NewIdentifyFriend(listFriendAircrafts[i]); // 標記搜索範圍的所有友機
                    }
                }
            }
        }
        void SearchFoe()
        {
            TargetSearch();
            if (Time.time > nextLockTime)
            {
                if (!targetRadarLockOn) targetRadarLockOn = targetNearest;
                if (!isLocalPlayer) targetTrack = targetNearest;
            }


            if (targetRadarLockOn)
            {
                targetDiff = targetRadarLockOn.position - myPosition;
                targetDistanceSqr = Vector3.SqrMagnitude(targetDiff);
                targetDirection = Vector3.Dot(targetDiff.normalized, myTransform.forward);

                if (targetDistanceSqr < MaxLockDistanceSqr && targetDirection > MaxLockAngle)
                {
                    if (isLocalPlayer)
                        HeadUpDisplayManager.Instance.MarkTarget(targetRadarLockOn, targetDistanceSqr);
                }
                else
                {
                    nextLockTime = Time.time + minLockTime;
                    targetTrack = null;
                    targetRadarLockOn = null;
                }
            }
            else
            {
                //if (!isLocalPlayer)
                //{
                //    for (int i = 0; i < maxtOnboardCount; i++)
                //    {
                //        if (targetOnboard[i])
                //            targetTrack = targetOnboard[i];
                //    }
                //}
            }
        }
        void TargetSearch()
        {
            countOnboard = 0;
            for (int k = 0; k < maxtOnboardCount; k++)
            {
                targetOnboard[k] = null;
            }
            targetAutoAim = null;
            targetNearest = null;
            targetNearestDirection = 0;
            int countFoe = listFoeAircrafts.Count;
            for (int i = 0; i < countFoe; i++)
            {
                if (i < countFoe && listFoeAircrafts[i])
                {
                    scanDiff = listFoeAircrafts[i].position - myPosition;
                    scanDistanceSqr = Vector3.SqrMagnitude(scanDiff);
                    scanDirection = Vector3.Dot(scanDiff.normalized, myTransform.forward);
                    if (scanDistanceSqr <= MaxSearchRadiusSqr && scanDirection >= MaxSearchAngle)
                    {
                        if (isLocalPlayer)
                            SatelliteCommander.Instance.IdentifyTarget(listFoeAircrafts[i]); // 標記搜索範圍的所有敵機
                        else
                        {
                            if (countOnboard < 10)
                            {
                                targetOnboard[countOnboard] = listFoeAircrafts[i];
                                countOnboard++;
                            }
                        }

                        if (scanDistanceSqr <= MaxLockDistanceSqr && scanDirection >= MaxLockAngle)
                        {
                            if (isLocalPlayer)
                                SatelliteCommander.Instance.FireControlLookTarget(listFoeAircrafts[i]);  // 標記鎖定範圍的所有敵機

                            // 尋找最接近的目標
                            if (scanDirection > targetNearestDirection)
                            {
                                targetNearest = listFoeAircrafts[i];
                                targetNearestDirection = scanDirection;

                                if (scanDirection > MaxAutoAim)
                                    targetAutoAim = targetNearest;

                            }
                        }
                    }
                }
            }
        }
        void RadarWarningEmitter()
        {
            if (targetRadarLockOn != targetLocked)
            {
                if (targetRadarLockOn)
                {
                    KocmocraftMechDroid target = targetRadarLockOn.GetComponent<KocmocraftMechDroid>();
                    if (target.Core == Core.LocalPlayer || target.Core == Core.RemotePlayer)
                    {
                        //Debug.Log(myTransform.name+"/"+myPhotonView.ViewID + "/" + kocmonautNumber + "/" + Time.frameCount + "/True/");
                        myPhotonView.RPC("RadarLockOn", RpcTarget.AllViaServer, target.Number, kocmonautNumber, true);
                    }
                }
                if (targetLocked)
                {
                    KocmocraftMechDroid target = targetLocked.GetComponent<KocmocraftMechDroid>();
                    if (target.Core == Core.LocalPlayer || target.Core == Core.RemotePlayer)
                    {
                        //Debug.Log(myTransform.name + "/" + myPhotonView.ViewID + "/" + kocmonautNumber + "/" + Time.frameCount + "/false/");
                        myPhotonView.RPC("RadarLockOn", RpcTarget.AllViaServer, target.Number, kocmonautNumber, false);
                    }
                }
                targetLocked = targetRadarLockOn;
            }
        }
        public void ManualLockOn()
        {
            targetRadarLockOn = targetNearest;
        }
        public void Stop() // 应改写成坠机Crash呼叫，不要使用Destroy
        {
            targetRadarLockOn = null;
            RadarWarningEmitter();
        }
    }
}