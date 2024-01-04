using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTable : InteractiveObject
{
    [SerializeField] private FoodTypes orderedFood; //cuando haya gatos esto se cambiara segun su pedido
    private FoodTypes playerFoodType;
    private FoodController tableFoodController;
    private FoodController playerFoodController;

    private void Start()
    {
        tableFoodController = GetComponent<FoodController>();
    }

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FoodController>() != null)
        {
            playerFoodController = other.GetComponent<FoodController>();
        }

        else if (other.GetComponent<ClientData>() != null)
        {
            orderedFood = other.GetComponent<ClientData>().foodOrdered;
        }

        base.OnTriggerEnter(other);
    }
    protected override void Interaction()
    {
        playerFoodType = playerFoodController.foodType;

        if(playerFoodType == orderedFood)
        {
            tableFoodController.foodType = orderedFood;

            playerFoodController.EnableFoodGO(false);
            tableFoodController.EnableFoodGO(true);
        }
        else
        {
            Debug.Log("Este no es el pedido");
        }
    }
}
