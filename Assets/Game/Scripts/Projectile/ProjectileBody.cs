using UnityEngine;

namespace Game.Scripts.Core
{
    public class ProjectileBody : MonoBehaviour
    { 
        [field: SerializeField] public ShapeType ShapeType { get; private set; }
        public void SyncTransform(Vector3 position) => transform.position = position;
    }
    
    public enum ShapeType
    {
        Cube,
        Sphere
    }
}