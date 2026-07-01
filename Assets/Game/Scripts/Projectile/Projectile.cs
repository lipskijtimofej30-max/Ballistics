using Assets.Game.Scripts.Core.Calculator;
using Assets.Game.Scripts.Settings;
using UnityEngine;

namespace Game.Scripts.Core
{
    public class Projectile : MonoBehaviour
    { 
        [field: SerializeField] public ShapeType ShapeType { get; private set; }
        
        public float Mass { get; private set; }
        public float CrossSectionalArea { get; private set;}
        public float DragCoefficient { get; private set; }

        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }

        private ProjectileSettings _settings;
        
        public void Initialize(ProjectileSettings projectileSettings,SimulationSettings simulationSettings,
            LaunchVelocityCalculator velocityCalculator, MassCalculator massCalculator, CrossSectionalAreaCalculator areaCalculator)
        {
            _settings = projectileSettings;
            
            Velocity = velocityCalculator.GetVelocity();
            Position = simulationSettings.InitialPosition;
            transform.position = Position;
            
            Mass = massCalculator.GetMass(ShapeType);
            CrossSectionalArea = areaCalculator.GetCrossSectionalArea(_settings.ShapeType, _settings.Size);
            DragCoefficient = GetDragCoefficient();
            
            Debug.Log($"Shape name : {gameObject.name}, shape type: {ShapeType}, mass : {Mass}, dragCoefficient: {DragCoefficient}, cross sectional area : {CrossSectionalArea};\n start speed : {simulationSettings.InitialSpeed}, start velocity : {Velocity}, start velocity magnitude : {Velocity.magnitude}");
        }
        
        private float GetDragCoefficient()
        {
            float dragCoefficient = 0f;
            if (ShapeType == ShapeType.Cube)
                dragCoefficient = 1.05f;
            else if (ShapeType == ShapeType.Sphere)
                dragCoefficient = 0.47f;
            return dragCoefficient;
        }
    }
    
    public enum ShapeType
    {
        Cube,
        Sphere
    }
}