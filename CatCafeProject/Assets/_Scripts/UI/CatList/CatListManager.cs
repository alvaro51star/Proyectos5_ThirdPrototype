using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class CatListManager : MonoBehaviour
{
    [SerializeField] private Dictionary<CatDataSO, int> catsPerType = new();
    [SerializeField] private List<GameObject> catListItem;

    [SerializeField] private bool isClosed = true;

    [SerializeField] private float openedXPosition, closedXPosition;
    [SerializeField] private float tweenDuration;
    [SerializeField] private RectTransform panelRect;

    [SerializeField] private GameObject flecha;

    private void Start()
    {
        panelRect = GetComponent<RectTransform>();
    }

    private void OnEnable()
    {
        GameManager.OnCatListCreated += SetList;
    }

    private void OnDisable()
    {
        GameManager.OnCatListCreated -= SetList;
    }

    private void SetList(List<CatDataSO> catList, List<CatDataSO> catDataList)
    {
        SetDictionaryItem(catList, catDataList);

        if (catsPerType.Count == catListItem.Count)
        {
            for (int i = 0; i < catsPerType.Count; i++)
            {
                if (catsPerType[catDataList[i]] > 0)
                {
                    catListItem[i].SetActive(true);
                    catListItem[i].transform.GetChild(1).GetComponent<TextMeshProUGUI>().text = $"x {catsPerType[catDataList[i]]}";
                }
            }
        }
    }

    private void SetDictionaryItem(List<CatDataSO> catList, List<CatDataSO> catDataList)
    {
        for (int i = 0; i < catDataList.Count; i++)
        {
            catsPerType.Add(catDataList[i], 0);
        }

        for (int i = 0; i < catList.Count; i++)
        {
            if (catsPerType.TryGetValue(catList[i], out int numberOfCats))
            {
                catsPerType[catList[i]] = ++numberOfCats;
            }
        }

        ShowDictionary();
    }


    //? solo para testeo
    private void ShowDictionary()
    {
        foreach (var item in catsPerType)
        {
            Debug.Log($"{item.Key} = {item.Value}");
        }
    }

    public void OpenCloseMenu()
    {
        if (isClosed)
        {
            flecha.transform.DOLocalRotate(new Vector3(0, 0, 180), tweenDuration).SetEase(Ease.InOutExpo);
            panelRect.DOAnchorPosX(openedXPosition, tweenDuration).SetEase(Ease.InOutExpo);
            isClosed = false;
        }
        else
        {
            flecha.transform.DOLocalRotate(Vector3.zero, tweenDuration).SetEase(Ease.InOutExpo);
            panelRect.DOAnchorPosX(closedXPosition, tweenDuration).SetEase(Ease.InOutExpo);
            isClosed = true;
        }
    }


}
