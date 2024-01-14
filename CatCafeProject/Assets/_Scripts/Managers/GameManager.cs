using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentDay = 0;
    public int week = 0;

    [SerializeField] private GameObject CafeteriaMode;
    [SerializeField] private GameObject DecorationMode;
    [SerializeField] private NavMeshSurface navMeshSurface;
    public GameModes initialGameMode;

    public static event Action<GameModes> OnGameModeChange;
    [Space]
    [Header("Cats Variables")]
    [SerializeField] private int maxCatsPerDay = 6;
    public List<CatDataSO> catDataList;
    public List<CatDataSO> catsForTheDay;

    [SerializeField] private float easyCatsPercentage = 0.6f;
    [SerializeField] private float normalCatsPercentage = 0.4f;
    [SerializeField] private float hardCatsPercentage = 0f;

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


    void Start()
    {
        week = currentDay / 7; //TODO PROBAR ESTO A VER SI FUNCIONA
        ChangeGameMode(initialGameMode);
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

        int catsNumber      = Random.Range(maxCatsPerDay - 2, maxCatsPerDay + 1);
        int easyCatNumber   = (int)(catsNumber * easyCatsPercentage);
        int normalCatNumber = (int)(catsNumber * normalCatsPercentage);
        int hardCatNumber   = (int)(catsNumber * hardCatsPercentage);
        //TODO REVISAR LOS VALORES DE LOS BUCLES FOR PARA VER SI COINCIDEN CON LOS DATOS DE LOS GATOS
        for (int i = 0; i < easyCatNumber; i++)
        {
            catsForTheDay.Add(catDataList[Random.Range(0, 3)]);
        }
        for (int i = 0; i < normalCatNumber; i++)
        {
            catsForTheDay.Add(catDataList[Random.Range(3, 7)]);
        }
        for (int i = 0; i < hardCatNumber; i++)
        {
            catsForTheDay.Add(catDataList[Random.Range(7, 9)]);
        }
    }
}
