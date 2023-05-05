using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FinDeNivel : MonoBehaviour
{

    [SerializeField] GameObject player;
    [SerializeField] AudioSource musica;
    [SerializeField] GameObject interfaz;

    bool gameEnded = false;
    void Awake()
    {
        gameEnded = true;
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;
        musica.Stop();
        interfaz.SetActive(false);
        player.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {

    }
    public bool GameEnded(){
        return gameEnded;
    }
        
}
