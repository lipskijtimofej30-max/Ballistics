namespace Game.Scripts.Core.Simulation
{
    public class SimulationComparisons
    {
        public string DisplayName { get; }
        public string Unit { get; }
        public float PreviousValue { get; }
        public float CurrentValue { get; }

        public float GetDifference()
        { return CurrentValue - PreviousValue; }

        public SimulationComparisons(string displayName, string unit, float previousValue, float currentValue)
        {
            DisplayName = displayName;
            Unit = unit;
            PreviousValue = previousValue;
            CurrentValue = currentValue;
        }
    }
}