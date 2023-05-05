using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;
using UnityEngine.Events;

public class WeaponRaycast : WeaponMelee
{
    [Header("Weapon info")]
    [SerializeField] Transform shootPoint;
    [SerializeField] float scatterAngle = 0f;
    [SerializeField] float shotCadence = 5f;
    [SerializeField] float projectilesPerShot = 1f;
    ParticleSystem shootParticleSystem;
    [FormerlySerializedAs("canShoot")]
    [SerializeField] bool canShootOnce;
    [SerializeField] bool canShootContinuously;
    //[SerializeField] float shotDamage = 0.2f;

    [Header("Debug")]
    [SerializeField] bool debugShot;

    [Header("Sounds")]
    [SerializeField] AudioSource shotSound;
    [SerializeField] AudioSource reloadSound;
    [SerializeField] float reloadSoundTimeStart=0f;
    [SerializeField] AudioSource outOfAmmoSound;

    bool isShootingContinuously;

    NoiseMaker noiseMaker;

    [SerializeField] UnityEvent onShot;

    private void OnValidate()
    {
        if (debugShot)
        {
            Shot();
            debugShot = false;
        }
    }

    private void Awake()
    {
        noiseMaker = GetComponentInChildren<NoiseMaker>();
        animator = GetComponentInChildren<Animator>();
        shootParticleSystem = GetComponentInChildren<ParticleSystem>();
    }

    public override WeaponUseType GetUseType()
    {
        return WeaponUseType.Shot;
    }

    float timeForNextShot = 0f;
    private void Update()
    {
        timeForNextShot -= Time.deltaTime;
        timeForNextShot = timeForNextShot > 0f ? timeForNextShot : 0f;
        if (isShootingContinuously)
        {
            InternalShot();
        }
    }

    public override void Shot()
    {
        if (canShootOnce) { InternalShot(); }
    }

    protected void InternalShot()
    {
        if (!isReloading && timeForNextShot <= 0f)
        {
            if (UseAmmo() == UseAmmoResult.ShotMade)
            {
                timeForNextShot += 1f / shotCadence;
                shootParticleSystem?.Play();
                shotSound?.Play();
                noiseMaker?.MakeNoise();
                RaycastHit hit;
                if (isPlayerWeapon)
                {
                    onShot.Invoke();
                    animator.SetTrigger("Shot");
                }
                for (int i = 0; i < projectilesPerShot; i++)
                {
                    float horizontalScatterAngle = Random.Range(-scatterAngle, scatterAngle);
                    Quaternion horizontalScatter = Quaternion.AngleAxis(horizontalScatterAngle, shootPoint.up);
                    float verticalScatterAngle = Random.Range(-scatterAngle, scatterAngle);
                    Quaternion verticalScatter = Quaternion.AngleAxis(verticalScatterAngle, shootPoint.right);

                    Vector3 shotForward = verticalScatter * (horizontalScatter * shootPoint.forward);

                    if (Physics.Raycast(shootPoint.position, shotForward, out hit, Mathf.Infinity, targetLayers, QueryTriggerInteraction.Ignore))
                    {
                        Debug.DrawLine(shootPoint.position, hit.point, Color.cyan, 10f);
                        Debug.DrawRay(hit.point, hit.normal, Color.red, 10f);

                        TargetBase targetBase = hit.collider.GetComponent<TargetBase>();

                        targetBase?.NotifyShot(CalcDamage(targetBase.transform.position));
                    }
                }
            }
            else
            {
                if (isPlayerWeapon)
                {
                    timeForNextShot += 1f / shotCadence;
                    outOfAmmoSound?.Play();
                }            
            }
        }
    }

    public override void StartShooting()
    {
        isShootingContinuously = canShootContinuously;
    }

    public override void StopShooting()
    {
        isShootingContinuously = false;
    }
    

    public override void Reload()
    {
        if (isPlayerWeapon) {
            animator.SetTrigger("Reload");
            reloadSound.PlayDelayed(reloadSoundTimeStart);
        }
        base.Reload();
    }
}
