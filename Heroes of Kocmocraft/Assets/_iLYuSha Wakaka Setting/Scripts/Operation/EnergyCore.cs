using Photon.Pun;
using System.Collections.Generic;
using UnityEngine;

namespace Kocmoca
{
    public class EnergyCore : MonoBehaviour, IPunObservable
    {
        [Header("Beacon")]
        public int number;
        public float energy { get; set; } = 0;
        private readonly int limitRadius = 163;
        private readonly int countEmitter = 80;
        private Faction faction = Faction.Unknown;
        public Queue<EnergyEmitter> queueEmitter = new Queue<EnergyEmitter>();
        [Header("VFX")]
        public GameObject energyEmitter;
        public GameObject vfxUnknow;
        public GameObject vfxFriend;
        public GameObject vfxFoe;


        private void Awake()
        {
            GetComponent<SphereCollider>().radius = limitRadius;
            for (int i = 0; i < countEmitter; i++)
            {
                EnergyEmitter emitter = Instantiate(energyEmitter).GetComponent<EnergyEmitter>();
                emitter.transform.SetParent(transform);
                emitter.Initialize(this, limitRadius);
                queueEmitter.Enqueue(emitter);
            }
            vfxUnknow.SetActive(true);
            vfxFriend.SetActive(false);
            vfxFoe.SetActive(false);
        }

        private void Start()
        {
            HeadUpDisplayManager.Instance.InitializeBeaconUI(number, faction, transform.position);
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
                //counter.ShowNumber(Mathf.CeilToInt(Mathf.Abs(energy)));
            }
        }

        private void OnTriggerEnter(Collider other)
        {
            if (other.CompareTag("Kocmocraft"))
            {
                EnergyEmitter emitter = queueEmitter.Dequeue();
                emitter.enabled = true;
                emitter.SetReceiver(other.transform, (Faction)other.transform.root.GetComponent<Kocmoport>().m_Faction);
            }
        }

        public void CountEnergy(Faction energySource, float amount)
        {
            if (PhotonNetwork.IsMasterClient)
            {
                energy = Mathf.Clamp(energy + (energySource == Faction.Apovaka ? amount : -amount), -999, 999);
                //counter.ShowNumber(Mathf.CeilToInt(Mathf.Abs(energy)));

                if (energy > 100)
                {
                    if (faction == Faction.Apovaka) return;
                    faction = Faction.Apovaka;
                }
                else if (energy < -100)
                {
                    if (faction == Faction.Perivaka) return;
                    faction = Faction.Perivaka;
                }
                else
                {
                    if (faction == Faction.Unknown) return;
                    faction = Faction.Unknown;
                }
                Identification identification = LocalPlayerRealtimeData.CheckFriendOrFoe(faction);
                HeadUpDisplayManager.Instance.SetBeaconInfo(number, identification);

                vfxUnknow.SetActive(false);
                vfxFriend.SetActive(false);
                vfxFoe.SetActive(false);

                switch (identification)
                {
                    case Identification.Unknown: vfxUnknow.SetActive(true); break;
                    case Identification.Friend: vfxFriend.SetActive(true); break;
                    case Identification.Foe: vfxFoe.SetActive(true); break;
                }
            }
        }
    }
}