using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPlacer : MonoBehaviour
{
    [SerializeField]
    private List<GameObject> placedGameObjects = new();
    [SerializeField] private Transform parent;

    public int PlaceObject(GameObject prefab, Vector3 position, ObjectData objectData)
    {
        GameObject newObject = Instantiate(prefab, parent);
        newObject.transform.position = position;
        newObject.AddComponent<FurnitureData>();
        newObject.GetComponent<FurnitureData>().AssignFurnitureData(objectData);
        placedGameObjects.Add(newObject);
        int index = placedGameObjects.Count - 1;
        newObject.GetComponent<FurnitureData>().AssingIndexInObjectsCreated(index);
        return index;
    }

    internal void RemoveObjectAt(int gameObjectIndex)
    {
        if (placedGameObjects.Count <= gameObjectIndex || placedGameObjects[gameObjectIndex] == null)
        {
            return;
        }
        Destroy(placedGameObjects[gameObjectIndex]);
        placedGameObjects[gameObjectIndex] = null;
    }
}
