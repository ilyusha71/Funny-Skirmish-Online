using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Kocmoca
{
    public class EnergyCore : MonoBehaviour, IPunObservable
    {
        public int beaconNumber;
        public Faction beaconFaction;
        private float energy;
        private int limitRadius = 300;
        private int countEmitter = 12;
        public GameObject prefabEmitter;
        public CounterManager counter;
        public Queue<EnergyEmitter> queueEmitter = new Queue<EnergyEmitter>();

        private void Awake()
        {
            energy = beaconFaction == Faction.Apovaka ? 30 : -30;
            GetComponent<SphereCollider>().radius = limitRadius;
            for (int i = 0; i < countEmitter; i++)
            {
                EnergyEmitter emitter = Instantiate(prefabEmitter).GetComponent<EnergyEmitter>();
                emitter.transform.SetParent(transform);
                emitter.transform.localPosition = Vector3.zero;
                emitter.Initialize(this, limitRadius);
                queueEmitter.Enqueue(emitter);
            }
        }
        private void Start()
        {
            HeadUpDisplayManager.Instance.InitializeBeaconUI(beaconNumber, beaconFaction, transform.position);
        }
        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(energy);
            }
            else
            {
                energy = (float)stream.ReceiveNext();
                counter.ShowNumber(Mathf.CeilToInt(Mathf.Abs(energy)));
            }
        }
        private void OnTriggerEnter(Collider other)
        {
            if (other.tag == "Kocmocraft")
            {
                EnergyEmitter emitter = queueEmitter.Dequeue();
                emitter.target = other.transform;
                emitter.targetFaction = other.GetComponent<KocmocraftManager>().Faction;
                emitter.enabled = true;
            }
        }
        public void CountEnergy(Faction faction, float amount)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                bool isPositive = energy > 0 ? true : false;
                energy = Mathf.Clamp(energy + (faction == Faction.Apovaka ? amount : -amount), -999, 999);
                counter.ShowNumber(Mathf.CeilToInt(Mathf.Abs(energy)));

                if (isPositive != energy > 0 ? true : false)
                {
                    beaconFaction = energy > 0 ? Faction.Apovaka : Faction.Perivaka;
                    HeadUpDisplayManager.Instance.ChangeBeaconFaction(beaconNumber, beaconFaction);
                }
            }
        }
    }
}