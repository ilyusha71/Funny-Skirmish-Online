using Photon.Pun;
using UnityEngine;
using System.Collections;

namespace Kocmoca
{
    public enum AIState
    {
        Idle,
        Patrol,
        Attack,
        Leave,

        WP,
        Observe,

        Crossfire,
    }
    public enum Mission
    {
        Default,
        Snipper,
        Battle,
    }

    public class LocalBotController : MonoBehaviour
    {
        public static Vector3 targetPos;
        public EnergyCore[] beacons;
        private int countBeacon;
        // Dependent Components
        private Transform myTransform;
        private Rigidbody myRigidbody;
        private AvionicsSystem myAvionicsSystem;
        private OnboardRadar myOnboardRadar;
        private FireControlSystem myLaserFCS;
        private FireControlSystem myRocketFCS;
        private FireControlSystem myMissileFCS;
        [Header("Kocmonaut Data")]
        public int kocmonautNumber;
        [Header("Modular Parameter")]
        public AIState AIstate = AIState.Idle;// AI state
        public Vector3 coordinateSpawn;
        public Vector3 coordinateDestination;
        public float distanceDestination; // 目的地距離
        public float radiusBattle = 700;// limited distance between (BattlePosition and AI position) , this is will create a circle battle area and AI cannot go far out of this area.
        public float limitElevation = 2000;
        [HideInInspector]
        //   public string[] TargetTag;// list of Tags that AI will attack to
        [Header("Target Locked")]
        //public Transform targetLocked;// current target object.
        private int targetNumber;
        [Header("Target Onboard")]
        public Transform targetTrack;
        public Transform targetAutoAim;
        public Transform targetRadarLockOn;
        public float targetRadarDistance;

        [HideInInspector]
        public int WeaponSelected = 0;
        // public int AttackRate = 80; // (0 - 100) e.g. if 100 this AI will always shooting.

        private float timestatetemp;
        public bool attacking;
        private Vector3 targetpositionTemp;

        public float targetFireDistance;
        public Vector3 targetTrackingDiff;
        public float targetTrackingDistanceSqr;
        public float targetTrackingDirection;

        [Header("Mission")]
        public Mission mission;
        public Transform nearestBeacon;
        public Vector3 observationPost;

        void Start()
        {
            mission =  Mission.Default;
            beacons = FindObjectsOfType<EnergyCore>();
            countBeacon = beacons.Length;
            // Dependent Components
            myTransform = transform;
            myRigidbody = GetComponent<Rigidbody>();
            myAvionicsSystem = GetComponentInChildren<AvionicsSystem>();
            //myAvionicsSystem.AutoPilot = true;// set auto pilot to true will make this plane flying and looking to Target automatically
            //myAvionicsSystem.FollowTarget = true;
            myOnboardRadar = GetComponentInChildren<OnboardRadar>();
            myLaserFCS = GetComponentsInChildren<FireControlSystem>()[0];
            myRocketFCS = GetComponentsInChildren<FireControlSystem>()[1];
            myMissileFCS = GetComponentsInChildren<FireControlSystem>()[2];
            // Modular Parameter
            coordinateSpawn = myTransform.position;
            coordinateDestination = beacons[Random.Range(0, beacons.Length)].transform.position;
            timestatetemp = 0;




            // 尋找最近的Beacon
            int nearest = -1;
            float distanceNearest = int.MaxValue;
            for (int i = 0; i < countBeacon; i++)
            {
                float distanceSqr = Vector3.SqrMagnitude(beacons[i].transform.position - myTransform.position);
                if (distanceSqr < distanceNearest)
                {
                    nearest = i;
                    distanceNearest = distanceSqr;
                }
            }
            nearestBeacon = beacons[nearest].transform;

            //switch (mission)
            //{
            //    case Mission.Snipper:
            //        AIstate = AIState.WP;
            //        observationPost = Vector3.Lerp(nearestBeacon.position, myTransform.position, 1370.0f / distanceNearest);
            //        targetPos = observationPost;
            //        break;
            //    case Mission.Battle:
            //        AIstate = AIState.Crossfire;
            //        myAvionicsSystem.ControlThrottle(1.0f);
            //        break;
            //    default:break;
            //}
        }

        private float dot;

        void FlyToNearestBeacon()
        {
            int indexNearest = -1;
            float nearestDistance = int.MaxValue;
            for (int i = 0; i < beacons.Length; i++)
            {
                float distance = Vector3.Distance(myTransform.position, beacons[i].transform.position);
                if (distance < nearestDistance)
                {
                    indexNearest = i;
                    nearestDistance = distance;
                }
            }
            coordinateDestination = beacons[indexNearest].transform.position;
            targetPos = coordinateDestination;
        }

        void FlyToFarthestBeacon()
        {
            int farthestBeacon = -1;
            float farthestDistance = 0;
            for (int i = 0; i < beacons.Length; i++)
            {
                float distance = Vector3.Distance(myTransform.position, beacons[i].transform.position);
                if (distance > farthestDistance)
                {
                    farthestBeacon = i;
                    farthestDistance = distance;
                }
            }
            coordinateDestination = beacons[farthestBeacon].transform.position;
            targetPos = coordinateDestination;
        }

