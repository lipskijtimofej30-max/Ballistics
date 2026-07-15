using Game.Scripts.Core;
using System;
using Game.Scripts.Settings;
using UnityEngine;
using Zenject;

namespace Assets.Game.Scripts.View.View
{
    public class VectorRenderer : MonoBehaviour
    {
        [Serializable]
        public class VectorSetup
        {
            public SpriteRenderer ArrowSprite;
            public float LengthMultiplier = 1f;
            public float MinLength = 1f;
            public float MaxLength = 10f;
        }
    
        [SerializeField] private VectorSetup _velocitySetup;
        [SerializeField] private VectorSetup _accelerationSetup;
        [SerializeField] private VectorSetup _totalForceSetup;
        
        private VectorVisualizationSettings _settings;

        [Inject]
        private void Construct(VectorVisualizationSettings visualizationSettings)
        {
            _settings = visualizationSettings;
            ClearAll();
        }

        public void UpdateVectors(ProjectileState state)
        {
            UpdateVectors(state.Position, state.Velocity, state.Acceleration, state.TotalForce, state.Size);
        }

        // Добавляем новый универсальный метод
        public void UpdateVectors(Vector3 position, Vector3 velocity, Vector3 acceleration, Vector3 netForce, float projectileSize)
        {
            if (_settings == null) return; 

            // Передаем также ScaleLength, если вы добавили его в настройки
            UpdateArrow(_velocitySetup, velocity, position, projectileSize, _settings.IsActiveVelocity);
            UpdateArrow(_accelerationSetup, acceleration, position, projectileSize, _settings.IsActiveAcceleration);
            UpdateArrow(_totalForceSetup, netForce, position, projectileSize, _settings.IsActiveTotalForce);
        }

        private void UpdateArrow(VectorSetup setup, Vector3 vectorValue, Vector3 centerPosition, float projectileSize, bool isVisible)
        {
            if (setup == null || setup.ArrowSprite == null) return;

            if (!isVisible || vectorValue.sqrMagnitude < 0.0001f)
            {
                setup.ArrowSprite.gameObject.SetActive(false);
                return;
            }

            setup.ArrowSprite.gameObject.SetActive(true);

            Vector3 direction = vectorValue.normalized;

            float offsetDistance = (projectileSize / 2f) + 0.1f;

            setup.ArrowSprite.transform.position = centerPosition + (direction * offsetDistance);

            setup.ArrowSprite.transform.up = direction;

            float visualLength = vectorValue.magnitude * setup.LengthMultiplier * _settings.ScaleLength;
            Vector2 currentSize = setup.ArrowSprite.size;
            setup.ArrowSprite.size = new Vector2(currentSize.x, Mathf.Clamp(visualLength, setup.MinLength, setup.MaxLength));
        }

        public void ClearAll()
        {
            if(_velocitySetup != null) _velocitySetup.ArrowSprite.gameObject.SetActive(false);
            if (_accelerationSetup != null) _accelerationSetup.ArrowSprite.gameObject.SetActive(false);
            if (_totalForceSetup != null) _totalForceSetup.ArrowSprite.gameObject.SetActive(false);
        }
    }
}
