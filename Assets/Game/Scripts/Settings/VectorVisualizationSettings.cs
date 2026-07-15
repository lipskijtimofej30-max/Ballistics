namespace Game.Scripts.Settings
{
    public class VectorVisualizationSettings
    {
        public bool IsActiveVelocity { get; set; } = true;
        public bool IsActiveAcceleration { get; set; } = true;
        public bool IsActiveTotalForce { get; set; } = false;
        public float ScaleLength { get; set; } = 1f;
        public VectorVisualizationSettings Clone() => new VectorVisualizationSettings()
        {
            IsActiveVelocity = this.IsActiveVelocity,
            IsActiveAcceleration = this.IsActiveAcceleration,
            IsActiveTotalForce = this.IsActiveTotalForce,
            ScaleLength = this.ScaleLength
        };
    }
}