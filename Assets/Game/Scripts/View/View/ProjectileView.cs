using Game.Scripts.Core;
using TMPro;
using UnityEngine;

namespace Game.Scripts.View
{
    public class ProjectileView : MonoBehaviour
    {
        [field:SerializeField] public ParameterView SizeParameter { get; private set; }
        [field:SerializeField] public ParameterView DensityParameter { get; private set; }
        [Header("Mass")]
        [SerializeField] private TextMeshProUGUI _massText;
        [SerializeField] private string _unitMass;

        public void SetMassText(string text) => _massText.text = $"{text} {_unitMass}";
    }
}