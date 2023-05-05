using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Nave : MonoBehaviour
{
    // Start is called before the first frame update

    [SerializeField] GameObject propulsorDeDespegue;
    [SerializeField] float velocidadDeDespegue = 1f;

    [SerializeField] GameObject propulsorDeArranque;
    [SerializeField] float timePropulsorDeArranque=3f;
    [SerializeField] float velocidadDeArranque = 3f;
    [SerializeField] GameObject player;
    [SerializeField] GameObject playerCamera;

    bool enMarcha=false;
    bool propulsoresEnMarcha = false;
    float time = 0f;

    private void Update()
    {
        if (enMarcha)
        {
            
            if(time>= timePropulsorDeArranque)
            {
                if (!propulsoresEnMarcha) {
                    propulsorDeArranque?.SetActive(true);
                    propulsorDeDespegue?.SetActive(false);
                }
                transform.position = new Vector3(transform.position.x - velocidadDeArranque * Time.deltaTime, transform.position.y, transform.position.z);
            }
            else
            {
                time += Time.deltaTime;
                transform.position = new Vector3(transform.position.x, transform.position.y + (velocidadDeDespegue*Time.deltaTime), transform.position.z);
            }

        }
    }
    public void Despegar()
    {
        propulsorDeDespegue?.SetActive(true);
        enMarcha=true;
        playerCamera.SetActive(false);
        player.SetActive(false);

    }
}
