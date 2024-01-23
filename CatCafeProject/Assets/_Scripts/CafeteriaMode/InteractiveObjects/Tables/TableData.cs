using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TableData : MonoBehaviour
{
    //public event Action OnAvailableTable;
    public TablesManager tablesManager;
    public bool isOcupied = false;
    [HideInInspector] public Transform selectedChair;
    [HideInInspector] public Transform selectedFoodTransform;
    public FoodTypes orderedFood;
    public FurnitureTheme furnitureTheme;
    [SerializeField] private InteractiveTable interactiveTable;
    [SerializeField] private Transform[] chairs;
    [SerializeField] private Transform[] foodTransforms;
    [SerializeField] private FoodController[] foodControllers;

    private void Start()
    {
        selectedChair = chairs[0];
    }
    public void ResetTableData(bool freeOrOcupied)
    {
        orderedFood = FoodTypes.Nothing;//de momento aqui, casi mejor cuando gato "termina" de comer
        isOcupied = freeOrOcupied;

        /*if(isOcupied)
            isOcupied= false;
        if(!isOcupied)
        {
            //OnAvailableTable?.Invoke();
        }*/
        RandomChair();
    }

    private void RandomChair()
    {
        int random = Random.Range(0, chairs.Length);

        selectedChair = chairs[random];
        selectedFoodTransform = foodTransforms[random];
        interactiveTable.tableFoodController = foodControllers[random];
    }
}
