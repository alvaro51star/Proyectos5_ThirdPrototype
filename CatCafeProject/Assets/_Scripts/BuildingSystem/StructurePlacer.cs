using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using UnityEngine;
using DG.Tweening;

/// <summary>
/// Object responsible for placing GameObjects on the map and storing references to it.
/// </summary>
public class StructurePlacer : MonoBehaviour
{
    [SerializeField]
    List<GameObject> placedObjects = new List<GameObject>();

    [SerializeField]
    private float scalingDelay = 0.3f, destroyDelay = 0.1f;
    private int GetFreeIndex()
    {
        int indexOfNull = placedObjects.IndexOf(null);
        if (indexOfNull > -1)
        {
            return indexOfNull;
        }
        placedObjects.Add(null);
        return placedObjects.Count - 1;
    }

    public Quaternion GetObjectsRotation(int index)
    {
        Quaternion rotationToReturn = Quaternion.identity;
        if (index >= 0 && index < placedObjects.Count && placedObjects[index] != null)
            rotationToReturn = placedObjects[index].transform.GetChild(0).rotation;
        return rotationToReturn;
    }


    /// <summary>
    /// When we place a structure on the map we return its index (index from a list of GO above)
    /// so that if we need ro remove it we use the index as a reference to remove the correct GO
    /// </summary>
    /// <param name="objectToPlace"></param>
    /// <param name="position"></param>
    /// <param name="rotation"></param>
    /// <param name="yHeight"></param>
    /// <returns></returns>
    public int PlaceStructure(GameObject objectToPlace, Vector3 position, Quaternion rotation, float yHeight)
    {
        int freeIndex = GetFreeIndex();
        GameObject newObject = Instantiate(objectToPlace);
        newObject.transform.SetParent(transform);
        Vector3 placementPosition = new Vector3(position.x, yHeight, position.z);
        newObject.transform.position = placementPosition;

        newObject.transform.GetChild(0).rotation = rotation;
        newObject.transform.localScale = new Vector3(1,0,1);
        placedObjects[freeIndex] = newObject;
        newObject.transform.DOScaleY(1, scalingDelay);
        return freeIndex;
    }

    public void RemoveObjectAt(int index)
    {
        GameObject newObject = placedObjects[index];
        newObject.transform.DOKill();
        newObject.transform.DOScaleY(0, destroyDelay).OnComplete(()=> Destroy(newObject));
        //Destroy(newObject);
        placedObjects[index] = null;

    }

    private void OnDisable()
    {
        foreach (GameObject obj in placedObjects) 
        {
            //Instead of removing NULL from our list we populate it with new structures
            //That is why we need to test here if it is null or not
            if(obj != null)
                obj.transform.DOComplete();
        }
    }
}
