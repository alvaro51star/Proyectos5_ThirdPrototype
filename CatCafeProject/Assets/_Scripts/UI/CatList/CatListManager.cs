using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;

public class CatListManager : MonoBehaviour
{
    [SerializeField] private Dictionary<CatDataSO, int> catsPerType = new();
    [SerializeField] private List<GameObject> catListItem;

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
}
