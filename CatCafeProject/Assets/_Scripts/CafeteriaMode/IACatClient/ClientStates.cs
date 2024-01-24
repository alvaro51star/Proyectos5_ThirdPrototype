using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Cinemachine.DocumentationSortingAttribute;

public class ClientStates : MonoBehaviour
{
    [SerializeField] private float maxSeconds;
    private float secondsToAnnoyed;
    private float secondsToAngry;
    private float secondsToLeave;

    [SerializeField] private CatMovement catMovement;
    [SerializeField] private ClientData clientData;
    [SerializeField] private Transform leaveTransform;

    public CatState catState;
    public bool isFed = false;

    private void OnEnable()//activado por ChairTrigger
    {
        CalculateSeconds();
        TimeStateChange(CatState.Happy);
    }

    private void CalculateSeconds()
    {
        secondsToAnnoyed = 0.3f * maxSeconds;
        secondsToAngry = 0.4f * maxSeconds;
        secondsToLeave = 0.3f * maxSeconds;
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
                break;
        }

        //Debug.Log(catState);
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
        yield return new WaitForSeconds(5f);

        FurnitureTheme furnitureTheme = catMovement.tableAssigned.furnitureTheme;
        //CatState catDecorationState = CalculateDecorationState();
        //int d = EconomyManager.instance.CalculateTotalMoney(clientData.catType, catState, catDecorationState,clientData.foodOrdered, furnitureTheme);
        Debug.Log("cliente se ha comido su pedido");
        TimeStateChange(CatState.Leaving);
    }

    /*private CatState CalculateDecorationState()
    {
        int likeness = 0;
        //si mas de 50 esta contento
        //si menos de 50% esta serio
        //si menos de 80% esta enfadado

        //FurnitureManager.instance.

        foreach(var item in clientData.catType.likes)
        {
            switch(item)
            {
                case FurnitureTheme.Flowers:
                    Furn
                    break;
                case FurnitureTheme.Hearts:
                    break;
                case FurnitureTheme.Leaves:
                    break;
                case FurnitureTheme.Fishes:
                    break;
            }
        }
        //si le gusta mas de un tema como lo hago???



    }*/
}
