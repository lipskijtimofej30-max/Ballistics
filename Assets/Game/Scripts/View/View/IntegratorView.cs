using Game.Scripts.Core;
using TMPro;
using UnityEngine;

namespace Game.Scripts.View.View
{
    public class IntegratorView : MonoBehaviour
    {
        [field: SerializeField] public ParameterView DtParameter { get; private set; }
        [field: SerializeField] public ParameterView TimeScaleParameter { get; private set; }
        [field: SerializeField] public TMP_Dropdown IntegratorMethodDropdown { get; private set; }
    }
}