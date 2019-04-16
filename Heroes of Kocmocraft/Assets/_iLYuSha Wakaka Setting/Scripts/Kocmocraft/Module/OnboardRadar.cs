/***************************************************************************
 * Onboard Radar
 * 機載雷達
 * Last Updated: 2018/10/21
 * Description:
 * 1. Radar System -> Onboard Radar
 * 2. 尋找與標示敵我航機
 * 3. 輔助FCS鎖定目標
 * 
 * Level 1：短程雷達（適合新手）（人機配置：低速宇航機）
 *      a. 機炮最大開火角度：7°
 *      b. 投射武器最大釋放角度：9°
 *      c. 追蹤武器最大釋放角度：21°
 *      d. 最遠自動瞄準距離：700m
 * Level 2：中程雷達（適合進階玩家）（人機配置：中速宇航機配置）
 *      a. 機炮最大開火角度：5°
 *      b. 投射武器最大釋放角度：7°
 *      c. 追蹤武器最大釋放角度：16°
 *      d. 最遠自動瞄準距離：1600m
 * Level 3：長程雷達系統（適合資深老手）（人機配置：高速宇航機配置）
 *      a. 機炮最大開火角度：3°
 *      b. 投射武器最大釋放角度：5°
 *      c. 追蹤武器最大釋放角度：11°
 *      d. 最遠自動瞄準距離：3000m
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
        private const int maxTargetCount = 10;
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
        public Transform targetTrack;
        public Transform targetLocked;

        [Header("Auto Aim")] // 自動瞄準系統（適用Player與Bot）
        public Transform targetAutoAim; // 自動瞄準在機炮範圍內最接近中心的目標
        // 機炮範圍根據宇航機的KocmoLaser決定
        // 開火時機炮只會射擊Auto Aim目標，不會射擊RadarLockOn目標，除非兩者目標相同

        [Header("Radar Lock On")] // 雷達鎖定系統（適用Player與Bot）
        public Transform targetRadarLockOn; // 符合機載

        public void Initialize(Core core, int faction, int type, int number)
        {
            // Dependent Components
            myTransform = transform;
            myPhotonView = GetComponent<PhotonView>();
            // Modular Parameter
            isLocalPlayer = core == Core.LocalPlayer ? true : false;
            kocmonautNumber = number;
            listFriendAircrafts = SatelliteCommander.Instance.factionData[faction].listFriend;
            listFoeAircrafts = SatelliteCommander.Instance.factionData[faction].listFoe;

            targetOnboard = new Transform[maxTargetCount];
            minLockTime = 0.73f;
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
                        if (Vector3.SqrMagnitude(scanDiff) <= RadarParameter.maxSearchRadiusSqr &&
                            Vector3.Dot(scanDiff.normalized, myTransform.forward) >= RadarParameter.maxSearchAngle)
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

                if (targetDistanceSqr < RadarParameter.maxLockDistanceSqr && targetDirection > KocmoMissileLauncher.maxFireAngle)
                {
                    if (isLocalPlayer)
                        HeadUpDisplayManager.Instance.MarkTarget(targetRadarLockOn, targetDistanceSqr);
                }
                else
                {
                    nextLockTime = Time.time + minLockTime;
                    targetRadarLockOn = null;
                }
            }
            else
            {
                if (!isLocalPlayer)
                {
                    for (int i = 0; i < maxTargetCount; i++)
                    {
                        if (targetOnboard[i])
                            targetTrack = targetOnboard[i];
                    }
                }
            }
        }
        void TargetSearch()
        {
            targetAutoAim = null;
            targetNearest = null;
            targetNearestDirection = 0;
            int countFoe = listFoeAircrafts.Count;
            for (int i = 0; i < maxTargetCount; i++)
            {
                targetOnboard[i] = null;
                if (i < countFoe && listFoeAircrafts[i])
                {
                    scanDiff = listFoeAircrafts[i].position - myPosition;
                    scanDistanceSqr = Vector3.SqrMagnitude(scanDiff);
                    scanDirection = Vector3.Dot(scanDiff.normalized, myTransform.forward);
                    if (scanDistanceSqr <= RadarParameter.maxSearchRadiusSqr && scanDirection >= RadarParameter.maxSearchAngle)
                    {
                        targetOnboard[i] = listFoeAircrafts[i];
                        if (isLocalPlayer)
                            SatelliteCommander.Instance.IdentifyTarget(listFoeAircrafts[i]); // 標記搜索範圍的所有敵機

                        if (scanDistanceSqr <= RadarParameter.maxLockDistanceSqr && scanDirection >= RadarParameter.maxLockAngle)
                        {
                            if (isLocalPlayer)
                                SatelliteCommander.Instance.FireControlLookTarget(listFoeAircrafts[i]);  // 標記鎖定範圍的所有敵機

                            // 尋找最接近的目標
                            if (scanDirection > targetNearestDirection)
                            {
                                targetNearest = listFoeAircrafts[i];
                                targetNearestDirection = scanDirection;

                                if (scanDirection > KocmoLaserCannon.maxFireAngle)
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