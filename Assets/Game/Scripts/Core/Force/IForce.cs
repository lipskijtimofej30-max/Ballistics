using UnityEngine;

namespace Game.Scripts.Core.Force
{
    public interface IForce
    {
        Vector3 Calculate(ProjectileState projectile); 
    }
}