using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [Header("Percentages")]
    [SerializeField] private float GOOD_TABLE_PERCENT = 0.35f;
    [SerializeField] private float BAD_TABLE_PERCENT = 0.25f;
    [SerializeField] private float TIME_PERCENT = 0.25f;
    [SerializeField] private float DECORATION_PERCENT = 0.35f;
    [Space]
    [Header("Multipliers")]
    [SerializeField] private float NORMAL_MULTIPLIER = 1.1f;
    [SerializeField] private float HARD_MULTIPLIER = 1.25f;
    [SerializeField] private float RICH_MULTIPLIER = 1.5f;


    public static EconomyManager instance;

    [Space]
    [Header("Money")]
    [SerializeField] private int defaultMoney = 100;
    [SerializeField] private int currentMoney = 0;

    public static event Action<int> OnMoneyUpdated;

    [Space]
    [Header("Food Value List")]
    public List<FoodTypeValueTupla> foodTypeValues;

    //TODO hacer una lista de los precios del dia para incluirlos en el recibo

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

    private void Start()
    {
        if (PlayerPrefs.HasKey("CurrentMoney"))
        {
            currentMoney = PlayerPrefs.GetInt("CurrentMoney");
        }
        else
        {
            currentMoney = defaultMoney;
            PlayerPrefs.SetInt("CurrentMoney", currentMoney);
        }
    }

    private void Update()
    {
        Debug.Log("Current money: " + currentMoney);
    }

    public void ModifyCurrentMoney(int value)
    {
        currentMoney += value;
        OnMoneyUpdated?.Invoke(currentMoney);
    }

    public int GetCurrentMoney()
    {
        return currentMoney;
    }

    public int CalculateTotalMoney(CatDataSO catData, CatState catStateTime, CatState catStateDecoration, FoodTypes foodType, FurnitureTheme tableTheme)
    {
        if (catData.difficulty == CatDifficulty.Rich)
        {
            return CalculateTip(catData, catStateTime, catStateDecoration, foodType, tableTheme) + GetFoodValue(foodType);
        }
        else
        {
            return CalculateTip(catData, catStateTime, catStateDecoration, foodType) + GetFoodValue(foodType);
        }
    }

    public int CalculateTip(CatDataSO catData, CatState catStateTime, CatState catStateDecoration, FoodTypes foodType)
    {
        int totalTip = 0;
        int foodValue = GetFoodValue(foodType);

        totalTip = CalculateCatStateTip(foodValue * TIME_PERCENT, catStateTime) + CalculateCatStateTip(foodValue * DECORATION_PERCENT, catStateDecoration);

        switch (catData.difficulty)
        {
            case CatDifficulty.Easy:
                totalTip *= 1;
                break;
            case CatDifficulty.Normal:
                totalTip *= (int)NORMAL_MULTIPLIER;
                break;
            case CatDifficulty.Hard:
                totalTip *= (int)HARD_MULTIPLIER;
                break;
            case CatDifficulty.Rich:
                totalTip *= (int)RICH_MULTIPLIER;
                break;
        }

        return totalTip;
    }

    public int CalculateTip(CatDataSO catData, CatState catStateTime, CatState catStateDecoration, FoodTypes foodType, FurnitureTheme tableTheme)
    {
        if (catData.difficulty != CatDifficulty.Rich)
        {
            return CalculateTip(catData, catStateTime, catStateDecoration, foodType);
        }

        int totalTip = 0;
        int foodValue = GetFoodValue(foodType);

        if (tableTheme == FurnitureTheme.Fishes)
        {
            totalTip += (int)(foodValue * 0.5f);
            totalTip += CalculateCatStateTip(foodValue * GOOD_TABLE_PERCENT, catStateTime);
        }
        else
        {
            totalTip += CalculateCatStateTip(foodValue * BAD_TABLE_PERCENT, catStateTime);
        }

        return totalTip;
    }

    private int CalculateCatStateTip(float maxTip, CatState catState)
    {
        switch (catState)
        {
            case CatState.Happy:
                return (int)maxTip;
            case CatState.Annoyed:
                return (int)(maxTip * 0.5f);
            case CatState.Angry:
                return 0;
            default:
                return 0;
        }
    }

    private int GetFoodValue(FoodTypes foodType)
    {
        for (int i = 0; i < foodTypeValues.Count; i++)
        {
            if (foodTypeValues[i].foodType == foodType)
            {
                return foodTypeValues[i].prize;
            }
        }
        return 0;
    }
}

[Serializable]
public struct FoodTypeValueTupla
{
    public FoodTypes foodType;
    public int prize;
}
