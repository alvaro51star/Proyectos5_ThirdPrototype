using DG.Tweening;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveTable : InteractiveObject
{
    [HideInInspector] public FoodController tableFoodController;
    [SerializeField] private TableData tableData;
    [SerializeField] private AudioSource tableAudioSource;
    [SerializeField] private AudioClip orderedFoodClip;
    [SerializeField] private AudioClip notOrderedFoodClip;
    private FoodTypes playerFoodType;
    private FoodController playerFoodController;
    private ClientStates clientStates;

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

        if(other.GetComponent<ClientStates>())
        {
            clientStates = other.GetComponent<ClientStates>();
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

            playerFoodController.foodType = FoodTypes.Nothing;
            tableData.orderedFood = FoodTypes.Nothing;

            clientStates.isFed = true;//to stop state change

            SoundManager.instance.ReproduceSound(orderedFoodClip, tableAudioSource);
        }
        else
        {
            Debug.Log("Este no es el pedido");
            SoundManager.instance.ReproduceSound(notOrderedFoodClip, tableAudioSource);            
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
