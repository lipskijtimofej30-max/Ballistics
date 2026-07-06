using Game.Scripts.Core;

namespace Game.Scripts.Infrastructure.Signals
{
    public class ShapeDropdownChangedSignal
    {
        public ShapeType Value { get; }
        
        public ShapeDropdownChangedSignal(ShapeType value)
        {
            Value = value;
        }
    }
}