using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ClientData : MonoBehaviour
{
    public GameObject bocadillo;
    public FoodController bocadilloFoodController;
    public CatDataSO catType;
    public FoodTypes foodOrdered;

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

        bocadilloFoodController.foodType = foodOrdered;
    }
}
