using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class MoneyTextUpdate : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI moneyText;

    private void Start()
    {
        UpdateText(EconomyManager.instance.GetCurrentMoney());
    }

    private void OnEnable()
    {
        EconomyManager.OnMoneyUpdated += UpdateText;
    }

    private void OnDisable()
    {
        EconomyManager.OnMoneyUpdated -= UpdateText;
    }

    private void UpdateText(int value)
    {
        moneyText.text = $"Money: {value}";
    }
}
