using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FoodSpawner))]
public class InteractiveCupboard : InteractiveObject
{
    [SerializeField] private FoodTypes foodType;
    [SerializeField] private FoodSpawner foodSpawner;
    private FoodController playerFoodController;

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<FoodController>() != null)
        {
            playerFoodController = other.GetComponent<FoodController>();        
        }
        base.OnTriggerEnter(other);
    }

    protected override void Interaction()
    {
        playerFoodController.foodType = foodType;
        foodSpawner.DisableSpawnedFood();
        playerFoodController.EnableFoodGO(true);
    }
}
