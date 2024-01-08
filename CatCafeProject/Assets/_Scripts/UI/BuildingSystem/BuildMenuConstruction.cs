using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BuildMenuConstruction : MonoBehaviour
{
    [SerializeField] private ItemDataBaseSO dataBase;
    [SerializeField] private GameObject buttonPrefab;
    [SerializeField] private Transform panelTransform;
    [SerializeField] private UIPlacementController placementController;
    [SerializeField] private RectTransform rectTransform;
    [SerializeField] private ButtonHighlightController buttonHighlight;


    private void Awake()
    {
        rectTransform = panelTransform.GetComponent<RectTransform>();

        foreach (var item in dataBase.structures)
        {
            GameObject newButton = Instantiate(buttonPrefab, panelTransform);
            newButton.GetComponent<ButtonData>().AssignData(item, placementController);
        }

        int buttonNumber = rectTransform.childCount;
        float width = buttonNumber * (buttonPrefab.GetComponent<RectTransform>().rect.width + rectTransform.GetComponent<HorizontalLayoutGroup>().spacing);

        rectTransform.sizeDelta = new Vector2(width, rectTransform.sizeDelta.y);
        buttonHighlight.GetButtons();
    }
}