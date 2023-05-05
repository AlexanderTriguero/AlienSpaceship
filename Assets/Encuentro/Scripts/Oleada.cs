using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Oleada : MonoBehaviour
{
    Enemigo[] enemigos;
    SpawnPoint[] spawnPoints;

    private void Awake()
    {
        enemigos = GetComponentsInChildren<Enemigo>();
        spawnPoints = GetComponentsInChildren<SpawnPoint>();
    }
    internal void deactivateAllEnemies()
    {
        foreach (Enemigo e in enemigos)
        {
            e.gameObject.SetActive(false);
        }
        deactiveAllSpawnPoints();
    }

    internal void deactiveAllSpawnPoints()
    {
        foreach (SpawnPoint s in spawnPoints)
        {
            s.gameObject.SetActive(false);
        }
    }

    internal void activeAllEnemies()
    {
        foreach (Enemigo e in enemigos)
        {
            e.gameObject.SetActive(true);
        }
        foreach (SpawnPoint s in spawnPoints)
        {
            s.gameObject.SetActive(true);
        }
    }

    internal bool areAllEnemiesDead()
    {
        bool allEnemiesAreDead = true;

        for(int i=0; allEnemiesAreDead && (i < enemigos.Length); i++)
        {
            allEnemiesAreDead = enemigos[i] == null;
        }
        return allEnemiesAreDead;
    }

    internal void updateEnemies()
    {
        enemigos = GetComponentsInChildren<Enemigo>(); 
    }
}
