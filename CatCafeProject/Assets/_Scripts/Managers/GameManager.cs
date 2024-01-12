using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public int currentDay = 0;

    [SerializeField] private GameObject CafeteriaMode;
    [SerializeField] private GameObject DecorationMode;
    [SerializeField] private NavMeshSurface navMeshSurface;
    public GameModes initialGameMode;

    public static event Action<GameModes> OnGameModeChange;
    [Space]
    [Header("Cats Variables")]
    [SerializeField] private int catsPerDay = 3;
    public List<CatDataSO> catDataList;
    public List<CatDataSO> catForTheDay;

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

        for (int i = 0; i < catsPerDay; i++)
        {
            
        }
    }
}
