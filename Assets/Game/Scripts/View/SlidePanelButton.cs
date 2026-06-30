using System.Net.NetworkInformation;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace Game.Scripts.View
{
    [RequireComponent(typeof(Button))]
    public class SlidePanelButton : MonoBehaviour
    {
        [Header("Panel")]
        [SerializeField] private RectTransform panel;
        [SerializeField] private RectTransform _text;

        [Header("Animation")]
        [SerializeField] private float duration = 0.35f;
        [SerializeField] private Ease ease = Ease.OutCubic;

        private Vector2 _shownPosition;
        private Vector2 _hiddenPosition;
        private bool _isOpened;
        private Tween _currentTween;
        
        private Button _button;

        private void Awake()
        {
            _button = GetComponent<Button>();
            // Позиция, когда панель открыта
            _shownPosition = panel.anchoredPosition;

            // Сдвигаем панель вправо на ее ширину
            _hiddenPosition = _shownPosition + Vector2.right * panel.rect.width;

            // Скрываем при старте
            panel.anchoredPosition = _hiddenPosition;
            _button.onClick.AddListener(() => Toggle());
        }

        public void Toggle()
        {
            _currentTween?.Kill();

            _currentTween = panel
                .DOAnchorPos(_isOpened ? _hiddenPosition : _shownPosition, duration)
                .SetEase(ease);

            _isOpened = !_isOpened;
            _text.rotation = _isOpened ? Quaternion.Euler(0f, 0f, 180f) : Quaternion.identity;
        }

        public void Open()
        {
            if (_isOpened) return;

            _currentTween?.Kill();

            _currentTween = panel
                .DOAnchorPos(_shownPosition, duration)
                .SetEase(ease);
            
            _text.rotation = Quaternion.Euler(0f, 0f, 0f);
            _isOpened = true;
        }

        public void Close()
        {
            if (!_isOpened) return;

            _currentTween?.Kill();

            _currentTween = panel
                .DOAnchorPos(_hiddenPosition, duration)
                .SetEase(ease);
            
            _text.rotation = Quaternion.Euler(0f, 0f, 180f);
            _isOpened = false;
        }
    }
}