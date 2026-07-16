namespace Game.Scripts.Settings
{
    public class VectorVisualizationSettings
    {
        public bool IsActiveVelocity { get; set; } = true;
        public bool IsActiveProjectionVelocity { get; set; } = true;
        public bool IsActiveAcceleration { get; set; } = false;
        public bool IsActiveTotalForce { get; set; } = false;
        public float ScaleLength { get; set; } = 1f;
        public VectorVisualizationSettings Clone() => new VectorVisualizationSettings()
        {
            IsActiveVelocity = this.IsActiveVelocity,
            IsActiveProjectionVelocity = this.IsActiveProjectionVelocity,
            IsActiveAcceleration = this.IsActiveAcceleration,
            IsActiveTotalForce = this.IsActiveTotalForce,
            ScaleLength = this.ScaleLength
        };
    }
}