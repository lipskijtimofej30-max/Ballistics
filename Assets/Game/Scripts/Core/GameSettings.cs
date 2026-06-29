using UnityEngine;

namespace Game.Scripts.Core
{
    public class GameSettings
    {
        public ShapeType ShapeType { get; private set; } = ShapeType.Cube;
        public Vector3 StartVelocity { get; private set; } = new Vector3(10f, 0f, 0f);
        public Vector3 StartPosition { get; private set; } = new Vector3(0.0f, 2.0f, 0.0f);

        public Vector3 G { get; private set; } = new Vector3(0f, -9.81f, 0f);
        
        public Vector3 WindVelocity { get; private set; } = new Vector3(10f, 0f, 0f);

        public float WindDensity { get; private set; } = 5f;
    }
}