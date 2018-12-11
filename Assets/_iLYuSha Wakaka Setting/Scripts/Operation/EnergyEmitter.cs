using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Kocmoca
{
    public class EnergyEmitter: MonoBehaviour
    {
        private EnergyCore core;
        public int limitRadius;
        private int tolerance = 37;
        private Transform myTransform;
        public Transform target;
        public Faction targetFaction;
        // Line Render
        private LineRenderer line;
        private Material instanceMaterial;
        private Vector2 offset;
        public float rate= 0.0397f;

        public bool inGoal;

        public void Initialize(EnergyCore core, int limitRadius)
        {
            this.core = core;
            this.limitRadius = limitRadius+50;
            myTransform = transform;
            line = GetComponent<LineRenderer>();
        }
        private void Update()
        {
            if (target)
            {
                offset.x = Mathf.Repeat(offset.x + rate, 1.0f);
                line.enabled = true;
                line.material.SetTextureOffset("_MainTex", offset);
                line.SetPosition(0, myTransform.position);
                line.SetPosition(1, target.position);
                core.CountEnergy(targetFaction, Time.deltaTime*5.0f);
                if (Vector3.Distance(target.position, myTransform.position) > limitRadius+ tolerance)
                    target = null;
            }
            else
            {
                line.enabled = false;
                core.queueEmitter.Enqueue(this);
                enabled = false;
            }
        }
    }
}