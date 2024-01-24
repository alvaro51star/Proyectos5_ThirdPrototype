using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureManager : MonoBehaviour
{
    public static FurnitureManager instance;

    public List<GameObject> furnitures;
    public List<GameObject> tableList;
    [SerializeField] private int totalFurnitures;

    [Space]
    [Header("Values of each furniture")]
    [SerializeField] private Dictionary<FurnitureTheme, int> furnitureTypeCountDictionary;


    [Space]
    [SerializeField] private int flowerFurnitureTotal = 0;
    [SerializeField] private int heartFurnitureTotal = 0;
    [SerializeField] private int leavesFurnitureTotal = 0;
    [SerializeField] private int fishFurnitureTotal = 0;
    [SerializeField] private int noThemeFurnitureTotal = 0;

    [Space]
    [Header("Percentages")]
    public float flowerFurniturePercentage;
    public float heartFurniturePercentage;
    public float leavesFurniturePercentage;
    public float fishFurniturePercentage;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }
    }

    // private void OnEnable()
    // {
    //     GameManager.OnGameModeChange += SetUpManagerForGamemode;
    // }

    // private void OnDisable()
    // {
    //     GameManager.OnGameModeChange -= SetUpManagerForGamemode;
    // }

    private void SetUpManagerForGamemode(GameModes gameMode)
    {
        if (gameMode == GameModes.Cafeteria)
        {
            SetFurnitureData();
        }
        else
        {
            ResetFurnitureManagerData();
        }
    }

    private void Start()
    {
        GetFurnitures();
        GetTables();
        CalculateFurnitureCountByTheme();
        CalculateFurniturePercentages();
    }

    public void SetFurnitureData()
    {
        GetFurnitures();
        GetTables();
        CalculateFurnitureCountByTheme();
        CalculateFurniturePercentages();
    }

    private void GetFurnitures()
    {
        //furnitures = new List<GameObject>(FindAnyObjectByType<StructurePlacer>().placedObjects);
        totalFurnitures = furnitures.Count;
    }

    private void GetTables()
    {
        foreach (GameObject furniture in furnitures)
        {
            if (furniture.GetComponent<FurnitureData>().furnitureType == FurnitureType.Table)
            {
                tableList.Add(furniture);
            }
        }
    }

    private void CalculateFurnitureCountByTheme()
    {
        foreach (GameObject item in furnitures)
        {
            if (item.TryGetComponent<FurnitureData>(out FurnitureData data))
            {
                switch (data.furnitureTheme)
                {
                    case FurnitureTheme.None:
                        noThemeFurnitureTotal++;
                        break;
                    case FurnitureTheme.Flowers:
                        flowerFurnitureTotal++;
                        break;
                    case FurnitureTheme.Hearts:
                        heartFurnitureTotal++;
                        break;
                    case FurnitureTheme.Leaves:
                        leavesFurnitureTotal++;
                        break;
                    case FurnitureTheme.Fishes:
                        fishFurnitureTotal++;
                        break;
                }
            }
        }
    }

    private void CalculateFurniturePercentages()
    {
        if (totalFurnitures == 0)
        {
            return;
        }
        flowerFurniturePercentage = ((float)flowerFurnitureTotal / totalFurnitures) * 100;
        heartFurniturePercentage = ((float)heartFurnitureTotal / totalFurnitures) * 100;
        leavesFurniturePercentage = ((float)leavesFurnitureTotal / totalFurnitures) * 100;
        fishFurniturePercentage = ((float)fishFurnitureTotal / totalFurnitures) * 100;
    }

    public void ResetFurnitureManagerData()
    {
        furnitures.Clear();
        tableList.Clear();
        totalFurnitures = 0;

        flowerFurniturePercentage = 0;
        heartFurniturePercentage = 0;
        leavesFurniturePercentage = 0;
        fishFurniturePercentage = 0;

        flowerFurnitureTotal = 0;
        heartFurnitureTotal = 0;
        leavesFurnitureTotal = 0;
        fishFurnitureTotal = 0;
        noThemeFurnitureTotal = 0;
    }
}
