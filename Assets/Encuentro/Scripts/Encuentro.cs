using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Encuentro : MonoBehaviour
{
    [SerializeField] GameObject finDeNivel = null;

    Oleada[] oleadas;
    int currentOleada = -1;

    private void Awake()
    {
        oleadas = GetComponentsInChildren<Oleada>();
    }

    private void Start()
    {
        foreach (Oleada o in oleadas)
        {
            o.deactivateAllEnemies();
        }

    }

    // Update is called once per frame
    void Update()
    {
        if ((currentOleada >= 0) && (currentOleada < oleadas.Length)) {
            if (oleadas[currentOleada].areAllEnemiesDead())
            {
                oleadas[currentOleada].deactiveAllSpawnPoints();
                currentOleada++;
                if (currentOleada < oleadas.Length)
                {
                    oleadas[currentOleada].activeAllEnemies();
                }
                else
                {

                    if (finDeNivel != null)
                    {
                        finDeNivel.SetActive(true);
                    }
                }
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && currentOleada < 0)
        {
            currentOleada = 0;
            oleadas[currentOleada].activeAllEnemies();

        }
    }

}
