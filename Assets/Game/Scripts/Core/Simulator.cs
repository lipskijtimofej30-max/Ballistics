using System;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class Simulator : MonoBehaviour
    {
        private ForceCalculator _forceCalculator;
        private ProjectileFactory _projectileFactory;
        private Projectile _currentProjectile;

        [field:SerializeField] public bool IsActive { get; private set; } = false;
        
        [Inject]
        private void Construct(ForceCalculator forceCalculator, ProjectileFactory projectileFactory)
        {
            _forceCalculator = forceCalculator;
            _projectileFactory = projectileFactory;
        }
        
        [ContextMenu("Spawn")]
        public void Spawn()
        {
            _currentProjectile = _projectileFactory.Create();
        }
        
        private void FixedUpdate()
        {
            if (!IsActive) return;
            if(_currentProjectile == null) return;
            
            var f_total = _forceCalculator.CalculateForceD(_currentProjectile) + _forceCalculator.CalculateForceG(_currentProjectile);
            var acceleration = f_total /_currentProjectile.Mass;
            var v_new = _currentProjectile.Rigidbody.velocity + acceleration * Time.fixedDeltaTime;
            //var x_new = _currentProjectile.transform.position + v_new * Time.fixedDeltaTime;
            _currentProjectile.Rigidbody.velocity = v_new;
            Debug.Log($"Force: {f_total}, acceleration: {acceleration}, v: {v_new}, projectile position: {_currentProjectile.transform.position}");
        }
    }
}