using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts.Settings
{
    public class IntegratorSettings
    {
        public IntegratorMethod IntegratorMethod { get; set; } = IntegratorMethod.SymplecticEuler;
        public float IntegrationStep {get; set;} = Time.fixedDeltaTime;
        public float TimeScale {get; set;} = 1.0f;
    }
}