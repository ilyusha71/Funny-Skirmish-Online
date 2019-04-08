using System.Collections;
using UnityEngine;
using DG.Tweening;

namespace Kocmoca
{
    public class EnergyEmitter: MonoBehaviour
    {
        private Transform myTransform;
        private EnergyCore core;
        private int limitRadiusSqr;
        private int tolerance = 5;
        // Receiver
        public Transform receiver;
        private Faction receiverFaction;
        // Line Render
        public LineRenderer lineFriend;
        public LineRenderer lineFoe;
        public LineRenderer line;
        private Material instanceMaterial;
        private Vector2 offset;
        private readonly float rate= 0.0397f;

        //public bool inGoal;

        public void Initialize(EnergyCore core, int limitRadius)
        {
            myTransform = transform;
            myTransform.localPosition = Vector3.zero;
            this.core = core;
            limitRadiusSqr = (limitRadius + tolerance)* (limitRadius + tolerance);
            lineFriend.enabled = false;
            lineFoe.enabled = false;
            enabled = false;
        }

        private void FixedUpdate()
        {
            if (receiver)
            {
                offset.x = Mathf.Repeat(offset.x + rate, 1.0f);
                line.material.SetTextureOffset("_MainTex", offset);
                line.SetPosition(0, myTransform.position);
                line.SetPosition(1, receiver.position);
                core.CountEnergy(receiverFaction, Time.deltaTime*5.0f);
                if (Vector3.SqrMagnitude(receiver.position - myTransform.position) > limitRadiusSqr)
                    receiver = null;
            }
            else
            {
                line.enabled = false;
                core.queueEmitter.Enqueue(this);
                enabled = false;
            }
        }

        public void SetReceiver(Transform target, Faction targetFaction)
        {
            receiver = target;
            receiverFaction = targetFaction;
            if (LocalPlayerRealtimeData.CheckFriendOrFoe(targetFaction) == Identification.Friend)
                line = lineFriend;
            else
                line = lineFoe;
            line.enabled = true;
        }
    }
}