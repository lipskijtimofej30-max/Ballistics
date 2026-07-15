using Game.Scripts.Core;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View.View
{
    public class VectorVisualizationView : MonoBehaviour
    {
        [field: SerializeField] public ParameterView ScaleParameter { get; private set; }
        [field: SerializeField] public Toggle VelocityToggle { get; private set; }
        [field: SerializeField] public Toggle AccelerationToggle { get; private set; }
        [field: SerializeField] public Toggle ForceToggle { get; private set; }
    }
}