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
    }

    public class LocalBotController : MonoBehaviour
    {
        public EnergyCore[] beacons;
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
        public float targetTrackingDistance;
        public float targetTrackingDirection;

        void Start()
        {
            beacons = FindObjectsOfType<EnergyCore>();
            // Dependent Components
            myTransform = transform;
            myRigidbody = GetComponent<Rigidbody>();
            myAvionicsSystem = GetComponent<AvionicsSystem>();
            myAvionicsSystem.AutoPilot = true;// set auto pilot to true will make this plane flying and looking to Target automatically
            myAvionicsSystem.FollowTarget = true;
            myOnboardRadar = GetComponent<OnboardRadar>();
            myLaserFCS = GetComponentsInChildren<FireControlSystem>()[0];
            myRocketFCS = GetComponentsInChildren<FireControlSystem>()[1];
            myMissileFCS = GetComponentsInChildren<FireControlSystem>()[2];
            // Modular Parameter
            coordinateSpawn = myTransform.position;
            coordinateDestination = beacons[Random.Range(0, beacons.Length)].transform.position;
            timestatetemp = 0;
        }

        private float dot;

        void FlyToNearestBeacon()
        {
            int nearestBeacon = -1;
            float nearestDistance = int.MaxValue;
            for (int i = 0; i < beacons.Length; i++)
            {
                float distance = Vector3.Distance(myTransform.position, beacons[i].transform.position);
                if (distance < nearestDistance)
                {
                    nearestBeacon = i;
                    nearestDistance = distance;
                }
            }
            coordinateDestination = beacons[nearestBeacon].transform.position;
            myAvionicsSystem.PositionTarget = coordinateDestination;
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
            myAvionicsSystem.PositionTarget = coordinateDestination;
        }

        void Update()
        {
            if (!myAvionicsSystem)
                return;

            distanceDestination = Vector3.Distance(coordinateDestination, myTransform.position);
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
            myAvionicsSystem.SpeedControl(1.0f, true);
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
                myAvionicsSystem.SpeedControl(1.0f, true);
            }
            Stabilizer();
        }
        void Attack()
        {
            if (myTransform.position.y > limitElevation || Time.time > timestatetemp+9)
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
            if (targetRadarLockOn)
            {
                if (myMissileFCS.countAmmo > 0)
                {
                    myMissileFCS.target = targetRadarLockOn;
                    myMissileFCS.Shoot();
                }

                float distanceTarget = Vector3.Distance(myTransform.position, targetRadarLockOn.position);
                if (distanceTarget < 300)
                {
                    if (myRocketFCS.countAmmo > 0)
                    {
                        myRocketFCS.target = targetRadarLockOn;
                        myRocketFCS.Shoot();
                    }
                }
            }
            if (targetTrack)
            {
                Vector3 targetTrackingPosition = targetTrack.position;
                myAvionicsSystem.PositionTarget = targetTrackingPosition;
                targetTrackingDistance = Vector3.Distance(myTransform.position, targetTrackingPosition);
                targetTrackingDirection = Vector3.Dot((targetTrackingPosition - myTransform.position).normalized, myTransform.forward);

                if (targetTrackingDirection < myOnboardRadar.maxLockAngle)
                {
                    IntoPatrolState();
                    return;
                }
                else
                {
                    if (targetTrackingDistance < myOnboardRadar.maxLockRadius)
                    {
                        if (targetTrackingDistance < 71)
                        {
                            IntoLeaveState();
                            return;
                        }
                        myAvionicsSystem.SpeedControl(0.5f, true);
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
            myAvionicsSystem.SpeedControl(1.0f, true);
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
