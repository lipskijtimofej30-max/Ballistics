using Game.Scripts.Core;
using System;
using Game.Scripts.Settings;
using UnityEngine;
using Zenject;
using ILogger = Game.Scripts.Infrastructure.Logger.ILogger;

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
        [SerializeField] private VectorSetup _velocityXSetup;
        [SerializeField] private VectorSetup _velocityYSetup;
        [SerializeField] private VectorSetup _accelerationSetup;
        [SerializeField] private VectorSetup _totalForceSetup;
        
        private VectorVisualizationSettings _settings;
        private ILogger _logger;

        [Inject]
        private void Construct(VectorVisualizationSettings visualizationSettings, ILogger logger)
        {
            _settings = visualizationSettings;
            _logger = logger;
            ClearAll();
        }

        public void UpdateVectors(ProjectileState state)
        {
            UpdateVectors(state.Position, state.Velocity, state.Acceleration, state.TotalForce, state.Size);
        }

        public void UpdateVectors(Vector3 position, Vector3 velocity, Vector3 acceleration, Vector3 netForce, float projectileSize)
        {
            if (_settings == null) return; 

            UpdateArrow(_velocitySetup, velocity, position, projectileSize, _settings.IsActiveVelocity);
            UpdateArrow(_accelerationSetup, acceleration, position, projectileSize, _settings.IsActiveAcceleration);
            UpdateArrow(_totalForceSetup, netForce, position, projectileSize, _settings.IsActiveTotalForce);
            
            Vector3 velocityX = new Vector3(velocity.x, 0f, 0f);
            Vector3 velocityY = new Vector3(0f, velocity.y, 0f);
            
            UpdateArrow(_velocityXSetup, velocityX, position, projectileSize, _settings.IsActiveVelocity && _settings.IsActiveProjectionVelocity);
            UpdateArrow(_velocityYSetup, velocityY, position, projectileSize, _settings.IsActiveVelocity && _settings.IsActiveProjectionVelocity);
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
            if (_velocitySetup?.ArrowSprite) _velocitySetup.ArrowSprite.gameObject.SetActive(false);
            if (_velocityXSetup?.ArrowSprite) _velocityXSetup.ArrowSprite.gameObject.SetActive(false);
            if (_velocityYSetup?.ArrowSprite) _velocityYSetup.ArrowSprite.gameObject.SetActive(false);
            if (_accelerationSetup?.ArrowSprite) _accelerationSetup.ArrowSprite.gameObject.SetActive(false);
            if (_totalForceSetup?.ArrowSprite) _totalForceSetup.ArrowSprite.gameObject.SetActive(false);
        }
    }
}
