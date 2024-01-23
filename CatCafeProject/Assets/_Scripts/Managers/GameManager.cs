using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Date Variables")]
    public int currentDay = 0;
    public int week = 0;

    [Space]
    [Header("Modes Variables")]
    [SerializeField] private GameObject CafeteriaMode;
    [SerializeField] private GameObject DecorationMode;
    [SerializeField] private NavMeshSurface navMeshSurface;
    public GameModes initialGameMode;

    public static event Action<GameModes> OnGameModeChange;
    public static event Action<List<CatDataSO>> OnCatListCreated;
    [Space]
    [Header("Cats Variables")]
    [SerializeField] private int maxCatsPerDay = 6;
    public List<CatDataSO> catDataList;
    public List<CatDataSO> catsForTheDay;

    [SerializeField] private float easyCatsPercentage = 0.6f;
    [SerializeField] private float normalCatsPercentage = 0.4f;
    [SerializeField] private float hardCatsPercentage = 0f;
    [SerializeField] private float richCatsPercentage = 0f;

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

        ChangeDay();
        ChangeWeekNumber();
    }


    void Start()
    {
        
        ChangeGameMode(initialGameMode);
        SetCatsForTheDay();
    }

    public void ChangeGameMode(GameModes gameMode)
    {
        switch (gameMode)
        {
            case GameModes.Decoration:
                DecorationMode?.SetActive(true);
                CafeteriaMode?.SetActive(false);
                //FurnitureManager.instance.ResetFurnitureManagerData();
                break;
            case GameModes.Cafeteria:
                //FurnitureManager.instance.SetFurnitureData();
                DecorationMode?.SetActive(false);
                CafeteriaGameMode();
                break;

        }
        OnGameModeChange?.Invoke(gameMode);
    }

    private void CafeteriaGameMode()
    {
        if (!DecorationMode.activeSelf)
        {
            navMeshSurface.BuildNavMesh();
            CafeteriaMode?.SetActive(true);
        }
    }

    public void SetCatsForTheDay()
    {
        catsForTheDay.Clear();

        int catsNumber;

        int easyCatNumber;
        int normalCatNumber;
        int hardCatNumber;
        int richCatNumber;

        if (IsSeventhDay())
        {

            ChangeCatNumberPerWeek();
            ChangeCatPercentagePerWeek();

            catsNumber = Random.Range(maxCatsPerDay - 2, maxCatsPerDay + 1);
            easyCatNumber = Mathf.RoundToInt(catsNumber * 0.1f);
            normalCatNumber = Mathf.RoundToInt(catsNumber * 0.1f);
            hardCatNumber = Mathf.RoundToInt(catsNumber * 0.5f);
            richCatNumber = Mathf.RoundToInt(catsNumber * 0.3f);
        }
        else
        {
            catsNumber = Random.Range(maxCatsPerDay - 2, maxCatsPerDay + 1);
            easyCatNumber = Mathf.RoundToInt(catsNumber * easyCatsPercentage);
            normalCatNumber = Mathf.RoundToInt(catsNumber * normalCatsPercentage);
            hardCatNumber = 0;
            richCatNumber = 0;
        }

        Debug.Log($"Cats this day = {catsNumber} , Easy Cats = {easyCatNumber} , Normal Cats = {normalCatNumber} , Hard Cats = {hardCatNumber} , Rich Cats = {richCatNumber}");

        for (int i = 0; i < easyCatNumber; i++)
        {
            catsForTheDay.Add(catDataList[Random.Range(0, 3)]);
        }
        for (int i = 0; i < normalCatNumber; i++)
        {
            catsForTheDay.Add(catDataList[Random.Range(3, 5)]);
        }
        for (int i = 0; i < hardCatNumber; i++)
        {
            catsForTheDay.Add(catDataList[Random.Range(5, 7)]);
        }
        for (int i = 0; i < richCatNumber; i++)
        {
            catsForTheDay.Add(catDataList[7]);
        }

        catsForTheDay = catsForTheDay.OrderBy(i => Guid.NewGuid()).ToList();
        OnCatListCreated?.Invoke(catsForTheDay);
    }

    private bool IsSeventhDay()
    {
        if (currentDay % 7 == 0)
        {
            return true;
        }
        return false;
    }

    private void ChangeCatPercentagePerWeek()
    {
        if (week % 2 != 0 && week % 3 != 0)
            return;


        if (week % 2 == 0)
        {
            easyCatsPercentage -= 0.1f;
            normalCatsPercentage += 0.1f;
        }

        if (week % 3 == 0)
        {
            easyCatsPercentage += 0.05f;
            normalCatsPercentage -= 0.05f;
        }

        Math.Clamp(easyCatsPercentage, 0f, 1f);
        Math.Clamp(normalCatsPercentage, 0f, 1f);
    }

    private void ChangeCatNumberPerWeek()
    {
        maxCatsPerDay += Random.Range(4, 7);
    }

    private void ChangeWeekNumber()
    {
        week = (currentDay / 7) + 1;
    }

    public void ChangeDay()
    {
        currentDay++;
    }
}
