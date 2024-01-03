using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    [SerializeField] private float GOOD_TABLE_PERCENT = 0.35f;
    [SerializeField] private float BAD_TABLE_PERCENT = 0.25f;


    public static EconomyManager instance;

    [SerializeField] private int defaultMoney = 100;
    [SerializeField] private int currentMoney = 0;

    public static event Action<int> OnMoneyUpdated;
    public List<FoodTypeValueTupla> foodTypeValues;

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

    public int CalculateTip(CatDataSO catData, CatState catState, FoodTypes foodType)
    {


        return 0;
    }

    public int CalculateTip(CatDataSO catData, CatState catState, FoodTypes foodType, FurnitureTheme tableTheme)
    {
        if (catData.difficulty != CatDifficulty.Rich)
        {
            return CalculateTip(catData, catState, foodType);
        }

        int totalTip = 0;
        int foodValue = GetFoodValue(foodType);

        if (tableTheme == FurnitureTheme.Fishes)
        {
            totalTip += (int)(foodValue * 0.5f);
            totalTip += CalculateCatStateTip(GetFoodValue(foodType) * GOOD_TABLE_PERCENT, catState);
        }
        else
        {
            totalTip += CalculateCatStateTip(GetFoodValue(foodType) * BAD_TABLE_PERCENT, catState);
        }

        totalTip += foodValue;
        return totalTip;
    }

    private int CalculateCatStateTip(float maxTimeTip, CatState catState)
    {
        switch (catState)
        {
            case CatState.Happy:
                return (int)maxTimeTip;
            case CatState.Annoyed:
                return (int)(maxTimeTip * 0.5f);
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
