using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Mathematics;
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

    [Space]
    [Header("Receipt variables")]
    [SerializeField]
    private Dictionary<FoodTypes, int> orders = new Dictionary<FoodTypes, int> {
        {FoodTypes.Milk,0},
        {FoodTypes.Donut,0},
        {FoodTypes.Cupcake,0},
        {FoodTypes.Cake,0}
    };

    [SerializeField] private List<int> totalTipsList;

    [Space]
    [Header("Receipt UI Variables")]
    [SerializeField] private Dictionary<FoodTypes, GameObject> foodReceiptTexts = new();
    //[SerializeField] private GameObject receiptObject;
    [SerializeField] private ReceiptManagement receiptManagement;

    [Space]
    [Header("Sound Variables")]
    [SerializeField] private AudioClip moneySound;
    [SerializeField] private AudioSource audioSource;

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

        receiptManagement = FindAnyObjectByType<ReceiptManagement>(FindObjectsInactive.Include);
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

        if (receiptManagement != null)
        {
            foodReceiptTexts.Add(FoodTypes.Milk, receiptManagement._milkText);
            foodReceiptTexts.Add(FoodTypes.Donut, receiptManagement._croissantText);
            foodReceiptTexts.Add(FoodTypes.Cupcake, receiptManagement._cupcakeText);
            foodReceiptTexts.Add(FoodTypes.Cake, receiptManagement._cakeText);
        }
    }

    private void Update()
    {
        //Debug.Log("Current money: " + currentMoney);
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
        if (orders.TryGetValue(foodType, out int value))  //add order to the menu
        {
            orders[foodType] = value + 1;
        }

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

        totalTipsList.Add(totalTip);
        return totalTip;
    }

    public int CalculateTip(CatDataSO catData, CatState catStateTime, CatState catStateDecoration, FoodTypes foodType, FurnitureTheme tableTheme)
    {
        if (catData.difficulty != CatDifficulty.Rich)
        {
            return 0;
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

        totalTipsList.Add(totalTip);
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

    public void PrintReceipt()
    {
        int totalTips = 0;
        int totalFoodMoney = 0;
        int totalMoneyEarnedForTheDay = 0;
        foreach (KeyValuePair<FoodTypes, int> keyValue in orders)
        {
            if (keyValue.Value != 0)
            {
                int foodValue = GetFoodValue(keyValue.Key);
                totalFoodMoney += foodValue * keyValue.Value;
            }
            //Debug.Log(keyValue.Key + " : " + foodValue + " x " + keyValue.Value + " = " + foodValue * keyValue.Value);
            //Debug.Log($"{keyValue.Key}: {foodValue} x {keyValue.Value} = {foodValue * keyValue.Value}");
        }
        foreach (int tip in totalTipsList)
        {
            totalTips += tip;
        }
        totalMoneyEarnedForTheDay = totalFoodMoney + totalTips;
        //TestOrders(); //!Solo sirve para probar la lista, una vez se compruebe, lo borraré
        StartCoroutine(ShowReceipt(totalMoneyEarnedForTheDay, totalTips));
        //Debug.Log(totalTips);
    }

    private IEnumerator ShowReceipt(int totalMoneyEarnedForTheDay, int totalTips)
    {
        receiptManagement._dayText.GetComponent<TextMeshProUGUI>().text = $"Day: {GameManager.instance.currentDay}";
        receiptManagement._weekText.GetComponent<TextMeshProUGUI>().text = $"Week: {GameManager.instance.week}";

        receiptManagement.gameObject.SetActive(true);

        if (foodReceiptTexts.Count == orders.Count)
        {
            foreach (var item in foodReceiptTexts)
            {
                if (orders[item.Key] > 0)
                {
                    item.Value.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{GetFoodValue(item.Key)} x {orders[item.Key]}";
                    item.Value.SetActive(true);
                    SoundManager.instance.ReproduceSound(moneySound, audioSource);
                    yield return new WaitForSeconds(0.5f);
                }
            }
        }

        //receipt._tipsText.GetComponentInChildren<TextMeshProUGUI>().text = $"{totalTips} $";
        receiptManagement._tipsText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{totalTips} $";
        receiptManagement._tipsText.transform.GetChild(0).gameObject.SetActive(true);
        SoundManager.instance.ReproduceSound(moneySound, audioSource);
        yield return new WaitForSeconds(0.5f);
        receiptManagement._totalMoneyText.transform.GetChild(0).GetComponent<TextMeshProUGUI>().text = $"{totalMoneyEarnedForTheDay} $";
        receiptManagement._totalMoneyText.transform.GetChild(0).gameObject.SetActive(true);
        SoundManager.instance.ReproduceSound(moneySound, audioSource);
        yield return new WaitForSeconds(0.5f);

        //TODO Despues faltaria activar un botoncito de siguiente dia

        yield return null;
    }


    private void TestOrders()
    {
        orders = new Dictionary<FoodTypes, int> {
            {FoodTypes.Milk,UnityEngine.Random.Range(0, 3)},
            {FoodTypes.Donut,UnityEngine.Random.Range(0, 3)},
            {FoodTypes.Cupcake,UnityEngine.Random.Range(0, 3)},
            {FoodTypes.Cake,UnityEngine.Random.Range(0, 3)}
        };
    }

    public void ResetTipList()
    {
        totalTipsList.Clear();
    }

    public void ResetOrders()
    {
        orders = new Dictionary<FoodTypes, int> {
            {FoodTypes.Milk,0},
            {FoodTypes.Donut,0},
            {FoodTypes.Cupcake,0},
            {FoodTypes.Cake,0}
        };
    }

    public void ResetDataForNextDay(){
        ResetTipList();
        ResetOrders();
    }

    [Serializable]
    public struct FoodTypeValueTupla
    {
        public FoodTypes foodType;
        public int prize;
    }
}
