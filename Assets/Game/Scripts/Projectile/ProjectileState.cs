using UnityEngine;

namespace Game.Scripts.Core
{
    public class ProjectileState
    {
        public Vector3 Position { get; set; }
        public Vector3 Velocity { get; set; }
        public float Mass { get; }
        public float CrossSectionalArea { get; }
        public float DragCoefficient { get; }

        public ProjectileState(Vector3 position,Vector3 velocity, float mass, float crossSectionalArea, float dragCoefficient)
        {
            Position = position;
            Velocity = velocity;
            Mass = mass;
            CrossSectionalArea = crossSectionalArea;
            DragCoefficient = dragCoefficient;
        }
        
        public ProjectileState Clone()
        {
            return new ProjectileState(Position, Velocity, Mass, CrossSectionalArea, DragCoefficient);
        }
    }
}