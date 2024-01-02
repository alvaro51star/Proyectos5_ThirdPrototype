using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager instance;

    [SerializeField] private int defaultMoney = 100;
    [SerializeField] private int currentMoney = 0;

    public static event Action<int> OnMoneyUpdated;

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
}
