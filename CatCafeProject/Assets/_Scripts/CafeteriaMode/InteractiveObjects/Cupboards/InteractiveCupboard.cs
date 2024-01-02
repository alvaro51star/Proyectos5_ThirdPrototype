using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FoodSpawner))]
public class InteractiveCupboard : InteractiveObject
{
    [SerializeField] private FoodTypes foodType;
    [SerializeField] private FoodSpawner foodSpawner;
    private PlayerFoodController playerFoodController;

    protected override void OnTriggerEnter(Collider other)
    {
        if(other.GetComponent<PlayerFoodController>() != null)
        {
            playerFoodController = other.GetComponent<PlayerFoodController>();        
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
