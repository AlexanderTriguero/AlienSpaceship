using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class TargetBase : MonoBehaviour
{
    public abstract void NotifyShot(float damage);
    public abstract void NotifySwing(float damage);
    public abstract void NotifyExplosion(float damage);
    public abstract void NotifyParticle(float damage);

    private void OnParticleCollision(GameObject other)
    {
        WeaponParticles weaponParticles = other.GetComponentInParent<WeaponParticles>();
        if (weaponParticles)
        {
            NotifyParticle(weaponParticles.particleDamage);
        }
    }
}
