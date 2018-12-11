﻿using Photon.Pun;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kocmoca
{
    public class EngineController : MonoBehaviour, IPunObservable
    {
        [Header("Dependent Components")]
        private AudioSource myAudioSource;
        private PropellerController myPropeller;
        [Header("Engine Data")]
        public float enginePower;
        public float engineMinVol;
        public float engineMaxVol;
        public float engineMinPitch;
        public float engineMaxPitch;
        ParticleSystem.MainModule mainModule;
        public ParticleSystem[] engineThruster;
        float thrusterStartLife;                //The start life that the thrusters normally have

        public void Initialize(Type type)
        {
            // Dependent Components
            myAudioSource = GetComponent<AudioSource>();
            myPropeller = GetComponentInChildren<PropellerController>();
            GetComponent<PhotonView>().ObservedComponents.Add(this);
            // Engine Data
            switch (KocmocraftData.GetEngineType(type))
            {
                case EngineType.TurboJet:
                    engineMinVol = TurboJet.engineMinVol;
                    engineMaxVol = TurboJet.engineMaxVol;
                    engineMinPitch = TurboJet.engineMinPitch;
                    engineMaxPitch = TurboJet.engineMaxPitch;
                    break;
                case EngineType.TurboFan:
                    engineMinVol = TurboFan.engineMinVol;
                    engineMaxVol = TurboFan.engineMaxVol;
                    engineMinPitch = TurboFan.engineMinPitch;
                    engineMaxPitch = TurboFan.engineMaxPitch;
                    break;
                case EngineType.TurboProp:
                    engineMinVol = Propeller.engineMinVol;
                    engineMaxVol = Propeller.engineMaxVol;
                    engineMinPitch = Propeller.engineMinPitch;
                    engineMaxPitch = Propeller.engineMaxPitch;
                    break;
            }
            engineThruster = GetComponentsInChildren<ParticleSystem>();

            //Record the thruster's particle start life property
            if (engineThruster.Length > 1)
            {
                mainModule = engineThruster[1].main;
                thrusterStartLife = mainModule.startLifetime.constant;
            }
        }
        private void Update()
        {
            EngineControl();
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
        void EngineControl()
        {
            myAudioSource.volume = Mathf.Lerp(engineMinVol, engineMaxVol, enginePower);
            myAudioSource.pitch = Mathf.Lerp(engineMinPitch, engineMaxPitch, enginePower);
            if (myPropeller) myPropeller.SetAnimationSpeed(enginePower);

            if (engineThruster.Length > 1)
            {
                float currentLifeTime = thrusterStartLife * enginePower;

                //If the thrusters are powered on at all...
                if (currentLifeTime > 0f)
                {
                    for (int i = 1; i < engineThruster.Length; i++)
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
                    for (int i = 1; i < engineThruster.Length; i++)
                    {
                        //...play the particle systems...
                        engineThruster[i].Stop();
                    }
                }
            }
        }

    }
}
