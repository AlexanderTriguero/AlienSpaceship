using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class WeaponInterface : MonoBehaviour
{

    TMP_Text[] weaponsAmmo;
    // Start is called before the first frame update

    void Awake()
    {
        weaponsAmmo = GetComponentsInChildren<TMP_Text>();
    }

    public void UpdateValues(int magazineAmmo, int ammoOutOfMagazine)
    {
        weaponsAmmo[0].text = magazineAmmo.ToString();
        weaponsAmmo[2].text = ammoOutOfMagazine.ToString();

    }
}
