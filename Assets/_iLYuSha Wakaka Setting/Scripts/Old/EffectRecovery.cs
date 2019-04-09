using System.Collections;
using System.Collections.Generic;
using UnityEngine;
namespace AirSupremacy

{
    public class EffectRecovery : ObjectRecycleSystem
    {
        public float maxLifeTime = 0.5f;
        //float timeRecovery;

        ////public void Open(ObjectPoolData objPoolData, float lifeTime)
        ////{
        ////    restart = true;
        ////    this.objPoolData = objPoolData;
        ////    timer = Time.time + lifeTime;
        ////}

        //void OnEnable()
        //{
        //    timeRecovery = Time.time + maxLifeTime;
        //}

        //void Update()
        //{
        //    if (Time.time > timeRecovery)
        //        Recycle(gameObject);
        //}

        //public void Recovery()
        //{
        //    restart = false;
        //    //objPoolData.Recovery(gameObject);
        //}

        //private void OnDestroy()
        //{
        //    //Recovery();
        //    Debug.Log(transform.name);
        //}
    }

}
