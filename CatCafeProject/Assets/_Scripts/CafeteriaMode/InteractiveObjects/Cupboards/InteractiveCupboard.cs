using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FoodSpawner))]
public class InteractiveCupboard : InteractiveObject
{
    [SerializeField] private FoodTypes foodType;
    [SerializeField] private FoodSpawner foodSpawner;
    [SerializeField] private float secondsToSpawn;
    [SerializeField] private AudioClip audioClipTakeFood;
    [SerializeField] private AudioSource audioSource;
    private FoodController playerFoodController;

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<FoodController>() != null && other.GetComponent<FoodController>().foodType == FoodTypes.Nothing && foodSpawner.foodIsActive)
        {
            playerFoodController = other.GetComponent<FoodController>();

            base.OnTriggerEnter(other);
        }
    }

    protected override void Interaction()
    {
        playerFoodController.foodType = foodType;
        foodSpawner.DisableSpawnedFood(secondsToSpawn);
        playerFoodController.EnableFoodGO(true);

        SoundManager.instance.ReproduceSound(audioClipTakeFood, audioSource);
    }
}
