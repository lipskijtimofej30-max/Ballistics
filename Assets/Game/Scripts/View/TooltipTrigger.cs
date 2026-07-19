using System.Collections;
using Game.Scripts.Infrastructure.Signals;
using UnityEngine;
using UnityEngine.EventSystems;
using Zenject;

namespace Game.Scripts.View
{
    public class TooltipTrigger : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
    {
        [SerializeField, TextArea] private string _tooltipText = "Введите подсказку";
        [SerializeField] private float _delay = 0.5f;
        [SerializeField] private RectTransform _targetRect; // если не задан, используется RectTransform этого объекта

        private SignalBus _signalBus;
        private Coroutine _showCoroutine;
        private RectTransform _rectTransform;
        private Canvas _canvas;

        [Inject]
        private void Construct(SignalBus signalBus)
        {
            _signalBus = signalBus;
        }

        private void Awake()
        {
            _rectTransform = _targetRect != null ? _targetRect : GetComponent<RectTransform>();
            _canvas = GetComponentInParent<Canvas>();
        }

        public void OnPointerEnter(PointerEventData eventData)
        {
            if (_showCoroutine != null)
                StopCoroutine(_showCoroutine);
            _showCoroutine = StartCoroutine(ShowAfterDelay());
        }

        public void OnPointerExit(PointerEventData eventData)
        {
            if (_showCoroutine != null)
            {
                StopCoroutine(_showCoroutine);
                _showCoroutine = null;
            }

            _signalBus.Fire(new HideTooltipSignal());
        }

        private IEnumerator ShowAfterDelay()
        {
            yield return new WaitForSeconds(_delay);
            Vector2 bottomCenter = GetBottomCenterScreenPosition();
            _signalBus.Fire(new ShowTooltipSignal(_tooltipText, bottomCenter));
            _showCoroutine = null;
        }

        private Vector2 GetBottomCenterScreenPosition()
        {
            Vector3[] corners = new Vector3[4];
            _rectTransform.GetWorldCorners(corners);
            // corners[0] — нижний левый, corners[3] — нижний правый
            Vector3 bottomCenterWorld = (corners[0] + corners[3]) / 2f;

            Camera cam = _canvas != null ? _canvas.worldCamera : null;
            Vector2 screenPoint = RectTransformUtility.WorldToScreenPoint(cam, bottomCenterWorld);
            return screenPoint;
        }

        private void OnDisable()
        {
            if (_showCoroutine != null)
            {
                StopCoroutine(_showCoroutine);
                _showCoroutine = null;
            }

            if (_signalBus != null)
                _signalBus.Fire(new HideTooltipSignal());
        }
    }
}