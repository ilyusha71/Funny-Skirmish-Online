/***************************************************************************
 * Observer Camera
 * 觀察者攝影機
 * Last Updated: 2018/09/22
 * Description:
 * 1. Damage Manager -> Flight View -> Observer Camera
 * 2. 宇航機視角切換
 ***************************************************************************/
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Cinemachine;

namespace Kocmoca
{
    public class ObserverCamera : CameraTrackingSystem
    {
        [Header("Observer")]
        public Transform Target;// player ( Your Plane)
        public Transform targetViewpoint; // 視角位置
        //public GameObject targetPilot; // 飛行員層級

        private CameraSway sway;
        private bool isCockpitView = false;
        //private bool isLocalView = false;
        private float FollowSpeedMult = 0.5f; // camera following speed 
        private float TurnSpeedMult = 5; // camera turning speed 
        [SerializeField]
        private Vector3 Offset;
        private Vector3 positionTargetUp;
        private Vector3 positionTarget;

        private Faction playerFaction;

        //public List<Transform> listOthersViewpoint = new List<Transform>();
        public List<int> listOthers = new List<int>();
        public int observerNumber;
        public int indexOtherViewpoint;

        public CinemachineVirtualCamera cmCockpit, cmFollow;
        public CinemachineFreeLook[] observerView;

        public void AssignPilotView(CinemachineVirtualCamera cockpitView, CinemachineVirtualCamera followView)
        {
            cmCockpit = cockpitView;
            cmCockpit.enabled = true;
            cmFollow = followView;
        }
        /// <summary>
        /// Update is called every frame, if the MonoBehaviour is enabled.
        /// </summary>
        void Update()
        {
            if (Input.GetKeyDown(KeyCode.C))
            {
                cmCockpit.enabled = !cmCockpit.isActiveAndEnabled;
                cmFollow.enabled = !cmFollow.isActiveAndEnabled;
            }
        }
















        //private void Awake()
        //{
        //    InitializeTrackingSystem();
        //}
        //private void Start()
        //{
        //    Controller.controlMode = ControlMode.Flying;
        //}
        //public void InitializeView(Transform cockpitViewpoint, int kocmonautNumber)
        //{
        //    //isLocalView = true;
        //    isCockpitView = true;
        //    targetViewpoint = cockpitViewpoint;
        //    //targetPilot = pilot;
        //    sway = GetComponent<CameraSway>();
        //    playerFaction = SatelliteCommander.Instance.listKocmonaut[kocmonautNumber].Faction;
        //    HeadUpDisplayManager.Instance.ShowObserver(playerFaction, targetViewpoint.root.name);

        //    //pilot.SetActive(false);
        //    Target = null;
        //    pivot.SetParent(targetViewpoint.parent);
        //    pivot.localPosition = targetViewpoint.localPosition;
        //    pivot.localRotation = targetViewpoint.localRotation;
        //    ReturnToZero();
        //    sway.enabled = true;
        //}

        //// 本地玩家坠机后转移摄影机
        //public void TransferCamera()
        //{
        //    pivot.SetParent(null);
        //    sway.enabled = false;
        //    ReturnToZero();
        //    StartCoroutine(Delay());
        //}
        //IEnumerator Delay()
        //{
        //    yield return new WaitForSeconds(1.73f);
        //    LocalPlayerRealtimeData.Status = FlyingStatus.Respawn;
        //    //isLocalView = false;
        //    NextBotViewpoint();
        //}
        //public void NextBotViewpoint()
        //{
        //    int lastTarget = observerNumber;
        //    do
        //    {
        //        indexOtherViewpoint++;
        //        indexOtherViewpoint = (int)Mathf.Repeat(indexOtherViewpoint, listOthers.Count);
        //        observerNumber = listOthers[indexOtherViewpoint];
        //    }
        //    while (observerNumber == 0 || observerNumber == lastTarget);
        //    Target = SatelliteCommander.Instance.listKocmocraft[observerNumber].Find("Cockpit Viewpoint");
        //    Kocmonaut observer = SatelliteCommander.Instance.listKocmonaut[observerNumber];
        //    HeadUpDisplayManager.Instance.ShowObserver(observer.Faction, Target.root.name);
        //    Offset = KocmocraftData.GetCameraOffset(observer.Type);
        //}
        //public void PreviousBotViewpoint()
        //{
        //    int lastTarget = observerNumber;
        //    do
        //    {
        //        indexOtherViewpoint--;
        //        indexOtherViewpoint = (int)Mathf.Repeat(indexOtherViewpoint, listOthers.Count);
        //        observerNumber = listOthers[indexOtherViewpoint];
        //    }
        //    while (observerNumber == 0 || observerNumber == lastTarget);
        //    Target = SatelliteCommander.Instance.listKocmocraft[observerNumber].Find("Cockpit Viewpoint");
        //    Kocmonaut observer = SatelliteCommander.Instance.listKocmonaut[observerNumber];
        //    HeadUpDisplayManager.Instance.ShowObserver(observer.Faction, Target.root.name);
        //    Offset = KocmocraftData.GetCameraOffset(observer.Type);
        //}
        //public void SwitchView()
        //{
        //    isCockpitView = !isCockpitView;

        //    if (isCockpitView && targetViewpoint)
        //    {
        //        //targetPilot.SetActive(false);
        //        Target = null;
        //        pivot.SetParent(targetViewpoint.parent);
        //        pivot.localPosition = targetViewpoint.localPosition;
        //        pivot.localRotation = targetViewpoint.localRotation;
        //        sway.enabled = true;
        //    }
        //    else
        //    {
        //        //targetPilot.SetActive(true);
        //        Target = targetViewpoint;
        //        pivot.SetParent(null);
        //        sway.enabled = false;
        //        ReturnToZero();
        //        //Offset = KocmocraftData.GetCameraOffset(Target.root.GetComponent<Kocmoport>().m_Type);
        //    }
        //}

        //private void Update()
        //{
        //    if (LocalPlayerRealtimeData.Status == FlyingStatus.Respawn)
        //    {
        //        if (Input.GetKeyDown(KeyCode.D))
        //            NextBotViewpoint();
        //        else if (Input.GetKeyDown(KeyCode.A))
        //            PreviousBotViewpoint();
        //    }
        //}

        //void FixedUpdate()
        //{
        //    if (!Target)
        //        return;

        //    if (LocalPlayerRealtimeData.Status == FlyingStatus.Flying)
        //    {
        //        pivot.rotation = Target.rotation;
        //        positionTargetUp = Vector3.Lerp(positionTargetUp, ((-Target.forward * Offset.z) + (Target.up * Offset.y)), Time.fixedDeltaTime * TurnSpeedMult);
        //        positionTarget = Target.position + (positionTargetUp);
        //        float distance = Vector3.Distance(positionTarget, pivot.position);
        //        pivot.position = Vector3.Lerp(pivot.position, positionTarget, Time.fixedDeltaTime * (distance * FollowSpeedMult));
        //    }
        //    else if (LocalPlayerRealtimeData.Status == FlyingStatus.Respawn)
        //    {
        //        pivot.position = Target.position;
        //        pivot.rotation = Target.rotation;
        //        Control();
        //    }
        //}
    }
}