using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class ClientStates : MonoBehaviour
{
    [SerializeField] private float maxSecondsPatience;
    [SerializeField] private float secondsEating;
    private float secondsToAnnoyed;
    private float secondsToAngry;
    private float secondsToLeave;

    [SerializeField] private CatMovement catMovement;
    [SerializeField] private ClientData clientData;
    [SerializeField] private Transform leaveTransform;

    [SerializeField] private AudioClip audioClipAngryLeave;
    [SerializeField] private AudioSource audioSource;

    public CatState catState;
    public bool isFed = false;

    private void OnEnable()//activado por ChairTrigger
    {
        CalculateSeconds();
        TimeStateChange(CatState.Happy);
    }

    private void CalculateSeconds()
    {
        secondsToAnnoyed = 0.3f * maxSecondsPatience;
        secondsToAngry = 0.4f * maxSecondsPatience;
        secondsToLeave = 0.3f * maxSecondsPatience;
    }

    private void TimeStateChange(CatState catStateChange)
    {
        catState = catStateChange;

        switch(catState)
        {
            case CatState.Happy:
                StartCoroutine(ChangeStateTimer(secondsToAnnoyed));
                break;
            case CatState.Annoyed:
                StartCoroutine(ChangeStateTimer(secondsToAngry));
                break;
            case CatState.Angry:
                StartCoroutine(ChangeStateTimer(secondsToLeave));
                break;
            case CatState.Leaving:
                catMovement.MovementToDestination(leaveTransform);
                SoundManager.instance.ReproduceSound(audioClipAngryLeave, audioSource);
                break;
        }

    }

    public IEnumerator ChangeStateTimer(float seconds)
    {        
        yield return new WaitForSeconds(seconds);

        if(!isFed)
        {
            TimeStateChange(catState + 1);
        }
        else
        {
            StartCoroutine(Eating());
        }
    }


    private IEnumerator Eating()
    {
        yield return new WaitForSeconds(secondsEating);

        FurnitureTheme furnitureTheme = catMovement.tableAssigned.furnitureTheme;
        CatState catDecorationState = CalculateDecorationState();
        int money = EconomyManager.instance.CalculateTotalMoney(clientData.catType, catState, catDecorationState, clientData.foodOrdered, furnitureTheme);
        EconomyManager.instance.ModifyCurrentMoney(money);

        Debug.Log("cliente se ha comido su pedido");
        TimeStateChange(CatState.Leaving);
    }

    private CatState CalculateDecorationState()
    {
        int likeness = 0;

        foreach(var item in clientData.catType.likes)
        {
            switch(item)
            {
                case FurnitureTheme.Flowers:
                    if(FurnitureManager.instance.flowerFurniturePercentage >= 50)
                    {
                        likeness++;
                    }
                    else if(FurnitureManager.instance.flowerFurniturePercentage <= 20)
                    {
                        likeness--;
                    }
                    break;
                case FurnitureTheme.Hearts:
                    if (FurnitureManager.instance.heartFurniturePercentage >= 50)
                    {
                        likeness++;
                    }
                    else if (FurnitureManager.instance.heartFurniturePercentage <= 20)
                    {
                        likeness--;
                    }
                    break;
                case FurnitureTheme.Leaves:
                    if (FurnitureManager.instance.leavesFurniturePercentage >= 50)
                    {
                        likeness++;
                    }
                    else if (FurnitureManager.instance.leavesFurniturePercentage <= 20)
                    {
                        likeness--;
                    }
                    break;
                case FurnitureTheme.Fishes:
                    if (FurnitureManager.instance.fishFurniturePercentage >= 50)
                    {
                        likeness++;
                    }
                    else if (FurnitureManager.instance.fishFurniturePercentage <= 20)
                    {
                        likeness--;
                    }
                    break;
            }            
        }

        if (likeness > 0)
        {
            return CatState.Happy;
        }
        else if (likeness == 0)
        {
            return CatState.Annoyed;
        }
        else
        {
            return CatState.Angry;
        }

    }
}
