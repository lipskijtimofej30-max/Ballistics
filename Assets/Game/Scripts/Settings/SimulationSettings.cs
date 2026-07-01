using UnityEngine;

namespace Assets.Game.Scripts.Settings
{
    public class SimulationSettings
    {
        public float InitialSpeed { get;set; } = 20f;
        public float LaunchAngle { get; set; } = 30f;
        public Vector3 InitialPosition { get; set; } = new Vector3(0f, 10.0f, 0.0f);
    }
}
