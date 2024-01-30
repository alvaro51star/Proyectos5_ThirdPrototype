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

    [SerializeField] private GameObject imageGO_l;
    [SerializeField] private GameObject imageGO_r;

    public CatState catState;
    public bool isFed = false;

    private void OnEnable()//activado por ChairTrigger
    {
        CalculateSeconds();
        TimeStateChange(CatState.Happy);
        leaveTransform = FindAnyObjectByType<ClientDestroyer>().transform;
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
                ChangeState(secondsToAnnoyed);
                break;
            case CatState.Annoyed:
                imageGO_l.SetActive(true);
                ChangeState(secondsToAngry);
                break;
            case CatState.Angry:
                imageGO_r.SetActive(true);
                ChangeState(secondsToLeave);
                break;
            case CatState.Leaving:
                clientData.bocadillo.SetActive(false);
                catMovement.m_sit = false;
                catMovement.MovementToDestination(leaveTransform);
                break;
        }

    }

    public void ChangeState(float seconds)
    {
        if (isFed)
        {
            StopAllCoroutines();
            StartCoroutine(Eating());
        }

        else
        {
            StartCoroutine(ChangeStateTimer(seconds));
        }

    }
    private IEnumerator ChangeStateTimer(float seconds)
    {
        yield return new WaitForSeconds(seconds);

        if (catState + 1 == CatState.Leaving)
        {
            SoundManager.instance.ReproduceSound(audioClipAngryLeave, audioSource);
        }

        TimeStateChange(catState + 1);
    }


    private IEnumerator Eating()
    {
        yield return new WaitForSeconds(secondsEating);

        FurnitureTheme furnitureTheme = catMovement.tableAssigned.furnitureTheme;
        CatState catDecorationState = CalculateDecorationState();
        int money = EconomyManager.instance.CalculateTotalMoney(clientData.catType, catState, catDecorationState, clientData.foodOrdered, furnitureTheme);
        EconomyManager.instance.ModifyCurrentMoney(money);

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
