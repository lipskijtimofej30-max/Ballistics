using UnityEngine;

namespace Game.Scripts.Core
{
    public class EnvironmentSettings
    {
        public Vector3 Gravity { get; private set; } = new Vector3(0f, -9.81f, 0f);
        
        public Vector3 WindVelocity { get; private set; } = new Vector3(-10f, 0f, 0f);

        public float AirDensity { get; private set; } = 1.22f;
        public bool AirResistance { get; private set; } = true;
    }
}