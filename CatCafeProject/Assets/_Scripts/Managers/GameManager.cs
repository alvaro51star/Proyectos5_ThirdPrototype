using System;
using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    [SerializeField] private GameObject     CafeteriaMode;
    [SerializeField] private GameObject     DecorationMode;
    [SerializeField] private NavMeshSurface navMeshSurface;

    public static event Action<GameModes> OnGameModeChange;

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
       ChangeGameMode(GameModes.Decoration);//esto obviamente no ira aqui
    }

    public void ChangeGameMode(GameModes gameMode)
    {
        switch (gameMode)
        {
            case GameModes.Decoration:
                DecorationMode?.SetActive(true);
                CafeteriaMode?.SetActive(false);
                FurnitureManager.instance.ResetFurnitureManagerData();
                break;
            case GameModes.Cafeteria:
                FurnitureManager.instance.SetFurnitureData();
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
}
