using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kocmoca
{
    [RequireComponent(typeof(AudioSource))]
    public class PowerController : MonoBehaviour, IPunObservable
    {
        [Header("Presetting")]
        public EngineType engineType; // 未来用ScriptableObject取代
        public AudioSource engineSound;
        public ParticleSystem[] airflow;
        public int countAirflow;

        [Serializable]
        public class Propeller // A class for storing the advanced options.
        {
            public GameObject Hopter;
            public AnimationClip Clip;
            public float Omega;
            public bool Reverse;
            private Animation Animation;

            public void InitializePropeller()
            {
                Animation = Hopter.AddComponent<Animation>();
                Animation.clip = Clip;
                Animation.AddClip(Clip, Clip.name);
                Animation[Clip.name].speed = Reverse ? -Omega : Omega;
                Animation.Play();
            }
            public void Power(float power)
            {
                Animation[Clip.name].speed = Reverse ? -Omega * power : Omega * power;
            }
        }
        [Range(0, 1)] public float enginePower = 0.5f;
        private float MaxVolume;
        private float MinPitch;
        private float MaxPitch;
        [Header("VFX")]
        ParticleSystem.MainModule mainModule;
        public float thrusterStartLife;                //The start life that the thrusters normally have
        public float currentLifeTime;
        [Header("Propeller")]
        public Propeller[] propeller;
        private int countPropeller;

#if UNITY_EDITOR
        public void Preset(PowerSystem powerSystem)
        {
            engineSound = GetComponent<AudioSource>();
            airflow = GetComponentsInChildren<ParticleSystem>();
            countAirflow = airflow.Length;
        }
#endif

        void Awake()
        {
                        engineSound = GetComponent<AudioSource>();

            switch (engineType)
            {
                case EngineType.Turbojet:
                    MaxVolume = Turbojet.MaxVolume;
                    MinPitch = Turbojet.MinPitch;
                    MaxPitch = Turbojet.MaxPitch;
                    break;
                case EngineType.Turbofan:
                    MaxVolume = Turbofan.MaxVolume;
                    MinPitch = Turbofan.MinPitch;
                    MaxPitch = Turbofan.MaxPitch;
                    break;
                case EngineType.Turboprop:
                    MaxVolume = Turboprop.MaxVolume;
                    MinPitch = Turboprop.MinPitch;
                    MaxPitch = Turboprop.MaxPitch;
                    break;
                case EngineType.Turboshaft:
                    MaxVolume = Turboshaft.MaxVolume;
                    MinPitch = Turboshaft.MinPitch;
                    MaxPitch = Turboshaft.MaxPitch;
                    break;
                case EngineType.IonThruster:
                    MaxVolume = IonThruster.MaxVolume;
                    MinPitch = IonThruster.MinPitch;
                    MaxPitch = IonThruster.MaxPitch;
                    break;
                case EngineType.BiomassEnergy:
                    MaxVolume = BiomassEnergy.MaxVolume;
                    MinPitch = BiomassEnergy.MinPitch;
                    MaxPitch = BiomassEnergy.MaxPitch;
                    break;
                case EngineType.PulsedPlasmaThruster:
                    MaxVolume = PulsedPlasmaThruster.MaxVolume;
                    MinPitch = PulsedPlasmaThruster.MinPitch;
                    MaxPitch = PulsedPlasmaThruster.MaxPitch;
                    break;
            }
            countAirflow = airflow.Length;
            //Record the thruster's particle start life property
            if (countAirflow > 0)
            {
                mainModule = airflow[0].main;
                thrusterStartLife = mainModule.startLifetime.constant;
            }

            countPropeller = propeller.Length;
            for (int i = 0; i < countPropeller; i++)
            {
                propeller[i].InitializePropeller();
            }
        }

        void Start()
        {
            // Burn-In
            Power(0.4f);
            PhotonView view = transform.root.GetComponent<PhotonView>();
            if (view)
                view.ObservedComponents.Add(this);
        }

        public void OnPhotonSerializeView(PhotonStream stream, PhotonMessageInfo info)
        {
            if (stream.IsWriting)
            {
                stream.SendNext(enginePower);
            }
            else
            {
                enginePower = (float)stream.ReceiveNext();
            }
        }
        public void Power(float power)
        {
            enginePower = power;
            engineSound.volume = Mathf.Lerp(0, MaxVolume, enginePower);
            engineSound.pitch = Mathf.Lerp(MinPitch, MaxPitch, enginePower);

            if (countAirflow > 0)
            {
                currentLifeTime = thrusterStartLife * enginePower;

                //If the thrusters are powered on at all...
                if (currentLifeTime > 0f)
                {
                    for (int i = 0; i < countAirflow; i++)
                    {
                        //...play the particle systems...
                        airflow[i].Play();

                        //...update the particle life for the left thruster...
                        mainModule = airflow[i].main;
                        mainModule.startLifetime = currentLifeTime;
                    }
                }
                //...Otherwise stop the particle effects
                else
                {
                    for (int i = 0; i < countAirflow; i++)
                    {
                        //...play the particle systems...
                        airflow[i].Stop();
                    }
                }
            }

            for (int i = 0; i < countPropeller; i++)
            {
                propeller[i].Power(enginePower);
            }
        }
    }
}
