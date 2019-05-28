/***************************************************************************
 * Onboard Radar
 * 機載雷達
 * Last Updated: 2019/05/27
 * 
 * v19.0527
 * 1. 计算相对位置，使用InverseTransformPoint比使用Vector3相减，效能提升137%
 * 2. 计算Vector3.Dot，使用Vector3.forward比使用myTransform.forward，效能提升127%
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
        public bool active;
        [Header("Preset")]
        public Radar radar;
        public Turret turret;
        public Kocmomech kocmomech;
        public Transform myTransform;



        [Header("Dependent Components")]
        private PhotonView myPhotonView;
        [Header("Kocmonaut Data")]
        private bool isLocalPlayer;
        public int kocmonautNumber;
        [Header("Modular Parameter")]
        public Transform[] FriendAircraftsArray;


        public Transform[] listFriend;
        public Transform[] listFoe;
        private int index; // for loop index
        private Vector3 relativePoint;
        private float distanceSqr;
        private float angle;

        [Tooltip("距离最近的目标")]
        public Transform nearestTarget;
        private float nearestDistanceSqr;
        [Tooltip("自动瞄准的目标")]
        public Transform autoAimTarget;
        private float minAngle;




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

        private void Reset()
        {
            int type = int.Parse(name.Split(new char[2] { '(', ')' })[1]);
            KocmocraftDatabase index = UnityEditor.AssetDatabase.LoadAssetAtPath<KocmocraftDatabase>("Assets/_iLYuSha Wakaka Setting/ScriptableObject/Kocmocraft Database.asset");
            radar = index.kocmocraft[type].radar;
            turret = index.kocmocraft[type].turret;
            kocmomech = index.kocmocraft[type].kocmomech;
            myTransform = transform;
            enabled = false;
        }

        public void Initialize(Core core, int faction, int type, int number)
        {
            enabled = true;
            // Dependent Components
            myPhotonView = transform.root. GetComponent<PhotonView>();
            // Modular Parameter
            isLocalPlayer = core == Core.LocalPlayer ? true : false;
            kocmonautNumber = number;
            //listFriendAircrafts = KocmocaData.factionData[faction].listFriend;
            //listFoeAircrafts = KocmocaData.factionData[faction].listFoe;
            FriendAircraftsArray = SatelliteCommander.factionData[faction].arrayFriend;
            listFoe = SatelliteCommander.factionData[faction].arrayFoe;
            this.faction = faction;

            targetOnboard = new Transform[maxtOnboardCount];

            moduleData = KocmocaData.KocmocraftData[type];


            active = true;
        }
        private void Update()
        {
            //if (!active) return;

            //myPosition = myTransform.position;
            //if (isLocalPlayer) SearchFriend();
            //SearchFoe();
            //RadarWarningEmitter();
            UpdateDefault();
        }





        public void Search()
        {
            myPosition = myTransform.position;
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
                        relativePoint = myTransform.InverseTransformPoint(listFoe[index].position);
                        distanceSqr = Vector3.SqrMagnitude(relativePoint);
                        angle = Vector3.Dot(relativePoint.normalized, Vector3.forward);

                        if (distanceSqr <= radar.radiusSqr && angle >= radar.range)
                            HeadUpDisplayManager.Instance.NewIdentifyFriend(listFriend[index]); // 標記友機
                    }
                }

                if (listFoe[index])
                {
                    relativePoint = myTransform.InverseTransformPoint(listFoe[index].position);
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
                    scanDirection = Vector3.Dot(scanDiff.normalized, myTransform.forward);
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
                    KocmocraftManager target = targetRadarLockOn.GetComponentInChildren<KocmocraftManager>();
                    if (target.Core == Core.LocalPlayer || target.Core == Core.RemotePlayer)
                    {
                        //Debug.Log(myTransform.name+"/"+myPhotonView.ViewID + "/" + kocmonautNumber + "/" + Time.frameCount + "/True/");
                        myPhotonView.RPC("RadarLockOn", RpcTarget.AllViaServer, target.Number, kocmonautNumber, true);
                    }
                }
                if (targetLocked)
                {
                    KocmocraftManager target = targetLocked.GetComponentInChildren<KocmocraftManager>();
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