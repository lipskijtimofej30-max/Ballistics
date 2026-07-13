using UnityEngine;

namespace Game.Scripts
{
    public class CameraMover : MonoBehaviour
    {
        [SerializeField] private Camera _camera;

        [Header("Настройки движения")] [Tooltip("Скорость перемещения камеры")] [SerializeField]
        private float _moveSpeed = 15f;

        [Tooltip("Скорость сглаживания (отставания). Чем меньше, тем сильнее отставание.")] [SerializeField]
        private float _smoothing = 5f;

        [Header("Ограничения (Clamp)")]
        [Tooltip("Минимальные координаты (X - по ширине, Y - по глубине/оси Z)")]
        [SerializeField]
        private Vector2 _minBounds;

        [Tooltip("Максимальные координаты (X - по ширине, Y - по глубине/оси Z)")] [SerializeField]
        private Vector2 _maxBounds;

        [Header("Настройки Зума (FOV)")] 
        [SerializeField] private float _minFOV = 60f;
        [SerializeField] private float _maxFOV = 135f;
        [Tooltip("Чувствительность колесика мыши")]
        [SerializeField] private float _fovSpeed = 30f;
        [Tooltip("Плавность зума")] 
        [SerializeField] private float _fovSmoothing = 10f;
        [Tooltip("Если до максимума осталось меньше этого значения, камера сама плавно дотянет FOV до упора")]
        [SerializeField] private float _fovSnapThreshold = 3f;
        [SerializeField] private float _baseFOV = 120f;
        
        private Vector3 _targetPosition;
        private float _targetFOV;

        private void Start()
        {
            _targetPosition = transform.position;

            if (_camera != null)
                _targetFOV = _camera.fieldOfView;
        }

        private void LateUpdate()
        {
            if (_camera == null) return;

            HandleInput();
            HandleFOVInput();

            ApplyMovementAndZoom();
        }

        private void HandleInput()
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical");

            Vector3 moveDirection = new Vector3(inputX, inputY, 0f).normalized;

            if (moveDirection != Vector3.zero)
            {
                _targetPosition += moveDirection * _moveSpeed * Time.deltaTime;
            }

            _targetPosition.x = Mathf.Clamp(_targetPosition.x, _minBounds.x, _maxBounds.x);
            _targetPosition.y = Mathf.Clamp(_targetPosition.y, _minBounds.y, _maxBounds.y);
        }

        private void HandleFOVInput()
        {
            float scrollInput = Input.GetAxis("Mouse ScrollWheel");

            if (scrollInput != 0f)
            {
                _targetFOV -= scrollInput * _fovSpeed;
                _targetFOV = Mathf.Clamp(_targetFOV, _minFOV, _maxFOV);
            }
            else
            {
                if (_baseFOV - _targetFOV <= _fovSnapThreshold && _targetFOV < _baseFOV + _fovSnapThreshold)
                {
                    _targetFOV = _baseFOV;
                }
            }
        }

        private void ApplyMovementAndZoom()
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, _smoothing * Time.deltaTime);
            _camera.fieldOfView = Mathf.Lerp(_camera.fieldOfView, _targetFOV, _fovSmoothing * Time.deltaTime);
        }
    }
}