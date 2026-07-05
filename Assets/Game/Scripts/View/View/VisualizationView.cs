using Game.Scripts.Core;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View.View
{
    public class VisualizationView : MonoBehaviour
    {
        [field:SerializeField] public ParameterView WidthParameter { get; private set; }
        [field:SerializeField] public TMP_Dropdown ColorDropdown { get; private set; }
        [field:SerializeField] public Toggle PreviewToggle { get; private set; }
    }
}