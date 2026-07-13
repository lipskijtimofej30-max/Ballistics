namespace Assets.Game.Scripts.Core.Experiment.Parameter
{
    public interface IExperimentParameter
    {
        public string DisplayName { get; }
        public string Unit { get; }
        public float GetValue();
        public void SetValue(float value);
        
        public float MinRangeValue { get; }
        public float MaxRangeValue { get; }
    }
}
