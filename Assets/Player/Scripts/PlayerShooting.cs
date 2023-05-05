using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Linq;

public class PlayerShooting : MonoBehaviour
{
    WeaponBase[] availableWeapons;
    WeaponBase currentWeapon;
    Animator[] weaponAnimator;

    [SerializeField] int currentWeaponIndex = 0;
    [SerializeField] GameObject weaponInterfaceObject;
    WeaponInterface[] weaponInterfaces;
    PlayerWithLife playerWithLife;

    private void Awake()
    {
        availableWeapons = GetComponentsInChildren<WeaponBase>(true);
        weaponInterfaces = weaponInterfaceObject.GetComponentsInChildren<WeaponInterface>();
        playerWithLife = GetComponent<PlayerWithLife>();
    }

    // Start is called before the first frame update
    void Start()
    {
        SelectCurrentWeapon(currentWeaponIndex);
        for (int i=0;i<availableWeapons.Length;i ++)
        {
            weaponInterfaces[i].UpdateValues(availableWeapons[i].GetammoInCurrentMagazine(),availableWeapons[i].GetAmmoOutOfMagazine());
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (playerWithLife.IsAlive())
        {
            if (!UIPausa.pausado)
            {
                // Inicio / final de disparo
                if (Input.GetKeyDown(KeyCode.Mouse0))
                {
                   /* if (!currentWeapon.NeedsReload())
                    {
                        if (currentWeapon.HasAmmo())
                        {*/
                            currentWeapon.Shot();
                            currentWeapon.StartShooting();
                   /*     }
                    }*/
                }

                if (Input.GetKeyUp(KeyCode.Mouse0))
                { currentWeapon.StopShooting(); }

                // Usar arma al estilo melee
                if (Input.GetKeyDown(KeyCode.Mouse1))
                { currentWeapon.Swing(); }

                // Recarga
                if (Input.GetKeyDown(KeyCode.R))
                {
                    if (currentWeapon.IsUsable())
                    {
                        if (currentWeapon.HasAmmo())
                        {
                            currentWeapon.Reload();
                        }
                    }
                }


                // Cambio de arma
                if (currentWeapon.IsUsable())
                {
                    if (Input.GetKeyDown(KeyCode.Alpha1))
                    {
                        currentWeaponIndex--;
                        SelectCurrentWeapon(currentWeaponIndex);
                    }
                    if (Input.GetKeyDown(KeyCode.Alpha2))
                    {
                        currentWeaponIndex++;
                        SelectCurrentWeapon(currentWeaponIndex);
                    }
                }
            }
        }
    }

    void SelectCurrentWeapon(int weaponIndex)
    {
        if (weaponIndex < 0) { currentWeaponIndex = availableWeapons.Length - 1; }
        else if (weaponIndex >= availableWeapons.Length) { currentWeaponIndex = 0; }

        currentWeapon?.StopShooting();
        currentWeapon = availableWeapons[currentWeaponIndex];

        for (int i=0; i<availableWeapons.Length;i++)
        {
            availableWeapons[i].gameObject.SetActive(i == currentWeaponIndex);
            weaponInterfaces[i].gameObject.SetActive(i == currentWeaponIndex);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Ammo"))
        {
            AmmoTypes.AmmoTypeForWeapon colliderAmmoType = other.GetComponent<AmmoTypes>().GetAmmoTypeForWeapon();
            for (int i=0;i<availableWeapons.Length;i++)
            {
                if (!availableWeapons[i].HasMaxAmmo())
                {
                    if (availableWeapons[i].GetAmmoPrefab().GetComponent<AmmoTypes>().GetAmmoTypeForWeapon()== colliderAmmoType) {
                        availableWeapons[i].AddAmmo();
                        weaponInterfaces[i].UpdateValues(availableWeapons[i].GetammoInCurrentMagazine(), availableWeapons[i].GetAmmoOutOfMagazine());
                        Destroy(other.gameObject);
                    }
                }
            }
        }
    }

    public void RefreshCurrentWeaponAmmo() {
        weaponInterfaces[currentWeaponIndex].UpdateValues(availableWeapons[currentWeaponIndex].GetammoInCurrentMagazine(), availableWeapons[currentWeaponIndex].GetAmmoOutOfMagazine());

    }

}
