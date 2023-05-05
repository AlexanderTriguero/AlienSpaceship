using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponInstantiate : WeaponBase 
{
    [SerializeField] GameObject prefabProyectil;
    [SerializeField] Transform shootPoint;
    [SerializeField] float forceToApplyOnShot=300f;
    [SerializeField] protected AudioSource shotSound;

    public override void Shot()
    {    
        shotSound?.Play();
        GameObject proyectil = Instantiate(prefabProyectil, shootPoint.position, shootPoint.rotation);
        proyectil.GetComponent<Rigidbody>()?.AddForce(shootPoint.forward * forceToApplyOnShot);    
    }

    public override void Swing()
    {
        base.Swing();
    }

    public override void StartShooting()
    {
        base.StartShooting();
    }
    public override void StopShooting()
    {
        base.StopShooting();
    }


}
