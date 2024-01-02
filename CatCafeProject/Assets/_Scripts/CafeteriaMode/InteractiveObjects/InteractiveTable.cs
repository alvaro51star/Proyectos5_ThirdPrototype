using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTable : InteractiveObject
{
    private FoodTypes foodType;
    private PlayerFoodController tableFoodController;
    private PlayerFoodController playerFoodController;

    private void Start()
    {
        tableFoodController = GetComponent<PlayerFoodController>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerFoodController>() != null)
        {
            playerFoodController = other.GetComponent<PlayerFoodController>();
        }
        base.OnTriggerEnter(other);
    }
    protected override void Interaction()
    {
        foodType = playerFoodController.foodType;
        tableFoodController.foodType = foodType;

        playerFoodController.EnableFoodGO(false);
        tableFoodController.EnableFoodGO(true);
    }
}
