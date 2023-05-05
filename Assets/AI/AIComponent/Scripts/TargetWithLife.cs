using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class TargetWithLife : TargetBase
{
    public enum DamageType
    {
        Shot,
        Swing,
        Explosion,
        Particle,
    }
    public struct DeathInfo
    {
        public DamageType type;

        public Vector3 direction;

        public Vector3 explosionPosition;
        public float explosionRadius;
    }

    [SerializeField] protected float life = 1f;
    [SerializeField] protected float maxLife = 1f;
    [SerializeField] protected float medikitLifeRecovery = 5f;
    [SerializeField] public UnityEvent<TargetWithLife, float> onLifeLost;
    [SerializeField] public UnityEvent<TargetWithLife, DeathInfo> onDeath;
    [SerializeField] protected AudioSource damageSound;
    [SerializeField] protected AudioSource deathSound;
    Animator targetAnimator;


    protected DeathInfo deathInfo = new DeathInfo();

    public override void NotifyShot(float damage)
    {
        LoseLife(DamageType.Shot, damage);
    }

    public override void NotifySwing(float damage)
    {
        LoseLife(DamageType.Swing, damage);
    }

    public override void NotifyExplosion(float damage)
    {
        LoseLife(DamageType.Explosion, damage);
    }

    public override void NotifyParticle(float damage)
    {
        LoseLife(DamageType.Particle, damage);
    }

    protected virtual void LoseLife(DamageType damageType, float howMuch)
    {
        deathInfo.type = damageType;
        switch (deathInfo.type)
        {
            case DamageType.Shot:
            case DamageType.Swing:
            case DamageType.Particle:
                deathInfo.direction = transform.position - PlayerMovement.instance.transform.position;
                break;
            case DamageType.Explosion:
                deathInfo.explosionPosition = Explosion.lastExplosionPosition;
                deathInfo.explosionRadius = Explosion.lastExplosionRadius;
                break;
        }

        life -= howMuch;
        onLifeLost.Invoke(this, life);
        CheckStillAlive();
    }

    protected virtual void CheckStillAlive()
    {
        if (life <= 0f)
        {
            deathSound?.Play();
            if (DestroyOnAllLifeLost()) {
                Destroy(gameObject); 
            }
            onDeath.Invoke(this, deathInfo);
        }
        else
        {
            damageSound?.Play();
        }
    }

    protected virtual bool DestroyOnAllLifeLost()
    {
        return true;
    }

    
}
