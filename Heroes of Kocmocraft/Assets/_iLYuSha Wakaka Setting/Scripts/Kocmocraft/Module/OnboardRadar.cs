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
        public Transform[] FriendAircraftsArray;
        public Transform[] FoeAircraftsArray;
        int faction;
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

        ModuleData moduleData;
        public void Initialize(Core core, int faction, int type, int number)
        {
            // Dependent Components
            myTransform = transform;
            myPhotonView = GetComponent<PhotonView>();
            // Modular Parameter
            isLocalPlayer = core == Core.LocalPlayer ? true : false;
            kocmonautNumber = number;
            //listFriendAircrafts = KocmocaData.factionData[faction].listFriend;
            //listFoeAircrafts = KocmocaData.factionData[faction].listFoe;
            FriendAircraftsArray = SatelliteCommander.factionData[faction].arrayFriend;
            FoeAircraftsArray = SatelliteCommander.factionData[faction].arrayFoe;
            this.faction = faction;

            targetOnboard = new Transform[maxtOnboardCount];
            minLockTime = 0.73f;

            moduleData = KocmocaData.KocmocraftData[type];
        }
        private void Update()
        {
            //myPosition = myTransform.position;
            //if (isLocalPlayer) SearchFriend();
            //SearchFoe();
            //RadarWarningEmitter();
            UpdateDefault();
        }
        void UpdateDefault()
        {
            myPosition = myTransform.position;
            if (isLocalPlayer) SearchFriend();
            SearchFoe();
            RadarWarningEmitter();
        }
        public void UpdateMe()
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
            int countFriend = FriendAircraftsArray.Length;
            for (int i = 0; i < countFriend; i++)
            {
                if (FriendAircraftsArray[i])
                {
                    if (FriendAircraftsArray[i] != myTransform)
                    {
                        scanDiff = FriendAircraftsArray[i].position - myPosition;
                        if (Vector3.SqrMagnitude(scanDiff) <= moduleData.MaxSearchRadiusSqr &&
                            Vector3.Dot(scanDiff.normalized, myTransform.forward) >= moduleData.MaxSearchRange)
                            HeadUpDisplayManager.Instance.NewIdentifyFriend(FriendAircraftsArray[i]); // 標記搜索範圍的所有友機
                    }
                }
            }
        }
        public void SearchFoe()
        {
            //TargetSearch();
            TargetSearch2();
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

                if (targetDistanceSqr < moduleData.MaxLockDistanceSqr && targetDirection > moduleData.MaxLockRange)
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

        void TargetSearch2()
        {
            countOnboard = 0;
            for (int k = 0; k < maxtOnboardCount; k++)
            {
                targetOnboard[k] = null;
            }
            targetAutoAim = null;
            targetNearest = null;
            targetNearestDirection = 0;
            int countFoe = FoeAircraftsArray.Length;
            for (int i = 0; i < countFoe; i++)
            {
                if (i < countFoe && FoeAircraftsArray[i])
                {
                    scanDiff = FoeAircraftsArray[i].position - myPosition;
                    scanDistanceSqr = Vector3.SqrMagnitude(scanDiff);
                    scanDirection = Vector3.Dot(scanDiff.normalized, myTransform.forward);
                    if (scanDistanceSqr <= moduleData.MaxSearchRadiusSqr && scanDirection >= moduleData.MaxSearchRange)
                    {
                        if (isLocalPlayer)
                            SatelliteCommander.Instance.IdentifyTarget(FoeAircraftsArray[i]); // 標記搜索範圍的所有敵機
                        else
                        {
                            if (countOnboard < 10)
                            {
                                targetOnboard[countOnboard] = FoeAircraftsArray[i];
                                countOnboard++;
                            }
                        }

                        if (scanDistanceSqr <= moduleData.MaxLockDistanceSqr && scanDirection >= moduleData.MaxLockRange)
                        {
                            if (isLocalPlayer)
                                SatelliteCommander.Instance.FireControlLookTarget(FoeAircraftsArray[i]);  // 標記鎖定範圍的所有敵機

                            // 尋找最接近的目標
                            if (scanDirection > targetNearestDirection)
                            {
                                targetNearest = FoeAircraftsArray[i];
                                targetNearestDirection = scanDirection;

                                if (scanDirection > moduleData.MaxAutoAimRange)
                                    targetAutoAim = targetNearest;

                            }
                        }
                    }
                }
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
            int countFoe = SatelliteCommander.factionData[faction].arrayFoe.Length;
            for (int i = 0; i < countFoe; i++)
            {
                if (i < countFoe && SatelliteCommander.factionData[faction].arrayFoe[i])
                {
                    scanDiff = SatelliteCommander.factionData[faction].arrayFoe[i].position - myPosition;
                    scanDistanceSqr = Vector3.SqrMagnitude(scanDiff);
                    scanDirection = Vector3.Dot(scanDiff.normalized, myTransform.forward);
                    if (scanDistanceSqr <= moduleData.MaxSearchRadiusSqr && scanDirection >= moduleData.MaxSearchRange)
                    {
                        if (isLocalPlayer)
                            SatelliteCommander.Instance.IdentifyTarget(SatelliteCommander.factionData[faction].arrayFoe[i]); // 標記搜索範圍的所有敵機
                        else
                        {
                            if (countOnboard < 10)
                            {
                                targetOnboard[countOnboard] = SatelliteCommander.factionData[faction].arrayFoe[i];
                                countOnboard++;
                            }
                        }

                        if (scanDistanceSqr <= moduleData.MaxLockDistanceSqr && scanDirection >= moduleData.MaxLockRange)
                        {
                            if (isLocalPlayer)
                                SatelliteCommander.Instance.FireControlLookTarget(SatelliteCommander.factionData[faction].arrayFoe[i]);  // 標記鎖定範圍的所有敵機

                            // 尋找最接近的目標
                            if (scanDirection > targetNearestDirection)
                            {
                                targetNearest = SatelliteCommander.factionData[faction].arrayFoe[i];
                                targetNearestDirection = scanDirection;

                                if (scanDirection > moduleData.MaxAutoAimRange)
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