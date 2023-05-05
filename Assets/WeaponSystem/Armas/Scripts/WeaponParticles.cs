using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponParticles : WeaponBase
{
    new ParticleSystem particleSystem;
    ParticleSystem.EmissionModule emissionModule;
    [SerializeField] public float particleDamage;

    private void Awake()
    {
        particleSystem = GetComponentInChildren<ParticleSystem>();
        emissionModule = particleSystem.emission;
        emissionModule.enabled = false;
    }

    public override void StartShooting()
    {
        emissionModule.enabled = true;
    }

    public override void StopShooting()
    {
        emissionModule.enabled = false;
    }
}
