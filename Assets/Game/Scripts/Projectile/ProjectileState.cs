using UnityEngine;

namespace Game.Scripts.Core
{
        public class ProjectileState
        {
            public ShapeType ShapeType { get; }
            public float Size { get; }
            public float Density { get; }
 
            public Vector3 Position { get; set; }
            public Vector3 Velocity { get; set; }
            public float Mass { get; }
            public float CrossSectionalArea { get; }
            public float DragCoefficient { get; }

            public Vector3 Acceleration { get; set; }
            public Vector3 TotalForce { get; set; }

            public ProjectileState(
                ShapeType shapeType, float size, float density,
                Vector3 position, Vector3 velocity,
                float mass, float crossSectionalArea, float dragCoefficient)
            {
                ShapeType = shapeType;
                Size = size;
                Density = density;
                Position = position;
                Velocity = velocity;
                Mass = mass;
                CrossSectionalArea = crossSectionalArea;
                DragCoefficient = dragCoefficient;
                Acceleration = Vector3.zero;
                TotalForce = Vector3.zero;
            }
       
 
            public ProjectileState Clone()
            {
                var clone = new ProjectileState(ShapeType, Size, Density, Position, Velocity, Mass, CrossSectionalArea, DragCoefficient);
                clone.Acceleration = Acceleration;
                clone.TotalForce = TotalForce;
                return clone;
            }
        }
}