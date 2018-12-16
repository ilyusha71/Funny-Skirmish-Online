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

namespace Kocmoca
{
    public class ObserverCamera : MonoBehaviour
    {
        private Transform myTransform;
        private CameraSway sway;
        private bool isCockpitView;
        private bool isLocalView;
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
        public Transform Target;// player ( Your Plane)
        public Transform targetViewpoint; // 視角位置
        public GameObject targetKocmonaut; // 宇航員物件

        private void Awake()
        {
            myTransform = transform;            
        }
        private void Start()
        {
            Controller.controlMode = ControlMode.Flying;
        }
        public void InitializeView(Transform cockpitViewpoint, Transform cockpitKocmonaut, int kocmonautNumber)
        {
            isLocalView = true;
            isCockpitView = true;
            targetViewpoint = cockpitViewpoint;
            targetKocmonaut = cockpitKocmonaut.gameObject;
            sway = GetComponent<CameraSway>();
            playerFaction = SatelliteCommander.Instance.listKocmonaut[kocmonautNumber].Faction;
            HeadUpDisplayManager.Instance.ShowObserver(playerFaction, targetViewpoint.root.name);

            targetKocmonaut.SetActive(false);
            Target = null;
            myTransform.SetParent(targetViewpoint.parent);
            myTransform.localPosition = targetViewpoint.localPosition;
            myTransform.localRotation = targetViewpoint.localRotation;
            sway.enabled = true;
        }
        public void TransferCamera()
        {
            myTransform.SetParent(null);
            sway.enabled = false;
            StartCoroutine(Delay());
        }
        IEnumerator Delay()
        {
            yield return new WaitForSeconds(1.73f);
            isLocalView = false;
            NextBotViewpoint();
        }
        public void NextBotViewpoint()
        {
            int lastTarget = observerNumber;
            do
            {
                indexOtherViewpoint++;
                indexOtherViewpoint = (int)Mathf.Repeat(indexOtherViewpoint, listOthers.Count);
                observerNumber = listOthers[indexOtherViewpoint];
            }
            while (observerNumber == 0 || observerNumber == lastTarget);
            Target = SatelliteCommander.Instance.listKocmocraft[observerNumber].Find("Cockpit Viewpoint");
            Kocmonaut observer = SatelliteCommander.Instance.listKocmonaut[observerNumber];
            HeadUpDisplayManager.Instance.ShowObserver(observer.Faction, Target.root.name);
            Offset = KocmocraftData.GetCameraOffset(observer.Type);
        }
        public void PreviousBotViewpoint()
        {
            int lastTarget = observerNumber;
            do
            {
                indexOtherViewpoint--;
                indexOtherViewpoint = (int)Mathf.Repeat(indexOtherViewpoint, listOthers.Count);
                observerNumber = listOthers[indexOtherViewpoint];
            }
            while (observerNumber == 0 || observerNumber == lastTarget);
            Target = SatelliteCommander.Instance.listKocmocraft[observerNumber].Find("Cockpit Viewpoint");
            Kocmonaut observer = SatelliteCommander.Instance.listKocmonaut[observerNumber];
            HeadUpDisplayManager.Instance.ShowObserver(observer.Faction, Target.root.name);
            Offset = KocmocraftData.GetCameraOffset(observer.Type);
        }
        public void SwitchView()
        {
            isCockpitView = !isCockpitView;

            if (isCockpitView && targetViewpoint)
            {
                targetKocmonaut.SetActive(false);
                Target = null;
                myTransform.SetParent(targetViewpoint.parent);
                myTransform.localPosition = targetViewpoint.localPosition;
                myTransform.localRotation = targetViewpoint.localRotation;
                sway.enabled = true;
            }
            else
            {
                targetKocmonaut.SetActive(true);
                Target = targetViewpoint;
                myTransform.SetParent(null);
                sway.enabled = false;
                Offset = KocmocraftData.GetCameraOffset(Target.root.GetComponent<KocmocraftManager>().Type);
            }
        }

        private void Update()
        {
            if (!isLocalView)
            {
                if (Input.GetKeyDown(KeyCode.D))
                    NextBotViewpoint();
                else if (Input.GetKeyDown(KeyCode.A))
                    PreviousBotViewpoint();
            }
        }
        void FixedUpdate()
        {
            if (!Target)
                return;

            myTransform.rotation = Target.rotation;
            positionTargetUp = Vector3.Lerp(positionTargetUp, ((-Target.forward * Offset.z) + (Target.up * Offset.y)), Time.fixedDeltaTime * TurnSpeedMult);
            positionTarget = Target.position + (positionTargetUp);
            float distance = Vector3.Distance(positionTarget, myTransform.position);
            myTransform.position = Vector3.Lerp(myTransform.position, positionTarget, Time.fixedDeltaTime * (distance * FollowSpeedMult));
        }
    }
}