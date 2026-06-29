using UnityEngine;

namespace Game.Scripts.Core
{
    [RequireComponent(typeof(Rigidbody))]
    public class Projectile : MonoBehaviour
    {
        [Tooltip("Для куба размер грани, для шара радиус")]
        [SerializeField] private float _size = 0.5f;
        [SerializeField] private float _density = 1f;
        
        [field: SerializeField] public ShapeType ShapeType { get; private set; }
        public float Mass { get; private set; }
        public float CrossSectionalArea { get; private set;}
        public Rigidbody Rigidbody { get; private set; }
        
        private GameSettings _gameSettings;

        private void Awake()
        {
            Rigidbody = GetComponent<Rigidbody>();
        }
        
        public void Initialize(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
            
            Rigidbody.velocity = _gameSettings.StartVelocity;
            transform.position = _gameSettings.StartPosition;
            
            Mass = GetMass();
            CrossSectionalArea = GetCrossSectionalArea();
        }
        
        private float GetMass()
        {
            float mass = 0f;
            if (ShapeType == ShapeType.Cube)
                mass = _density * (_size * _size * _size); //m=p*a^3
            else if (ShapeType == ShapeType.Sphere)
                mass = _density * (4/3 * Mathf.PI * (_size * _size * _size)); //m=p(4/3пR^3)
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
    }
    
    public enum ShapeType
    {
        Cube,
        Sphere
    }
}