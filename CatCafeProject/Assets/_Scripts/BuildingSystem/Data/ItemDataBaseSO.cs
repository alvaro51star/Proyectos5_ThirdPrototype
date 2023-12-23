using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using UnityEngine;

/// <summary>
/// Database where we add all the Objects that we want to place on our map
/// </summary>
[CreateAssetMenu]
public class ItemDataBaseSO : ScriptableObject
{
    public List<ItemData> structures;

    public ItemData GetItemWithID(int id)
    {
        return structures.FirstOrDefault(structureData => structureData.ID == id);
    }

    /// <summary>
    /// Valudates ID of the objects to prevent duplicates
    /// </summary>
    private void OnValidate()
    {
        HashSet<int> IDs = new();
        foreach (var item in structures)
        {
            if (IDs.Contains(item.ID))
                Debug.LogError($"Dupliate ID found {item.ID} for {item.name} in StructuresData");
            IDs.Add(item.ID);
            if (item.previewObject == null)
                item.previewObject = item.prefab;
        }
    }
}

/// <summary>
/// Single Objet (wall / furniture) definition
/// </summary>
[Serializable]
public class ItemData
{
    public string name;
    //most important prameter since we save it in our data and if we wanted to recreate the place objects
    //all we would have to do is find an item with a give ID, read its data and place it back on the map
    public int ID;
    public Vector2Int size;
    public PlacementType objectPlacementType;
    public GameObject prefab;
    [Tooltip("If empty will be set to prefab object")]
    public GameObject previewObject;
}
