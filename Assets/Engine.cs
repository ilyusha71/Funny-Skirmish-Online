using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace Kocmoca
{
    [RequireComponent(typeof(AudioSource))]
    public class Engine : MonoBehaviour
    {
        [Serializable]
        public class Propeller // A class for storing the advanced options.
        {
            public GameObject Rotor;
            public Animation Animation;
            public bool Reverse;
        }
        [Range(0,1)]public float power;
        private AudioSource engineSound;
        [SerializeField]private EngineType engineType;
        private float engineMaxVolume;
        private float engineMinThrottlePitch;
        private float engineMaxThrottlePitch;
        [Header("Propeller")]
        public Propeller[] propeller;


        public void InitializeEngine()
        {
            engineSound = GetComponent<AudioSource>();
            power = 0.5f;
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
        }

        void Awake()
        {
            InitializeEngine();
        }
        private void Update()
        {
            engineSound.volume = Mathf.Lerp(0, engineMaxVolume, power);
            engineSound.pitch = Mathf.Lerp(engineMinThrottlePitch, engineMaxThrottlePitch, power);
        }
    }
}
