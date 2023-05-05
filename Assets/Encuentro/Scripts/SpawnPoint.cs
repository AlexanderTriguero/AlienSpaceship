using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SpawnPoint : MonoBehaviour
{
    [Header("Spawned enemy info")]
    [SerializeField] GameObject[] enemyPrefabs;
    [SerializeField] Transform route;

    [SerializeField]  float spawnTime = 10f;
    float TimeForNextSpawn;

    bool onVision = false;
    float randomRange;

    void Start()
    {
        randomRange = enemyPrefabs.Length;
        TimeForNextSpawn = 0f;
    }

    GameObject spawnedEnemy=null;
    void Update()
    {
        if (!onVision)
        {
            if (TimeForNextSpawn<=0)
            {
                TimeForNextSpawn = spawnTime;
                float enemyID= Random.Range(0f, randomRange);
                for (int i=0;i<enemyPrefabs.Length;i++)
                {
                    if (enemyID>=i && enemyID<i+1)
                    {
                        spawnedEnemy = Instantiate(enemyPrefabs[i], transform.position, transform.rotation,this.gameObject.transform);
                        spawnedEnemy.GetComponent<NavigateRute>().SetRoute(route);
                        spawnedEnemy.GetComponent<Enemigo>().SetBehaviour(Enemigo.BehaviourType.Valiant);
                        GetComponentInParent<Encuentro>();
                    }
                }
            }
            else
            {
                TimeForNextSpawn = TimeForNextSpawn - Time.deltaTime;
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer)=="Player")
        {
            onVision = true;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (LayerMask.LayerToName(other.gameObject.layer) == "Player")
        {
            onVision = false;
        }
    }
}
