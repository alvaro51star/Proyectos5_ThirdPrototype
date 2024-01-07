using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class TableData : MonoBehaviour
{
    //public event Action OnAvailableTable;
    public TablesManager tablesManager;
    public bool isOcupied;
    public Transform selectedChair;
    public FoodTypes orderedFood;
    public FurnitureTheme furnitureTheme;
    [SerializeField] private List<Transform> chairs;

    private void Start()
    {
        TableIsFree();
    }

    public void TableIsFree()
    {
        orderedFood = FoodTypes.Nothing;//de momento aqui, casi mejor cuando gato "termina" de comer
        //isOcupied = false;
        RandomChair();
        if(!isOcupied)
        {
            //OnAvailableTable?.Invoke();
        }
    }

    private void RandomChair()
    {
        selectedChair = chairs[Random.Range(0, chairs.Count)];
    }
}
