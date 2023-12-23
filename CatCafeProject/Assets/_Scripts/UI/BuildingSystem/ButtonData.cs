using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class ButtonData : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI furnitureName;
    [SerializeField] private TextMeshProUGUI furniturePrize;
    [SerializeField] private Button button;
    private int id = -1;
    //[SerializeField] private PlacementSystem placementSystem;
    [SerializeField] private Image image; //!Por implementar


    public ButtonData(ObjectData item)
    {
        furnitureName.text = item.Name;
        furniturePrize.text = item.Prize.ToString();
    }

    void Start()
    {
        button.onClick.AddListener(TaskOnClick);
    }

    void TaskOnClick()
    {
        if (id >= 0)
        {
            //placementSystem.StartPlacement(id);
        }
    }

    // public void AssignData(ObjectData item, PlacementSystem placementSystem)
    // {
    //     furnitureName.text = item.Name;
    //     furniturePrize.text = item.Prize.ToString();
    //     this.placementSystem = placementSystem;
    // }
}
