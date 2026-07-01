using Game.Scripts.Core;
using UnityEngine;

namespace Assets.Game.Scripts.View
{
    public class SimulationView : MonoBehaviour
    {
        [field: SerializeField] public ParameterView InitialSpeedParameter { get; private set; }
        [field: SerializeField] public ParameterView HeightParameter { get; private set; }
        [field: SerializeField] public ParameterView AngleParameter { get; private set; }
    }
}
