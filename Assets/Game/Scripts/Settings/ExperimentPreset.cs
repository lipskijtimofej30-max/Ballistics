using Game.Scripts.Core;
using UnityEngine;

namespace Game.Scripts.Settings
{
    public class ExperimentPreset
    {
        public ShapeType ShapeType { get; set; }
        public float Density { get; set; }
        public float Size { get; set; }

        // Параметры запуска
        public float InitialSpeed { get; set; }
        public float LaunchAngle { get; set; }
        public float InitialHeight { get; set; }

        // Окружение
        public Vector3 Gravity { get; set; }
        public bool AirResistanceEnabled { get; set; }
        public Vector3 WindVelocity { get; set; }
        public float AirDensity { get; set; }

        // Интегратор
        public IntegratorMethod IntegratorMethod { get; set; }
        public float IntegrationStep { get; set; }

        public ExperimentPreset(ShapeType shapeType, float density, float size, float initialSpeed,
            float launchAngle, float initialHeight, Vector3 gravity, bool airResistanceEnabled, 
            Vector3 windVelocity, IntegratorMethod integratorMethod, float integrationStep)
        {
            ShapeType = shapeType;
            Density = density;
            Size = size;
            InitialSpeed = initialSpeed;
            LaunchAngle = launchAngle;
            InitialHeight = initialHeight;
            Gravity = gravity;
            AirResistanceEnabled = airResistanceEnabled;
            WindVelocity = windVelocity;
            IntegratorMethod = integratorMethod;
            IntegrationStep = integrationStep;
        }
    }
}