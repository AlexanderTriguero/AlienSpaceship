using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class UIPausa : MonoBehaviour
{
    [SerializeField] GameObject pausa;
    public static bool pausado=false;
    private bool menuAbierto=false;
    [SerializeField] PlayerWithLife playerWithLife;
    bool activarCursor=true;

    [SerializeField] FinDeNivel endOfLevel;
    // Start is called before the first frame update
    void Start()
    {
    }

    // Update is called once per frame
    void Update()
    {
        if (playerWithLife.IsAlive() && !endOfLevel.GameEnded())
        {
            if (Input.GetButtonDown("Cancel"))
            {

                if (!pausado)
                {
                    Cursor.lockState = CursorLockMode.None;
                    Cursor.visible = true;
                    pausado = true;
                    pausa.SetActive(true);
                    Time.timeScale = 0;
                }
                else
                {
                    if (!menuAbierto)
                    {
                        resumen();
                    }
                }
            }
        }
        else
        {
            if (activarCursor)
            {
                activarCursor = false;
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
        }
    }

    public void activePanel(GameObject activeCanvas){
       activeCanvas.SetActive(true);
       menuAbierto=true;
    }
    public void deactivePanel(GameObject activeCanvas){
       activeCanvas.SetActive(false);
       menuAbierto=false;

   }
   public void resumen(){
        pausado=false;
        pausa.SetActive(false);
        Time.timeScale = 1;
        Cursor.visible = false;
        Cursor.lockState = CursorLockMode.Locked;


    }
    public void changeMS(Slider mSlider){
       
       menuAbierto=true;
   }
    public void Salir()
    {
        Application.Quit();
    }

}
