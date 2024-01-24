using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientStates : MonoBehaviour
{
    [SerializeField] private float maxSeconds;
    private float secondsToAnnoyed;
    private float secondsToAngry;
    private float secondsToLeave;

    [SerializeField] private CatMovement catMovement;
    [SerializeField] private Transform leaveTransform;

    public CatState catState;
    public bool isFed = false;

    private void OnEnable()//activado por ChairTrigger
    {
        CalculateSeconds();
        StateChange(CatState.Happy);
    }

    private void CalculateSeconds()
    {
        secondsToAnnoyed = 0.3f * maxSeconds;
        secondsToAngry = 0.4f * maxSeconds;
        secondsToLeave = 0.3f * maxSeconds;
    }

    private void StateChange(CatState catStateChange)
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

        Debug.Log(catState);
    }

    public IEnumerator ChangeStateTimer(float seconds)
    {
        Debug.Log(seconds);
        yield return new WaitForSeconds(seconds);

        if(!isFed)
        {
            StateChange(catState + 1);
        }

    }
}
