using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class ForceCalculator
    {
        private GameSettings _gameSettings;

        [Inject]
        private void Construct(GameSettings gameSettings)
        {
            _gameSettings = gameSettings;
        }
        
        public Vector3 CalculateForceD(Projectile projectile)
        {
            var v_rel = projectile.Rigidbody.velocity - _gameSettings.WindVelocity;
            float cd = 0f;
            if (_gameSettings.ShapeType == ShapeType.Cube)
                cd = 1.05f;
            else if (_gameSettings.ShapeType == ShapeType.Sphere)
                cd = 0.47f;
            return -0.5f * cd * _gameSettings.WindDensity * projectile.CrossSectionalArea * v_rel.magnitude *  v_rel;
        }

        public Vector3 CalculateForceG(Projectile projectile)
        {
            return projectile.Mass * _gameSettings.G;
        }
    }
}