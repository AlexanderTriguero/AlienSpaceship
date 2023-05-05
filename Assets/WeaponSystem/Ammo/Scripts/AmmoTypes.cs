using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AmmoTypes : MonoBehaviour
{
    [SerializeField] AmmoTypeForWeapon ammoType;
    public enum AmmoTypeForWeapon
    {
        Pistola,
        Escopeta,
        Metralleta
    }
    // Start is called before the first frame update

    public AmmoTypeForWeapon GetAmmoTypeForWeapon()
    {
        return ammoType;
    }

}
