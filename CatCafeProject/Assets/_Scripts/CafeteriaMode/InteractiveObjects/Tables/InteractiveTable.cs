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
    [SerializeField] private AudioClip popBocadilloClip;
    private FoodTypes playerFoodType;
    private FoodController playerFoodController;
    private ClientStates clientStates;
    private ClientData clientData;

    protected override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<CatMovement>() && other.GetComponent<CatMovement>().tableAssigned == tableData)
        {
            if (other.GetComponent<FoodController>())
            {
                playerFoodController = other.GetComponent<FoodController>();
            }

            if (other.GetComponent<ClientData>())
            {
                clientData = other.GetComponent<ClientData>();

                tableFoodController.foodType = clientData.foodOrdered;

                clientData.bocadilloFoodController.EnableFoodGO(true);
                clientData.bocadillo.SetActive(true);
                SoundManager.instance.ReproduceSound(popBocadilloClip, tableAudioSource);
            }

            if (other.GetComponent<ClientStates>())
            {
                clientStates = other.GetComponent<ClientStates>();
            }
        }

        if(other.GetComponent<FoodController>())
        {
            playerFoodController = other.GetComponent<FoodController>();
        }

        if ((tableFoodController) && (tableFoodController.foodType != FoodTypes.Nothing))
        {
            base.OnTriggerEnter(other);
        }
    }
    protected override void Interaction()//take ordered food
    {        
        playerFoodType = playerFoodController.foodType;

        if(playerFoodType == tableFoodController.foodType)
        {
            playerFoodController.EnableFoodGO(false);
            tableFoodController.EnableFoodGO(true);

            playerFoodController.foodType = FoodTypes.Nothing;

            clientStates.isFed = true;//to stop state change            
            clientData?.bocadillo.SetActive(false);
            SoundManager.instance.ReproduceSound(orderedFoodClip, tableAudioSource);
        }
        else
        {
            SoundManager.instance.ReproduceSound(notOrderedFoodClip, tableAudioSource);            
        }
    }

    protected override void OnTriggerExit(Collider other)
    {
        base.OnTriggerExit(other);
    }
}
