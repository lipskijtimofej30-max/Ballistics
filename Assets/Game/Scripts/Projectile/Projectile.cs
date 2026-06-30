using UnityEngine;

namespace Game.Scripts.Core
{
    public class Projectile : MonoBehaviour
    {
        [Tooltip("Для куба размер грани, для шара радиус")]
        [SerializeField] private float _size = 0.5f;
        [SerializeField] private float _density = 1f;
        
        [field: SerializeField] public ShapeType ShapeType { get; private set; }
        
        public float Mass { get; private set; }
        public float CrossSectionalArea { get; private set;}
        public float DragCoefficient { get; private set; }

        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        private ProjectileSettings _settings;
        
        public void Initialize(ProjectileSettings gameSettings, LaunchVelocityCalculator velocityCalculator)
        {
            _settings = gameSettings;
            
            Velocity = velocityCalculator.GetVelocity();
            Position = _settings.StartPosition;
            transform.position = Position;
            
            Mass = GetMass();
            CrossSectionalArea = GetCrossSectionalArea();
            DragCoefficient = GetDragCoefficient();
            
            Debug.Log($"Shape name : {gameObject.name}, shape type: {ShapeType}, mass : {Mass}, dragCoefficient: {DragCoefficient}, cross sectional area : {CrossSectionalArea};\n start speed : {_settings.StartSpeed}, start velocity : {Velocity}, start velocity magnitude : {Velocity.magnitude}");
        }
        
        private float GetMass()
        {
            float mass = 0f;
            if (ShapeType == ShapeType.Cube)
                mass = _density * (_size * _size * _size); //m=p*a^3
            else if (ShapeType == ShapeType.Sphere)
                mass = _density * (4f/3f * Mathf.PI * (_size * _size * _size)); //m=p(4/3пR^3)
            return mass;
        }

        private float GetCrossSectionalArea()
        {
            float area = 0f;
            if (ShapeType == ShapeType.Cube)
                area =_size * _size;
            else if (ShapeType == ShapeType.Sphere)
                area = Mathf.PI * (_size * _size);
            return area;
        }

        private float GetDragCoefficient()
        {
            float cd = 0f;
            if (ShapeType == ShapeType.Cube)
                cd = 1.05f;
            else if (ShapeType == ShapeType.Sphere)
                cd = 0.47f;
            return cd;
        }
    }
    
    public enum ShapeType
    {
        Cube,
        Sphere
    }
}