using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableData : MonoBehaviour
{
    public bool isOcupied;
    public Transform selectedChair;
    public FoodTypes orderedFood;
    [SerializeField] private List<Transform> chairs;

    private void Start()
    {
        TableIsFree();
    }

    public void TableIsFree()
    {
        orderedFood = FoodTypes.Nothing;//de momento aqui, casi mejor cuando gato "termina" de comer
        isOcupied = false;
        RandomChair();
    }

    private void RandomChair()
    {
        selectedChair = chairs[Random.Range(0, chairs.Count)];
    }
}
