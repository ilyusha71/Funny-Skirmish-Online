using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kocmoca
{
    public class BigShip : MonoBehaviour
    {
        Transform myTransform;
        private ObjectPoolData listAmmoBS;

        private void Awake()
        {
            myTransform = transform;
        }
        // Start is called before the first frame update
        void Start()
        {
            listAmmoBS = ResourceManager.instance.listAmmoBS;
            InvokeRepeating("Shoot", Random.Range(1.0f, 3.0f), Random.Range(1.0f, 3.0f));
        }

        void Shoot()
        {
            GameObject ammo = listAmmoBS.Reuse(myTransform.position, myTransform.rotation);
            ammo.GetComponent<Ammo>().InputAmmoData(-999, 0, Vector3.zero, 0);
        }

    }

}
