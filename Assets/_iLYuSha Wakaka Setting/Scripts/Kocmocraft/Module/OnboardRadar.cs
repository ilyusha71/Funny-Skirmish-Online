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
        private int maxSearchRadius = 3000; // 最大搜索半徑
        private float maxSearchAngle = Mathf.Cos(37 * Mathf.Deg2Rad); // 最大搜索夾角
        [HideInInspector]
        public int maxLockRadius = 1370; // 最大鎖定半徑
        [HideInInspector]
        public float maxLockAngle = Mathf.Cos(27 * Mathf.Deg2Rad); // 最大鎖定夾角
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
        private float targetDistance;
        private float targetDirection;

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
            TargetSearch();
            TargetLockOn();
            RadarWarningEmitter();
        }
        void SearchFriend()
        {
            SatelliteCommander.Instance.ResetOnboardRadarRadar();
            int countFriend = listFriendAircrafts.Count;
            for (int i = 0; i < countFriend; i++)
            {
                if (listFriendAircrafts[i])
                {
                    if (listFriendAircrafts[i] != myTransform)
                    {
                        float distance = Vector3.Distance(listFriendAircrafts[i].position, myPosition);
                        float direction = Vector3.Dot((listFriendAircrafts[i].position - myPosition).normalized, myTransform.forward);
                        if (distance <= maxSearchRadius && direction >= maxSearchAngle)
                            SatelliteCommander.Instance.IdentifyFriend(listFriendAircrafts[i]); // 標記搜索範圍的所有友機
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
                    float distance = Vector3.Distance(listFoeAircrafts[i].position, myPosition);
                    float direction = Vector3.Dot((listFoeAircrafts[i].position - myPosition).normalized, myTransform.forward);
                    if (distance <= maxSearchRadius && direction >= maxSearchAngle)
                    {
                        targetOnboard[i] = listFoeAircrafts[i];
                        if (isLocalPlayer)
                            SatelliteCommander.Instance.IdentifyTarget(listFoeAircrafts[i]); // 標記搜索範圍的所有敵機

                        if (distance <= maxLockRadius && direction >= maxLockAngle)
                        {
                            if (isLocalPlayer)
                                SatelliteCommander.Instance.FireControlLookTarget(listFoeAircrafts[i]);  // 標記鎖定範圍的所有敵機

                            // 尋找最接近的目標
                            if (direction > targetNearestDirection)
                            {
                                targetNearest = listFoeAircrafts[i];
                                targetNearestDirection = direction;
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
                targetDistance = Vector3.Distance(myPosition, targetRadarLockOn.position);
                targetDirection = Vector3.Dot((targetRadarLockOn.position - myPosition).normalized, myTransform.forward);

                if (targetDirection > KocmoMissileLauncher.maxFireAngle && targetDistance < maxLockRadius)
                {
                    if (targetDirection < KocmoLaserCannon.maxFireAngle)
                        targetAutoAim = null;
                    if (isLocalPlayer)
                        HeadUpDisplayManager.Instance.MarkTarget(targetRadarLockOn, targetDistance);
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