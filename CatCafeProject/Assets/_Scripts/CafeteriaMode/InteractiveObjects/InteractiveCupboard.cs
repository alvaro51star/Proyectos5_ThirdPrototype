using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FoodSpawner))]
public class InteractiveCupboard : InteractiveObject
{
    public static event Action OnFoodEnabled;

    [SerializeField] private FoodTypes foodType;

    protected override void OnTriggerEnter(Collider other)
    {
        base.OnTriggerEnter(other);

        if(other.GetComponent<PlayerFoodController>() != null)
        {
            PlayerFoodController foodController = other.GetComponent<PlayerFoodController>();
            foodController.foodType = foodType;
            FoodSpawner.OnTakeFood += foodController.TakeFood;            
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);

        if (other.GetComponent<PlayerFoodController>() != null)
        {
            FoodSpawner.OnTakeFood -= other.GetComponent<PlayerFoodController>().TakeFood;
        }
    }
    protected override void Interaction()
    {
        OnFoodEnabled?.Invoke();//usado en FoodSpawner
    }
}
