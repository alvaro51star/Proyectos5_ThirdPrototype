using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private GameObject foodGO;
    [SerializeField] private float secondsToSpawn;
    [SerializeField] private AudioClipsNames audioClip;

    private void Start()
    {
        foodGO.SetActive(true);
    }

    //timer en corutina para evitar update, TENER CUIDADO EN PAUSA
    private IEnumerator SpawnTimer (float seconds)
    {        
        yield return new WaitForSecondsRealtime(seconds);
        foodGO.SetActive(true);

        SoundManager.instance.ReproduceSound(audioClip);
    }

    public void DisableSpawnedFood()
    {
        foodGO.SetActive(false);

        StartCoroutine(SpawnTimer(secondsToSpawn));
    }
}
