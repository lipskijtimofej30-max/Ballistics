using Game.Scripts.Core;
using TMPro;
using UnityEngine;

namespace Game.Scripts.View.View
{
    public class ExperimentSettingsView : MonoBehaviour
    {
        [field: SerializeField] public ParameterView MinParameter { get; private set; }
        [field: SerializeField] public ParameterView MaxParameter { get; private set; }
        [field: SerializeField] public ParameterView StepParameter { get; private set; }
        [field: SerializeField] public ParameterView PauseParameter { get; private set; }
        [field: SerializeField] public TMP_Dropdown DropdownParameter { get; private set; }
    }
}