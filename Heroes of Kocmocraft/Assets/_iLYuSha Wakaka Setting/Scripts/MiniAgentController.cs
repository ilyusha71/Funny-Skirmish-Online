using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace Kocmoca
{
    public class MiniAgentController : MonoBehaviour
    {
        public ObjectPoolData[] listAmmoOPD;
        private Transform myTransform;
        private Rigidbody myRigidbody;
        private void Awake()
        {
            myTransform = transform;
            myRigidbody = GetComponent<Rigidbody>();
            myTransform.position = new Vector3(Random.Range(-2000, 2000), Random.Range(-2000, 2000), Random.Range(50, 300));
            SPD = Random.Range(90, 130);

        }
        // Start is called before the first frame update
        void Start()
        {
            listAmmoOPD = ResourceManager.instance.listAmmoOPD;
            InvokeRepeating("RandomLock", 10.0f, Random.Range(3, 10));
            float delay = Random.Range(10.0f, 20.0f);
            float round = Random.Range(3.0f, 7.0f);
            InvokeRepeating("Shoot", delay, round);
            InvokeRepeating("Shoot", delay + 0.3f, round);
            InvokeRepeating("Shoot", delay + 0.7f, round);
            InvokeRepeating("Shoot", delay + 1.1f, round);
            InvokeRepeating("Shoot", delay + 1.6f, round);

        }
        Transform target;
        void RandomLock()
        {
            //PositionTarget = 
            foreach (object value in RandomValues(SatelliteCommander.Instance.listKocmocraft).Take(Random.Range(5, 10)))
            {
                 target = (Transform)value;
            }
        }

        void Shoot()
        {
            GameObject ammo = listAmmoOPD[0].Reuse(myTransform.position, myTransform.rotation);
            ammo.GetComponent<Ammo>().InputAmmoData(LocalPlayerRealtimeData.Number, 0, GetComponent<Rigidbody>().velocity, 0);
        }

        public IEnumerable<TValue> RandomValues<TKey, TValue>(IDictionary<TKey, TValue> dict)
        {
           System.Random rand = new System.Random();
            List<TValue> values = Enumerable.ToList(dict.Values);
            int size = dict.Count;
            while (true)
            {
                yield return values[rand.Next(size)];
            }
        }


        float SPD = 50;
        public float RotationSpeed = 50.0f;// Turn Speed
        public float SpeedPitch = 2;// rotation X
        public float SpeedRoll = 3;// rotation Z
        public float SpeedYaw = 1;// rotation Y
        public float DampingTarget = 10.0f;// rotation speed to facing to a target

        [HideInInspector]
        public Vector3 PositionTarget = Vector3.zero;// current target position
        [HideInInspector]
        private Vector3 positionTarget = Vector3.zero;
        public Quaternion mainRot = Quaternion.identity;


        void FixedUpdate()
        {
            if(target)
                PositionTarget = target.position;

            Vector3 velocityTarget = Vector3.zero;

            if (myRigidbody.angularVelocity.magnitude > 3)
                myRigidbody.Sleep();

            // rotation facing to the positionTarget
            positionTarget = Vector3.Lerp(positionTarget, PositionTarget, Time.fixedDeltaTime * DampingTarget);
            Vector3 relativePoint = this.transform.InverseTransformPoint(positionTarget).normalized;
            mainRot = Quaternion.LookRotation(positionTarget - this.transform.position);
            myRigidbody.rotation = Quaternion.Lerp(myRigidbody.rotation, mainRot, Time.fixedDeltaTime * (RotationSpeed * 0.005f) * SpeedYaw);
            myRigidbody.rotation *= Quaternion.Euler(-relativePoint.y * SpeedPitch * 0.3f, 0, -relativePoint.x * SpeedRoll * 0.5f);

            velocityTarget = (myRigidbody.rotation * Vector3.forward) * (SPD);
            myRigidbody.velocity = velocityTarget;

            //yaw = Mathf.Lerp(yaw, 0, Time.deltaTime);
        }
    }

}
