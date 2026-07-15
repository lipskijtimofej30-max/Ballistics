using Game.Scripts.Core;
using System;
using UnityEngine;

namespace Assets.Game.Scripts.View.View
{
    public class VectorRenderer : MonoBehaviour
    {
        [Serializable]
        public class VectorSetup
        {
            public bool IsVisible;
            public SpriteRenderer ArrowSprite;
            public float LengthMultiplier = 1f;
            public float MinLength = 0.5f;
            public float MaxLength = 10f;
        }

        [SerializeField] private VectorSetup _velocitySetup;
        [SerializeField] private VectorSetup _accelerationSetup;
        [SerializeField] private VectorSetup _totalForceSetup;

        public void UpdateVectors(ProjectileState state)
        {
            UpdateArrow(_velocitySetup, state.Velocity, state.Position, state.Size);
            UpdateArrow(_accelerationSetup, state.Acceleration, state.Position, state.Size);
            UpdateArrow(_totalForceSetup, state.TotalForce, state.Position, state.Size);
        }

        private void UpdateArrow(VectorSetup setup, Vector3 vectorValue, Vector3 centerPosition, float projectileSize)
        {
            if (setup == null || setup.ArrowSprite == null) return;

            if (!setup.IsVisible || vectorValue.sqrMagnitude < 0.0001f)
            {
                setup.ArrowSprite.gameObject.SetActive(false);
                return;
            }

            setup.ArrowSprite.gameObject.SetActive(true);

            Vector3 direction = vectorValue.normalized;

            float offsetDistance = (projectileSize / 2f) + 0.1f;

            setup.ArrowSprite.transform.position = centerPosition + (direction * offsetDistance);

            setup.ArrowSprite.transform.up = direction;

            float visualLength = vectorValue.magnitude * setup.LengthMultiplier;
            Vector2 currentSize = setup.ArrowSprite.size;
            setup.ArrowSprite.size = new Vector2(currentSize.x, Mathf.Clamp(visualLength, setup.MinLength, setup.MaxLength));
        }
    }
}
