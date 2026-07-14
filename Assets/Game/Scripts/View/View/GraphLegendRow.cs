using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View.View
{
    public class GraphLegendRow : MonoBehaviour
    {
        [SerializeField] private Image _colorImage;
        [SerializeField] private TMP_Text _nameText;

        public void Initialize(Color color, string text)
        {
            _colorImage.color = color;
            _nameText.text = text;
        }
    }
}