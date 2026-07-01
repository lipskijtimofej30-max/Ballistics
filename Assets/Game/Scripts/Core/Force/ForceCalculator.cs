using System.Collections.Generic;
using Game.Scripts.Core.Force;
using UnityEngine;
using Zenject;

namespace Game.Scripts.Core
{
    public class ForceCalculator
    {
        private List<IForce> _forces = new();
        private EnvironmentSettings _environmentSettings;

        [Inject]
        private void Construct(List<IForce> forces, EnvironmentSettings environmentSettings)
        {
            _forces = forces;
            _environmentSettings = environmentSettings;
            Debug.Log($"Force count: {forces.Count}");
        }

        public Vector3 CalculateTotalForce(Projectile projectile)
        {
            Vector3 total =  Vector3.zero;
            foreach (var force in _forces)
            {
                if (!_environmentSettings.AirResistanceEnabled)
                    if (force is DragForce df)
                    {
                        Debug.Log($"Drag force dont calculate");
                        continue;
                    }
                
                total +=  force.Calculate(projectile);
            }
            return total;
        }
    }
}