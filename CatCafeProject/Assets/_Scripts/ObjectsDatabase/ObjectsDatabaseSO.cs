using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "ObjectsDatabaseSO", menuName = "CatCafeProject/ObjectsDatabaseSO", order = 0)]
public class ObjectsDatabaseSO : ScriptableObject
{
    public List<ObjectData> objectData;
}

[Serializable]
public class ObjectData
{
    [field: SerializeField]
    public string Name { get; private set; }

    [field: SerializeField]
    public int ID { get; private set; }

    [field: SerializeField]
    public Vector2Int Size { get; private set; } = Vector2Int.one;

    [field: SerializeField]
    public GameObject Prefab { get; private set; }

    [field: SerializeField]
    public int Prize { get; private set; } = 0;

    [field: SerializeField]
    public FurnitureType furnitureType { get; private set; }

    [field: SerializeField]
    public FurnitureTheme furnitureTheme { get; private set; }
}

public enum FurnitureType
{
    Carpet,
    Chair,
    Table,
    Bed,
    Mesa,
    CatTower,
    Decoration,
    Font,
    Sofa,
    Lamp,
    GeneralFurniture
}

public enum FurnitureTheme
{
    None,
    Flowers,
    Hearts,
    Leaves,
    Fishes
}