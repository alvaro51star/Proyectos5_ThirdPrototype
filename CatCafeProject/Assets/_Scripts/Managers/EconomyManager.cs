using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EconomyManager : MonoBehaviour
{
    public static EconomyManager instance;

    [SerializeField] private int defaultMoney = 100;
    [SerializeField] private int currentMoney = 0;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
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
        }
    }

    public void ModifyCurrentMoney(int value)
    {
        currentMoney += value;
    }
}
