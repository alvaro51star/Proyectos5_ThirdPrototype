using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    [SerializeField] private GameObject foodGO;
    [SerializeField] private AudioClip audioClip;
    [SerializeField] private AudioSource audioSource;
    public bool foodIsActive;

    private void Start()
    {
        foodGO.SetActive(true);
        foodIsActive = true;
    }

    private IEnumerator SpawnTimer (float seconds)
    {        
        yield return new WaitForSeconds(seconds);
        foodGO.SetActive(true);
        foodIsActive = true;

        SoundManager.instance.ReproduceSound(audioClip, audioSource);
    }

    public void DisableSpawnedFood(float seconds)
    {
        foodGO.SetActive(false);
        foodIsActive=false;
        StartCoroutine(SpawnTimer(seconds));
    }
}
