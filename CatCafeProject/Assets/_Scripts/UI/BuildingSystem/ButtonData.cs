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
    [SerializeField] private int id = -1;
    [SerializeField] private UIPlacementController placementController;
    [SerializeField] private Image image;
    [SerializeField] private Image themeImage;


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
            placementController.SelectObjectWithIndex(id);
        }
    }

    public void AssignData(ItemData item, UIPlacementController placementController)
    {
        id = item.ID;
        furniturePrize.text = item.buyValue.ToString();
        this.placementController = placementController;
        if (item.image != null)
        {
            image = item.image;
        }

        themeImage = placementController.themeImages[(int)item.furnitureTheme];
    }
}