        void Update()
        {
            if (!myAvionicsSystem)
                return;

            distanceDestination = Vector3.Distance(coordinateDestination, myTransform.position);
            if (mission == Mission.Default)
            {
                switch (AIstate)
                {
                    case AIState.Idle:
                        Idle(); break;
                    case AIState.Patrol:
                        Patrol(); break;
                    case AIState.Attack:
                        Attack(); break;
                    case AIState.Leave:
                        Leave(); break;
                }
            }
            else
            {
                switch (AIstate)
                {
                    case AIState.WP:
                        GotoObservationPost(); break;
                    case AIState.Observe:
                        Observe(); break;

                    case AIState.Crossfire:
                        Crossfire(); break;
                }
            }
        }

        void Crossfire()
        {
            myAvionicsSystem.ControlThrottle(1.0f);

            targetTrack = myOnboardRadar.targetTrack;
            if (targetTrack)
                targetPos = targetTrack.position;

            targetAutoAim = myOnboardRadar.targetAutoAim;
            if (targetAutoAim)
                myLaserFCS.Shoot();
        }


        void GotoObservationPost()
        {
            myAvionicsSystem.ControlThrottle(1.0f);
            targetTrack = myOnboardRadar.targetTrack;
            if (targetTrack)
                AIstate = AIState.Observe;
        }

        void Observe()
        {
            targetTrack = myOnboardRadar.targetTrack;
            if (targetTrack)
                targetPos = targetTrack.position;
            else
                AIstate = AIState.WP;

            myAvionicsSystem.ControlThrottle(1.0f);
            targetAutoAim = myOnboardRadar.targetAutoAim;
            if (targetAutoAim)
                myLaserFCS.Shoot();
        }


        void Idle()
        {
            if (myTransform.position.y > limitElevation)
            {
                IntoLeaveState();
                return;
            }
            if (distanceDestination < radiusBattle)
            {
                IntoPatrolState();
                return;
            }
            FlyToNearestBeacon();
            myAvionicsSystem.ControlThrottle(1.0f);
            targetTrack = null;
            Stabilizer();
        }
        void Patrol()
        {
            if (myTransform.position.y > limitElevation)
            {
                IntoLeaveState();
                return;
            }
            if (distanceDestination > radiusBattle)
            {
                IntoIdleState();
                return;
            }

            targetTrack = myOnboardRadar.targetTrack;
            if (targetTrack)
            {
                IntoAttackState();
                return;
            }
            else
            {
                FlyToNearestBeacon();
                myAvionicsSystem.ControlThrottle(1.0f);
            }
            Stabilizer();
        }
        void Attack()
        {
            if (myTransform.position.y > limitElevation || Time.time > timestatetemp + 60)
            {
                IntoLeaveState();
                return;
            }
            if (distanceDestination > radiusBattle)
            {
                IntoIdleState();
                return;
            }

            targetAutoAim = myOnboardRadar.targetAutoAim;
            targetRadarLockOn = myOnboardRadar.targetRadarLockOn;

            if (targetAutoAim)
                myLaserFCS.Shoot();
            //if (targetRadarLockOn)
            //{
            //    if (myMissileFCS.countAmmo > 0)
            //    {
            //        myMissileFCS.target = targetRadarLockOn;
            //        myMissileFCS.Shoot();
            //    }

            //    float distanceTarget = Vector3.Distance(myTransform.position, targetRadarLockOn.position);
            //    if (distanceTarget < 300)
            //    {
            //        if (myRocketFCS.countAmmo > 0)
            //        {
            //            myRocketFCS.target = targetRadarLockOn;
            //            myRocketFCS.Shoot();
            //        }
            //    }
            //}
            if (targetTrack)
            {
                Vector3 targetTrackingPosition = targetTrack.position;
                targetPos = targetTrackingPosition;
                targetTrackingDiff = targetTrackingPosition - myTransform.position;
                targetTrackingDistanceSqr = Vector3.SqrMagnitude(targetTrackingDiff);
                targetTrackingDirection = Vector3.Dot(targetTrackingDiff.normalized, myTransform.forward);

                if (targetTrackingDirection < RadarParameter.maxLockAngle)
                {
                    IntoPatrolState();
                    return;
                }
                else
                {
                    if (targetTrackingDistanceSqr < RadarParameter.maxLockDistanceSqr)
                    {
                        if (targetTrackingDistanceSqr < 5000)
                        {
                            IntoLeaveState();
                            return;
                        }
                        myAvionicsSystem.ControlThrottle(0.0f);
                    }
                }
            }
            else
            {
                IntoPatrolState();
                return;
            }
            Stabilizer();
        }
        void Leave()
        {
            FlyToFarthestBeacon();
            myAvionicsSystem.ControlThrottle(1.0f);
            if (myTransform.position.y < limitElevation)
            {
                if (distanceDestination > radiusBattle || Time.time > timestatetemp + 3)
                {
                    IntoIdleState();
                    return;
                }
            }
            Stabilizer();
        }

        public void IntoIdleState()
        {
            timestatetemp = Time.time;
            AIstate = AIState.Idle;
        }
        public void IntoPatrolState()
        {
            timestatetemp = Time.time;
            AIstate = AIState.Patrol;
        }
        public void IntoAttackState()
        {
            timestatetemp = Time.time;
            AIstate = AIState.Attack;
        }
        public void IntoLeaveState()
        {
            timestatetemp = Time.time;
            AIstate = AIState.Leave;
        }
        void Stabilizer()
        {
            myRigidbody.isKinematic = false;
            if (Time.time > timestatetemp + 15)
            {
                myRigidbody.isKinematic = true;
                timestatetemp = Time.time;
            }
        }
    }
}