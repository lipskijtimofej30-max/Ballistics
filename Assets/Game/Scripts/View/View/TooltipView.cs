using Game.Scripts.Core.Tooltip;
using TMPro;
using UnityEngine;

namespace Game.Scripts.View.View
{
    public class TooltipView : MonoBehaviour, ITooltipView
    {
        [SerializeField] private GameObject _rootPanel;
        [SerializeField] private TMP_Text _text;
        [SerializeField] private Vector2 _offset = new Vector2(0f, -15f);
        [SerializeField] private Canvas _parentCanvas;

        private RectTransform _rectTransform;
        private Vector2 _canvasSize;

        private void Awake()
        {
            _rectTransform = _rootPanel.GetComponent<RectTransform>();
            _rootPanel.SetActive(false);
        }

        public void Show(string text, Vector2 screenPosition)
        {
            _text.text = text;
            PositionPanel(screenPosition);
            _rootPanel.SetActive(true);
        }

        public void Hide()
        {
            _rootPanel.SetActive(false);
        }

        private void PositionPanel(Vector2 screenPosition)
        {
            if (_parentCanvas == null) return;

            RectTransformUtility.ScreenPointToLocalPointInRectangle(
                _parentCanvas.transform as RectTransform,
                screenPosition,
                _parentCanvas.worldCamera,
                out Vector2 localPoint
            );
            _rectTransform.localPosition = localPoint + _offset;
        }
    }
}