using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTable : InteractiveObject
{
    [SerializeField] private FoodController tableFoodController;
    [SerializeField] private TableData tableData;    
    private FoodTypes playerFoodType;
    private FoodController playerFoodController;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FoodController>() != null)
        {
            playerFoodController = other.GetComponent<FoodController>();
        }

        if (other.GetComponent<ClientData>() != null)
        {
            tableData.orderedFood = other.GetComponent<ClientData>().foodOrdered;
        }

        base.OnTriggerEnter(other);
    }
    protected override void Interaction()//take ordered food
    {
        playerFoodType = playerFoodController.foodType;

        if(playerFoodType == tableData.orderedFood)
        {
            tableFoodController.foodType = tableData.orderedFood;

            playerFoodController.EnableFoodGO(false);
            tableFoodController.EnableFoodGO(true);
        }
        else
        {
            Debug.Log("Este no es el pedido");
            //sonido "no"
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
        tableData.TableIsFree();
    }
}
