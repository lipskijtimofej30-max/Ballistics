using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View
{
    public class ProjectileView : MonoBehaviour
    {
        [SerializeField] private Slider _sizeSlider;
        [SerializeField] private Slider _densitySlider;
        [SerializeField] private TextMeshProUGUI _sizeUnitText;
        [SerializeField] private TextMeshProUGUI _densityUnitText;
        [SerializeField] private TextMeshProUGUI _massText;
        [SerializeField] private string _unitSize;
        [SerializeField] private string _unitDensity;
        [SerializeField] private string _unitMass;
        
        public Slider SizeSlider => _sizeSlider;
        public Slider DensitySlider => _densitySlider;

        public void SetSizeText(string text)
        {
            _sizeUnitText.text = $"{text} {_unitSize}";
        }

        public void SetDensityText(string text)
        {
            _densityUnitText.text = $"{text} {_unitDensity}";
        }

        public void SetMassText(string text)
        {
            _massText.text = $"{text} {_unitMass}";
        }
    }
}