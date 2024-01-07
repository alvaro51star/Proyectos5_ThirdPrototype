using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTable : InteractiveObject
{
    [HideInInspector] public FoodController tableFoodController;
    [SerializeField] private TableData tableData;    
    private FoodTypes playerFoodType;
    private FoodController playerFoodController;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FoodController>())
        {
            playerFoodController = other.GetComponent<FoodController>();
        }

        if (other.GetComponent<ClientData>())
        {
            tableData.orderedFood = other.GetComponent<ClientData>().foodOrdered;
            Debug.Log(tableData.orderedFood);
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
            tableData.orderedFood = FoodTypes.Nothing;
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
        if(other.GetComponent<ClientData>())
        {
            //tableData.TableIsFree();
        }
    }
}
