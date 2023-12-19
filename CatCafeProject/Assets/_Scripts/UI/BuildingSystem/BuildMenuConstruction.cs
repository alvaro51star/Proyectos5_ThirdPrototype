using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuildMenuConstruction : MonoBehaviour
{
    [SerializeField] private ObjectsDatabaseSO database;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform panelTransform;
    [SerializeField] private PlacementSystem placementSystem;
    

    private void Awake()
    {
        foreach (var item in database.objectData)
        {
            GameObject newButton = Instantiate(buttonPrefab, panelTransform);
            newButton.GetComponent<ButtonData>().AssignData(item, placementSystem);
        }
    }
}