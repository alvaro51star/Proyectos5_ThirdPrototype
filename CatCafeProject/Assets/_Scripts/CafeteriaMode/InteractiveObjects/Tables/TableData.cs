using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TableData : MonoBehaviour
{
    public FurnitureTheme furnitureTheme;

    public bool isOccupied = false;
    [HideInInspector] public Transform selectedChair;
    [HideInInspector] public Transform selectedFoodTransform;

    [SerializeField] private InteractiveTable interactiveTable;
    [SerializeField] private Transform[] chairs;
    [SerializeField] private GameObject[] foodControllerGO;

    private void Start()
    {
        selectedChair = chairs[0];
    }
    public void ResetTableData(bool tableIsOccupied)
    {
        isOccupied = tableIsOccupied;

        if (!tableIsOccupied)
        {
            interactiveTable.tableFoodController.EnableFoodGO(false);
            interactiveTable.tableFoodController.foodType = FoodTypes.Nothing;
            //RandomChair();
        }

        else
        {
            RandomChair();
        }

        //RandomChair();
    }

    private void RandomChair()
    {
        int random = Random.Range(0, chairs.Length);

        selectedChair = chairs[random];
        selectedFoodTransform = foodControllerGO[random].transform;
        interactiveTable.tableFoodController = foodControllerGO[random].GetComponent<FoodController>();
    }
}
