using System.Collections.Generic;
using Game.Scripts.Core.Force;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class ForceCalculator
    {
        private List<IForce> _forces = new();

        [Inject]
        private void Construct(List<IForce> forces)
        {
            _forces = forces;
            Debug.Log($"Force count: {forces.Count}");
        }

        public Vector3 CalculateTotalForce(Projectile projectile)
        {
            Vector3 total =  Vector3.zero;
            foreach (var force in _forces)
                total +=  force.Calculate(projectile);
            return total;
        }
    }
}