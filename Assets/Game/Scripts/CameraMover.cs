using UnityEngine;

namespace Game.Scripts
{
    public class CameraMover : MonoBehaviour
    {
        [Header("Настройки движения")]
        [Tooltip("Скорость перемещения камеры")]
        public float moveSpeed = 15f;
        [Tooltip("Скорость сглаживания (отставания). Чем меньше, тем сильнее отставание.")]
        public float smoothing = 5f;

        [Header("Ограничения (Clamp)")]
        [Tooltip("Минимальные координаты (X - по ширине, Y - по глубине/оси Z)")]
        public Vector2 minBounds;
        [Tooltip("Максимальные координаты (X - по ширине, Y - по глубине/оси Z)")]
        public Vector2 maxBounds;

        private Vector3 _targetPosition;

        private void Start()
        {
            _targetPosition = transform.position;
        }

        private void LateUpdate()
        {
            HandleInput();
            MoveCamera();
        }

        private void HandleInput()
        {
            float inputX = Input.GetAxisRaw("Horizontal");
            float inputY = Input.GetAxisRaw("Vertical"); 

            Vector3 moveDirection = new Vector3(inputX, inputY, 0f).normalized;

            if (moveDirection != Vector3.zero)
            {
                _targetPosition += moveDirection * moveSpeed * Time.deltaTime;
            }

            _targetPosition.x = Mathf.Clamp(_targetPosition.x, minBounds.x, maxBounds.x);
            _targetPosition.y = Mathf.Clamp(_targetPosition.y, minBounds.y, maxBounds.y);
        }

        private void MoveCamera()
        {
            transform.position = Vector3.Lerp(transform.position, _targetPosition, smoothing * Time.deltaTime);
        }
    }
}