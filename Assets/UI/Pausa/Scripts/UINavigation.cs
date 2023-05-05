using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UINavigation : MonoBehaviour
{
   public void cargarEscena(string sceneName){
       SceneManager.LoadScene(sceneName);
   }

   public void activePanel(GameObject activeCanvas){
       activeCanvas.SetActive(true);

   }
   public void deactivePanel(GameObject activeCanvas){
       activeCanvas.SetActive(false);

   }
   
    public void Salir()
    {
        Application.Quit();
    }
}
