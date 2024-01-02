using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(FoodSpawner))]
public class InteractiveCupboard : InteractiveObject
{
    public static event Action OnFoodEnabled;

    protected override void Interaction()
    {
        OnFoodEnabled?.Invoke();//condiciones dentro de FoodSpawner
        Debug.Log("Player está interactuando con InteractiveCupboard");
    }
}
