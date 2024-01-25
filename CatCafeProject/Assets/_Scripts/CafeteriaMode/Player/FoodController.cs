using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FoodController : MonoBehaviour
{
    public FoodTypes foodType;
    [SerializeField] private GameObject milkGO;
    [SerializeField] private GameObject donutGO;
    [SerializeField] private GameObject cupcakeGO;
    [SerializeField] private GameObject cakeGO;    

    public void EnableFoodGO(bool isEnabled)
    {
        switch (foodType)
        {
            case FoodTypes.Milk:
                milkGO.SetActive(isEnabled);
                break;
            case FoodTypes.Donut:
                donutGO.SetActive(isEnabled);
                break;
            case FoodTypes.Cupcake:
                cupcakeGO.SetActive(isEnabled);
                break;
            case FoodTypes.Cake:
                cakeGO.SetActive(isEnabled);
                break;
        }
    }
}
