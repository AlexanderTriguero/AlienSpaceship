using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class EndOfLevel : MonoBehaviour
{
    [SerializeField] protected LayerMask targetLayers = Physics.DefaultRaycastLayers;
    [SerializeField] public UINavigation uINavigation;
    [SerializeField] GameObject canvas;
    [SerializeField] AudioSource musica;
    [SerializeField] protected UnityEvent ponerNaveEnMarcha;

    [SerializeField] float showWindowTime=0f;

    void Awake()
    {
        Time.timeScale = 1;
    }

    bool playerOnTrigger = false;
    float currentShowWindowTime = 0f;
    void Update()
    {
        if (playerOnTrigger)
        {
            if (currentShowWindowTime >= showWindowTime)
            {
                uINavigation.activePanel(canvas);
                musica.Stop();
                Time.timeScale = 0;
            }
            else
            {
                currentShowWindowTime += Time.deltaTime;
            }
        }
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.layer== LayerMask.NameToLayer("Player"))
        {
            playerOnTrigger = true;
            ponerNaveEnMarcha?.Invoke();
        }
    }
}
