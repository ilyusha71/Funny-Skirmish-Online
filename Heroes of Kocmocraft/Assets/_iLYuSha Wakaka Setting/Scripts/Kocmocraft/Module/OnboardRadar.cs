/***************************************************************************
 * Onboard Radar
 * 機載雷達
 * Last Updated: 2019/05/27
 * 
 * v19.0527
 * 1. 计算相对位置，使用InverseTransformPoint比使用Vector3相减，效能提升137%
 * 2. 计算Vector3.Dot，使用Vector3.forward比使用portTransform.forward，效能提升127%
 * 
 * v18.1021
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
        [Header("Preset Data")]
        public Radar radar;
        public Turret turret;
        public Kocmomech kocmomech;
        [Header("Active System")]
        public int kocmoNumber;
        public int indexFaction;
        public bool isLocalPlayer;
        public Transform[] listFriend;
        public Transform[] listFoe;
        private Transform portTransform;
        private PhotonView portPhotonView;

        private int index; // for loop index
        private Vector3 relativePoint;
        private float distanceSqr;
        private float angle;
        [Header("Variable")]
        [Tooltip("距离最近的目标")]
        public Transform nearestTarget;
        private float nearestDistanceSqr;
        [Tooltip("自动瞄准的目标")]
        public Transform autoAimTarget;
        private float minAngle;





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

        public void Preset(KocmocraftModule module)
        {
            radar = module.radar;
            turret = module.turret;
            kocmomech = module.kocmomech;
            enabled = false;
        }

        public void Active(Transform rootTransform, PhotonView rootPhotonView, int rootFaction,int rootNumber, bool rootIsLocalPlayer)
        {
            enabled = true;
            portTransform = rootTransform;
            portPhotonView = rootPhotonView;
            indexFaction = rootFaction;
            kocmoNumber = rootNumber;
            isLocalPlayer = rootIsLocalPlayer;
            listFriend = SatelliteCommander.factionData[indexFaction].arrayFriend;
            listFoe = SatelliteCommander.factionData[indexFaction].arrayFoe;

            //targetOnboard = new Transform[maxtOnboardCount];
        }
        private void Update()
        {
            Search();
            //if (!active) return;

            //myPosition = portTransform.position;
            //if (isLocalPlayer) SearchFriend();
            //SearchFoe();
            //RadarWarningEmitter();
            //UpdateDefault();
        }
        public void Search()
        {
            myPosition = portTransform.position;
            nearestTarget = null;
            nearestDistanceSqr = Mathf.Infinity;
            autoAimTarget = null;
            minAngle = -1;

            for (index = 0; index < 50; index++)
            {
                if (isLocalPlayer)
                {
                    if (listFriend[index])
                    {
                        relativePoint = portTransform.InverseTransformPoint(listFriend[index].position);
                        distanceSqr = Vector3.SqrMagnitude(relativePoint);
                        angle = Vector3.Dot(relativePoint.normalized, Vector3.forward);

                        if (distanceSqr <= radar.radiusSqr && angle >= radar.range)
                            HeadUpDisplayManager.Instance.NewIdentifyFriend(listFriend[index]); // 標記友機
                    }
                }

                if (listFoe[index])
                {
                    relativePoint = portTransform.InverseTransformPoint(listFoe[index].position);
                    distanceSqr = Vector3.SqrMagnitude(relativePoint);
                    angle = Vector3.Dot(relativePoint.normalized, Vector3.forward);

                    if (distanceSqr <= radar.radiusSqr && angle >= radar.range)
                    {
                        if (isLocalPlayer)
                            SatelliteCommander.Instance.IdentifyTarget(listFoe[index]); // 標記敵機 与下面合并
                        //SatelliteCommander.Instance.FireControlLookTarget(listFoe[index]);  // 標記鎖定範圍的所有敵機

                        // 距离最接近的目标
                        if (distanceSqr > nearestDistanceSqr)
                        {
                            nearestTarget = listFoe[index];
                            nearestDistanceSqr = distanceSqr;
                        }
                        // 自动瞄准的目标
                        if (angle >= turret.maxAutoAimRange && angle >= minAngle)
                        {
                            autoAimTarget = listFoe[index];
                            minAngle = angle;
                        }
                    }
                }
            }
        }

















        void UpdateDefault()
        {
            myPosition = portTransform.position;
            if (isLocalPlayer) SearchFriend();
            SearchFoe();
            RadarWarningEmitter();
        }
        public void UpdateMe()
        {
            myPosition = portTransform.position;
            if (isLocalPlayer) SearchFriend();
            SearchFoe();
            RadarWarningEmitter();
        }



        void SearchFriend()
        {
            SatelliteCommander.Instance.ResetOnboardRadarRadar();
            HeadUpDisplayManager.Instance.NewResetOnboardRadarRadar();
            int countFriend = listFriend.Length;
            for (int i = 0; i < countFriend; i++)
            {
                if (listFriend[i])
                {
                    if (listFriend[i] != portTransform)
                    {
                        scanDiff = listFriend[i].position - myPosition;
                        if (Vector3.SqrMagnitude(scanDiff) <= moduleData.MaxSearchRadiusSqr &&
                            Vector3.Dot(scanDiff.normalized, portTransform.forward) >= moduleData.MaxSearchRange)
                            HeadUpDisplayManager.Instance.NewIdentifyFriend(listFriend[i]); // 標記搜索範圍的所有友機
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
                targetDirection = Vector3.Dot(targetDiff.normalized, portTransform.forward);

                if (targetDistanceSqr < moduleData.MaxLockDistanceSqr && targetDirection > moduleData.MaxLockRange)
                {
                    if (isLocalPlayer)
                        HeadUpDisplayManager.Instance.MarkTarget(targetRadarLockOn, targetDistanceSqr);
                }
                else
                {
                    nextLockTime = Time.time + kocmomech.lockTime;
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
            int countFoe = listFoe.Length;
            for (int i = 0; i < countFoe; i++)
            {
                if (i < countFoe && listFoe[i])
                {
                    scanDiff = listFoe[i].position - myPosition;
                    scanDistanceSqr = Vector3.SqrMagnitude(scanDiff);
                    scanDirection = Vector3.Dot(scanDiff.normalized, portTransform.forward);
                    if (scanDistanceSqr <= moduleData.MaxSearchRadiusSqr && scanDirection >= moduleData.MaxSearchRange)
                    {
                        if (isLocalPlayer)
                            SatelliteCommander.Instance.IdentifyTarget(listFoe[i]); // 標記搜索範圍的所有敵機
                        else
                        {
                            if (countOnboard < 10)
                            {
                                targetOnboard[countOnboard] = listFoe[i];
                                countOnboard++;
                            }
                        }

                        if (scanDistanceSqr <= moduleData.MaxLockDistanceSqr && scanDirection >= moduleData.MaxLockRange)
                        {
                            if (isLocalPlayer)
                                SatelliteCommander.Instance.FireControlLookTarget(listFoe[i]);  // 標記鎖定範圍的所有敵機

                            // 尋找最接近的目標
                            if (scanDirection > targetNearestDirection)
                            {
                                targetNearest = listFoe[i];
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
            int countFoe = SatelliteCommander.factionData[indexFaction].arrayFoe.Length;
            for (int i = 0; i < countFoe; i++)
            {
                if (i < countFoe && SatelliteCommander.factionData[indexFaction].arrayFoe[i])
                {
                    scanDiff = SatelliteCommander.factionData[indexFaction].arrayFoe[i].position - myPosition;
                    scanDistanceSqr = Vector3.SqrMagnitude(scanDiff);
                    scanDirection = Vector3.Dot(scanDiff.normalized, portTransform.forward);
                    if (scanDistanceSqr <= moduleData.MaxSearchRadiusSqr && scanDirection >= moduleData.MaxSearchRange)
                    {
                        if (isLocalPlayer)
                            SatelliteCommander.Instance.IdentifyTarget(SatelliteCommander.factionData[indexFaction].arrayFoe[i]); // 標記搜索範圍的所有敵機
                        else
                        {
                            if (countOnboard < 10)
                            {
                                targetOnboard[countOnboard] = SatelliteCommander.factionData[indexFaction].arrayFoe[i];
                                countOnboard++;
                            }
                        }

                        if (scanDistanceSqr <= moduleData.MaxLockDistanceSqr && scanDirection >= moduleData.MaxLockRange)
                        {
                            if (isLocalPlayer)
                                SatelliteCommander.Instance.FireControlLookTarget(SatelliteCommander.factionData[indexFaction].arrayFoe[i]);  // 標記鎖定範圍的所有敵機

                            // 尋找最接近的目標
                            if (scanDirection > targetNearestDirection)
                            {
                                targetNearest = SatelliteCommander.factionData[indexFaction].arrayFoe[i];
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
                    AvionicsSystem target = targetRadarLockOn.GetComponentInChildren<AvionicsSystem>();
                    if (target.controlUnit == ControlUnit.LocalPlayer || target.controlUnit == ControlUnit.RemotePlayer)
                    {
                        //Debug.Log(portTransform.name+"/"+myPhotonView.ViewID + "/" + kocmoNumber + "/" + Time.frameCount + "/True/");
                        portPhotonView.RPC("RadarLockOn", RpcTarget.AllViaServer, target.kocmoNumber, kocmoNumber, true);
                    }
                }
                if (targetLocked)
                {
                    AvionicsSystem target = targetLocked.GetComponentInChildren<AvionicsSystem>();
                    if (target.controlUnit == ControlUnit.LocalPlayer || target.controlUnit == ControlUnit.RemotePlayer)
                    {
                        //Debug.Log(portTransform.name + "/" + myPhotonView.ViewID + "/" + kocmoNumber + "/" + Time.frameCount + "/false/");
                        portPhotonView.RPC("RadarLockOn", RpcTarget.AllViaServer, target.kocmoNumber, kocmoNumber, false);
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