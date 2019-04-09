using Photon.Pun;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kocmoca
{
    [RequireComponent(typeof(AudioSource))]
    public class Engine : MonoBehaviour, IPunObservable
    {
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
                Animation[Clip.name].speed = Reverse ? -Omega* power : Omega* power;
            }
        }
        [Range(0,1)]public float enginePower = 0.5f;
        private AudioSource engineSound;
        [SerializeField]private EngineType engineType= EngineType.IonThruster;
        private float engineMaxVolume;
        private float engineMinThrottlePitch;
        private float engineMaxThrottlePitch;
        [Header("VFX")]
        ParticleSystem.MainModule mainModule;
        public ParticleSystem[] engineThruster;
        public float thrusterStartLife;                //The start life that the thrusters normally have
        public float currentLifeTime;
        public int countVFX;
        [Header("Propeller")]
        public Propeller[] propeller;
        private int countPropeller;

        void Awake()
        {
            engineSound = GetComponent<AudioSource>();
            switch (engineType)
            {
                case EngineType.Turbojet:
                    engineMaxVolume = Turbojet.engineMaxVolume;
                    engineMinThrottlePitch = Turbojet.engineMinThrottlePitch;
                    engineMaxThrottlePitch = Turbojet.engineMaxThrottlePitch;
                    break;
                case EngineType.Turbofan:
                    engineMaxVolume = Turbofan.engineMaxVolume;
                    engineMinThrottlePitch = Turbofan.engineMinThrottlePitch;
                    engineMaxThrottlePitch = Turbofan.engineMaxThrottlePitch;
                    break;
                case EngineType.Turboprop:
                    engineMaxVolume = Turboprop.engineMaxVolume;
                    engineMinThrottlePitch = Turboprop.engineMinThrottlePitch;
                    engineMaxThrottlePitch = Turboprop.engineMaxThrottlePitch;
                    break;
                case EngineType.Turboshaft:
                    engineMaxVolume = Turboshaft.engineMaxVolume;
                    engineMinThrottlePitch = Turboshaft.engineMinThrottlePitch;
                    engineMaxThrottlePitch = Turboshaft.engineMaxThrottlePitch;
                    break;
                case EngineType.IonThruster:
                    engineMaxVolume = IonThruster.engineMaxVolume;
                    engineMinThrottlePitch = IonThruster.engineMinThrottlePitch;
                    engineMaxThrottlePitch = IonThruster.engineMaxThrottlePitch;
                    break;
                case EngineType.BiomassEnergy:
                    engineMaxVolume = BiomassEnergy.engineMaxVolume;
                    engineMinThrottlePitch = BiomassEnergy.engineMinThrottlePitch;
                    engineMaxThrottlePitch = BiomassEnergy.engineMaxThrottlePitch;
                    break;
                case EngineType.PulsedPlasmaThruster:
                    engineMaxVolume = PulsedPlasmaThruster.engineMaxVolume;
                    engineMinThrottlePitch = PulsedPlasmaThruster.engineMinThrottlePitch;
                    engineMaxThrottlePitch = PulsedPlasmaThruster.engineMaxThrottlePitch;
                    break;
            }
            engineThruster = GetComponentsInChildren<ParticleSystem>();
            countVFX = engineThruster.Length;
            //Record the thruster's particle start life property
            if (countVFX > 0)
            {
                mainModule = engineThruster[0].main;
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
            Power(0.3f);
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
            engineSound.volume = Mathf.Lerp(0, engineMaxVolume, enginePower);
            engineSound.pitch = Mathf.Lerp(engineMinThrottlePitch, engineMaxThrottlePitch, enginePower);

            if (countVFX > 0)
            {
                 currentLifeTime = thrusterStartLife * enginePower;

                //If the thrusters are powered on at all...
                if (currentLifeTime > 0f)
                {
                    for (int i = 0; i < countVFX; i++)
                    {
                        //...play the particle systems...
                        engineThruster[i].Play();

                        //...update the particle life for the left thruster...
                        mainModule = engineThruster[i].main;
                        mainModule.startLifetime = currentLifeTime;
                    }
                }
                //...Otherwise stop the particle effects
                else
                {
                    for (int i = 0; i < countVFX; i++)
                    {
                        //...play the particle systems...
                        engineThruster[i].Stop();
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
