using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace Game.Scripts.View.View
{
    public class GraphTooltipView : MonoBehaviour, IPointerMoveHandler, IPointerExitHandler
    {
        [SerializeField] private RectTransform _hitbox;
        [SerializeField] private RectTransform _tooltipPanel;
        [SerializeField] private TMP_Text _tooltipText;
        [SerializeField] private RectTransform _cursorDot;

        public event Action<Vector2> OnPointerMoved; 
        public event Action OnPointerExited;

        private void Start()
        {
            Hide();
        }

        public void OnPointerMove(PointerEventData eventData)
        {
            if (RectTransformUtility.ScreenPointToLocalPointInRectangle(
                    _hitbox, eventData.position, eventData.pressEventCamera, out Vector2 localPoint))
            {
                Vector2 normalized = new Vector2(
                    localPoint.x / _hitbox.rect.width,
                    localPoint.y / _hitbox.rect.height
                );
                
                normalized.x = Mathf.Clamp01(normalized.x);
                normalized.y = Mathf.Clamp01(normalized.y);
                
                OnPointerMoved?.Invoke(normalized);
            }
        }

        public void OnPointerExit(PointerEventData eventData) => OnPointerExited?.Invoke();

        public void Show(string text)
        {
            _tooltipPanel.gameObject.SetActive(true);
            _cursorDot.gameObject.SetActive(true);
            _tooltipText.text = text;
        }
        
        public void UpdateDotPosition(Vector2 normalizedPos)
        {
            if (_cursorDot == null) return;

            if (!_cursorDot.gameObject.activeSelf)
                _cursorDot.gameObject.SetActive(true);

            Vector2 localPosInHitbox = new Vector2(
                Mathf.Lerp(_hitbox.rect.xMin, _hitbox.rect.xMax, normalizedPos.x),
                Mathf.Lerp(_hitbox.rect.yMin , _hitbox.rect.yMax, normalizedPos.y)
            );

            Vector3 worldPosition = _hitbox.TransformPoint(localPosInHitbox);
            
            _cursorDot.position = worldPosition;
        }

        public void Hide()
        {
            _tooltipPanel.gameObject.SetActive(false);
            if (_cursorDot != null) _cursorDot.gameObject.SetActive(false);
        }
    }
}