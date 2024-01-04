using System.Collections;
using System.Collections.Generic;
using Unity.AI.Navigation;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    [SerializeField] private GameObject     CafeteriaMode;
    [SerializeField] private GameObject     DecorationMode;
    [SerializeField] private NavMeshSurface navMeshSurface;

    // Start is called before the first frame update
    void Start()
    {
       ChangeGameMode(GameModes.Cafeteria);//esto obviamente no ira aqui
    }

    public void ChangeGameMode(GameModes gameMode)
    {
        switch (gameMode)
        {
            case GameModes.Decoration:
                DecorationMode?.SetActive(true);
                CafeteriaMode?.SetActive(false);
                break;
            case GameModes.Cafeteria:
                DecorationMode?.SetActive(false);
                CafeteriaGameMode();
                break;
        }
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
