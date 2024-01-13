using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientData : MonoBehaviour
{
    public CatDataSO catType;
    public FoodTypes foodOrdered;//de momento cambiar en editor

    private void Start()
    {
        if (catType.difficulty == CatDifficulty.Rich)
        {
            foodOrdered = FoodTypes.Cake;
        }
        else
        {
            int randomResult = Random.Range(1, 5);
            foodOrdered = randomResult switch
            {
                1 => FoodTypes.Milk,
                2 => FoodTypes.Donut,
                3 => FoodTypes.Cupcake,
                4 => FoodTypes.Cake,
                _ => FoodTypes.Nothing,
            };
        }
    }
}
