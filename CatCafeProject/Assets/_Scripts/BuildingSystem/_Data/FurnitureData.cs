using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureData: MonoBehaviour
{
    [SerializeField] private string furnitureName;
    [SerializeField] private int furnitureId;
    [SerializeField] private int furniturePrize;
    [SerializeField] public FurnitureType furnitureType;
    [SerializeField] public FurnitureTheme furnitureTheme;
    [SerializeField] private int listIndex;

    public delegate void FurnitureAction(FurnitureData furnitureData, Vector3 position);
    public static event FurnitureAction OnCreateFurniture, OnDestroyFurniture;


    public FurnitureData(string furnitureName, int furnitureId, int furniturePrize, FurnitureType furnitureType, FurnitureTheme furnitureTheme)
    {
        this.furnitureName  = furnitureName;
        this.furnitureId    = furnitureId;
        this.furniturePrize = furniturePrize;
        this.furnitureType  = furnitureType;
        this.furnitureTheme = furnitureTheme;
    }

    public void AssignFurnitureData(string furnitureName, int furnitureId, int furniturePrize, FurnitureType furnitureType, FurnitureTheme furnitureTheme)
    {
        this.furnitureName  = furnitureName;
        this.furnitureId    = furnitureId;
        this.furniturePrize = furniturePrize;
        this.furnitureType  = furnitureType;
        this.furnitureTheme = furnitureTheme;
    }

    public void AssignFurnitureData(ObjectData objectData)
    {
        furnitureName  = objectData.Name;
        furnitureId    = objectData.ID;
        furniturePrize = objectData.Prize;
        furnitureType  = objectData.furnitureType;
        furnitureTheme = objectData.furnitureTheme;
    }

    public void AssignFurnitureData(ItemData itemData)
    {
        furnitureName  = itemData.name;
        furnitureId    = itemData.ID;
        furniturePrize = itemData.buyValue;
        furnitureType  = itemData.furnitureType;
        furnitureTheme = itemData.furnitureTheme;
    }

    public void AssingIndexInObjectsCreated(int index)
    {
        listIndex = index;
    }
}
