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
        public Transform targetAutoAim;
        public Transform targetRadarLockOn;
        public Transform targetLocked;

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
            //TargetSearch();
            TargetLockOn();
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
        void TargetSearch()
        {
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
                            }
                        }
                    }
                }
            }
        }
        void TargetLockOn()
        {
            if (Time.time > nextLockTime)
            {
                targetAutoAim = targetNearest;
                if (!targetRadarLockOn) targetRadarLockOn = targetNearest;
                if (!isLocalPlayer) targetTrack = targetNearest;
                //if (isLocalPlayer)
                //{
                //    if (!targetRadarLockOn || Input.GetKeyDown(KeyCode.R))
                //        targetRadarLockOn = targetNearest;
                //}
                //else
                //{
                //    targetTrack = targetNearest;
                //    targetRadarLockOn = targetNearest;
                //}
            }
            if (targetRadarLockOn)
            {
                targetDiff = targetRadarLockOn.position - myPosition;
                targetDistanceSqr = Vector3.SqrMagnitude(targetDiff);
                targetDirection = Vector3.Dot(targetDiff.normalized, myTransform.forward);

                if (targetDistanceSqr < RadarParameter.maxLockDistanceSqr && targetDirection > KocmoMissileLauncher.maxFireAngle)
                {
                    if (targetDirection < KocmoLaserCannon.maxFireAngle)
                        targetAutoAim = null;
                    if (isLocalPlayer)
                        HeadUpDisplayManager.Instance.MarkTarget(targetRadarLockOn, targetDistanceSqr);
                }
                else
                {
                    nextLockTime = Time.time + minLockTime;
                    targetAutoAim = null;
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
                TargetSearch();
            }
            //if (isLocalPlayer)
            //{
            //   // SatelliteCommander.Instance.MarkTarget();
            //}
        }
        void RadarWarningEmitter()
        {
            if (targetRadarLockOn != targetLocked)
            {
                if (targetRadarLockOn)
                {
                    KocmocraftMechDroid target = targetRadarLockOn.GetComponent<KocmocraftMechDroid>();
                    if (target.Core == Core.LocalPlayer || target.Core == Core.RemotePlayer)
                        myPhotonView.RPC("RadarLockOn", RpcTarget.AllViaServer, target.Number, kocmonautNumber, true);
                }
                if (targetLocked)
                {
                    KocmocraftMechDroid target = targetLocked.GetComponent<KocmocraftMechDroid>();
                    if (target.Core == Core.LocalPlayer || target.Core == Core.RemotePlayer)
                        myPhotonView.RPC("RadarLockOn", RpcTarget.AllViaServer, target.Number, kocmonautNumber, false);
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