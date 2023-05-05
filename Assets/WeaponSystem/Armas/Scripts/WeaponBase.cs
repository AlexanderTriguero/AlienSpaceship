using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public abstract class WeaponBase : MonoBehaviour
{
    [Header("Layer Info")]
    [SerializeField] protected LayerMask targetLayers = Physics.DefaultRaycastLayers;

    [Header("Min Range delivers max Damage")]
    [SerializeField] float minRange = 1f;
    [SerializeField] float maxDamage = 1f;

    [Header("Max Range delivers min Damage")]
    [SerializeField] float maxRange = 25f;
    [SerializeField] float minDamage = 0.25f;

    [Header("Ammo")]
    [SerializeField] protected GameObject ammoPrefab;

    protected bool isUsable = true;

    [SerializeField] protected bool isPlayerWeapon = false;

    public enum WeaponUseType
    {
        Swing,
        Shot,
        ContinuousShoot,
        Undefined,
    };

    public virtual WeaponUseType GetUseType() { return WeaponUseType.Undefined; }

    public virtual void Swing()
    {

    }

    public virtual void Shot()
    {
        Debug.Log("Shot called in WeaponBase");
    }

    public virtual void StartShooting()
    {

    }

    public virtual void StopShooting()
    {

    }

    protected float CalcDamage(Vector3 hitPosition)
    {
        return CalcDamage(Vector3.Distance(transform.position, hitPosition));
    }

    protected float CalcDamage(float distance)
    {
        if (distance < minRange) { return maxDamage; }
        if (distance > maxRange) { return 0f; }
        float finalDamage = Mathf.Lerp(maxDamage, minDamage, (distance - minRange) / (maxRange - minRange));
        return finalDamage;
    }

    public float GetMaxRange() { return maxRange; }
    public float GetMinRange() { return minRange; }

    [Header("Ammo, Magazines, reload")]
    [SerializeField] int maxAmmo = 100;
    [SerializeField] int currentAmmo = 24;
    [SerializeField] int ammoInCurrentMagazine = 12;
    [SerializeField] int magazineCapacity = 12;
    [SerializeField] float reloadTime = 5f;
    [SerializeField] bool consumesAmmo = true;
    protected bool isReloading;
    [SerializeField] UnityEvent onReload;

    protected enum UseAmmoResult
    {
        ShotMade,
        NeedsReload,
        NoAmmo,
    };

    protected UseAmmoResult UseAmmo()
    {
        if (currentAmmo == 0) return UseAmmoResult.NoAmmo;
        if (ammoInCurrentMagazine == 0) return UseAmmoResult.NeedsReload;

        if (consumesAmmo) currentAmmo--;
        ammoInCurrentMagazine--;
        return UseAmmoResult.ShotMade;
    }

    internal void AddAmmo()
    {
        currentAmmo += magazineCapacity;
    }

    public bool HasAmmo() { return currentAmmo > 0; }
    public bool NeedsReload() { return HasAmmo() && (ammoInCurrentMagazine == 0); }

    public bool HasMaxAmmo() { return currentAmmo >= maxAmmo; }

    public virtual void Reload()
    {
        if (isUsable)
        {
            isUsable = false;
            isReloading = true;
            Invoke(nameof(ReloadAfterSeconds), reloadTime);
        }
    }

    void ReloadAfterSeconds()
    {
        isUsable = true;
        isReloading = false;
        ammoInCurrentMagazine = Mathf.Min(magazineCapacity, currentAmmo);
        if (isPlayerWeapon)
        {
            onReload.Invoke();
        }
    }

    public GameObject GetAmmoPrefab()
    {
        return ammoPrefab;
    }
    public int GetCurrentAmmo()
    {
        return currentAmmo;
    }
    public int GetammoInCurrentMagazine()
    {
        return ammoInCurrentMagazine;
    }
    public int GetAmmoOutOfMagazine()
    {
        return currentAmmo-ammoInCurrentMagazine;
    }

    public bool IsPlayerWeapon()
    {
        return isPlayerWeapon;
    }

    public bool IsUsable()
    {
        return isUsable;
    }
}
