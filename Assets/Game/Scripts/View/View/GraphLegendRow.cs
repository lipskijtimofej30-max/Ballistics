using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View.View
{
    [RequireComponent(typeof(CanvasGroup))]
    public class GraphLegendRow : MonoBehaviour
    {
        [SerializeField] private Image _colorImage;
        [SerializeField] private Button _legendButton;
        [SerializeField] private TMP_Text _nameText;
        [SerializeField] private CanvasGroup _canvasGroup;

        private int _index;
        private Action<int> _onClicked;

        private void Awake()
        {
            _legendButton.onClick.AddListener(() => _onClicked?.Invoke(_index));
        }
        public void Initialize(int index, Color color, string title, bool isVisible, Action<int> onClicked)
        {
            _index = index;
            _colorImage.color = color;
            _nameText.text = title;
            _onClicked = onClicked;
        
            SetVisualState(isVisible);
        }

        public void SetVisualState(bool isVisible)
        {
            _canvasGroup.alpha = isVisible ? 1f : 0.5f; 
        }
    }
}