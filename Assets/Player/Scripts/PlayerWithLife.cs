using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class PlayerWithLife : TargetWithLife
{
    [Header("PlayerValues")]
    [SerializeField] Image barraDeVida;
    [SerializeField] AudioSource music;
    bool isAlive = true;
    protected override void CheckStillAlive()
    {
        barraDeVida.fillAmount = life / maxLife;
        
        if (life <= 0f)
        {
            deathSound.Play();
            if (DestroyOnAllLifeLost()) {
               
                music.Stop();
                isAlive = false;
            }
            onDeath.Invoke(this, deathInfo);
        }
        else
        {
            damageSound?.Play();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Medikit"))
        {
            if (life < maxLife)
            {
                if (maxLife < life + medikitLifeRecovery)
                {
                    life = maxLife;
                }
                else
                {
                    life += medikitLifeRecovery;
                }
                barraDeVida.fillAmount = life / maxLife;
                Destroy(other.gameObject);
            }
        }
    }

    public bool IsAlive()
    {
        return isAlive;
    }
}
