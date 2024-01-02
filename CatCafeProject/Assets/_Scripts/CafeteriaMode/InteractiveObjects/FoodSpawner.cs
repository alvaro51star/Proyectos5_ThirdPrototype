using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class FoodSpawner : MonoBehaviour
{
    public static event Action OnTakeFood;//para que el player coja la comida

    [SerializeField] private GameObject foodGO;
    [SerializeField] private float secondsToSpawn;

    private void Start()
    {
        foodGO.SetActive(true);
        InteractiveCupboard.OnFoodEnabled += DisableSpawnedFood; //añadimos funcion para que si pueda desactivarla cuando interactue
    }

    //timer en corutina para evitar update, TENER CUIDADO EN PAUSA
    private IEnumerator SpawnTimer (float seconds)
    {
        yield return new WaitForSeconds(seconds);
        foodGO.SetActive(true);
        if (foodGO.activeSelf)
        {
            InteractiveCupboard.OnFoodEnabled += DisableSpawnedFood;
        }
    }

    private void DisableSpawnedFood()
    {
        Debug.Log("DisableSpawnedFood is available");
        foodGO.SetActive(false);
        if (!foodGO.activeSelf)
        {
            Debug.Log(foodGO.activeSelf);
            InteractiveCupboard.OnFoodEnabled -= DisableSpawnedFood;
            OnTakeFood?.Invoke();
        }
        StartCoroutine(SpawnTimer(secondsToSpawn));
    }
}
