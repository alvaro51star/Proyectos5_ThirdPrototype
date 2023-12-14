using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FurnitureData : MonoBehaviour
{
    [SerializeField] private string furnitureName;
    [SerializeField] private int furnitureId;
    [SerializeField] private Vector2Int furnitureSize;
    [SerializeField] private int furniturePrize;
    [SerializeField] private FurnitureType furnitureType;
    [SerializeField] private FurnitureTheme furnitureTheme;
    [SerializeField] private int listIndex;

    public delegate void FurnitureAction(FurnitureData furnitureData, Vector3 position);
    public static event FurnitureAction OnCreateFurniture, OnDestroyFurniture;


    public FurnitureData(string furnitureName, int furnitureId, Vector2Int furnitureSize, int furniturePrize, FurnitureType furnitureType, FurnitureTheme furnitureTheme)
    {
        this.furnitureName = furnitureName;
        this.furnitureId = furnitureId;
        this.furnitureSize = furnitureSize;
        this.furniturePrize = furniturePrize;
        this.furnitureType = furnitureType;
        this.furnitureTheme = furnitureTheme;
    }

    public void AssignFurnitureData(string furnitureName, int furnitureId, Vector2Int furnitureSize, int furniturePrize, FurnitureType furnitureType, FurnitureTheme furnitureTheme)
    {
        this.furnitureName = furnitureName;
        this.furnitureId = furnitureId;
        this.furnitureSize = furnitureSize;
        this.furniturePrize = furniturePrize;
        this.furnitureType = furnitureType;
        this.furnitureTheme = furnitureTheme;
    }

    public void AssignFurnitureData(ObjectData objectData)
    {
        this.furnitureName = objectData.Name;
        this.furnitureId = objectData.ID;
        this.furnitureSize = objectData.Size;
        this.furniturePrize = objectData.Prize;
        this.furnitureType = objectData.furnitureType;
        this.furnitureTheme = objectData.furnitureTheme;
    }

    public void AssingIndexInObjectsCreated(int index)
    {
        listIndex = index;
    }

    private void OnDestroy()
    {
        OnDestroyFurniture?.Invoke(this, transform.position);
    }

    private void Start()
    {

    }
}
