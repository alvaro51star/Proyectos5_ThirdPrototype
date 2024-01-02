using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTable : InteractiveObject
{
    [SerializeField] private FoodTypes orderedFood; //cuando haya gatos esto se cambiara segun su pedido
    private FoodTypes playerFoodType;
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
        playerFoodType = playerFoodController.foodType;

        if(playerFoodType == orderedFood)
        {
            tableFoodController.foodType = playerFoodType;

            playerFoodController.EnableFoodGO(false);
            tableFoodController.EnableFoodGO(true);
        }
        else
        {
            Debug.Log("Este no es el pedido");
        }
    }
}
