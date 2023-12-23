using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows us to show the preview for the objects that we are planning to place.
/// Shows transparent objects and tint them red in case the placement is forbidden
/// </summary>
public class PlacementPreview : MonoBehaviour
{
    [SerializeField]
    private Material transparentMaterial;
    private Material transparentMaterialInstance;

    private List<GameObject> previewObjects = new();
    private GameObject cursorObject = null;
    private GameObject previewTemplate;
    private Renderer previewObjectRenderer;

    [SerializeField]
    private float yOffset = 0.05f;

    [SerializeField]
    private Color destroyColor;

    Color defautlColor;

    private void Start()
    { 
        defautlColor = transparentMaterial.color;
        transparentMaterialInstance = new Material(transparentMaterial);
    }

    /// <summary>
    /// Updates the preview prefabs positions
    /// </summary>
    /// <param name="positions"></param>
    /// <param name="rotation"></param>
    public void MovePreview(List<Vector3> positions, List<Quaternion> rotation)
    {
        if(previewObjects.Count > positions.Count)
        {
            for (int i = previewObjects.Count - 1; i >= positions.Count; i--)
            {
                Destroy(previewObjects[i]);
                previewObjects.RemoveAt(i);
            }
        }
        for (int i = 0; i < positions.Count; i++)
        {
            if(previewObjects.Count == i)
            {
                previewObjects.Add(Instantiate(previewTemplate));
            }
            Vector3 pos = positions[i];
            pos.y += yOffset;
            previewObjects[i].transform.position = pos;
            previewObjects[i].transform.localScale = Vector3.one*1.02f;
            if (previewObjects[i].transform.childCount != 0)
                previewObjects[i].transform.GetChild(0).rotation = rotation[i];
            else
                previewObjects[i].transform.rotation = rotation[i];
        }
    }


    /// <summary>
    /// Disables preview objects
    /// </summary>
    public void StopShowingPreview()
    {
        previewObjectRenderer = null;
        transparentMaterialInstance.color = defautlColor;
        foreach (var item in previewObjects)
        {
            Destroy(item);
        }
        if(cursorObject != null)
            Destroy(cursorObject);
        previewTemplate = null;
        previewObjects.Clear();
    }

    /// <summary>
    /// Starts showing preview objects.
    /// </summary>
    /// <param name="placedObject"></param>
    /// <param name="keepMaterial">For removing objects state we change the preview to a red transparent square and set this to True</param>
    public void StartShowingPreview(GameObject placedObject, bool keepMaterial = false)
    {
        if (keepMaterial)
        {
            previewTemplate = Instantiate(placedObject, transform);
        }
        else
        {
            previewTemplate = CreatePreviewObject(placedObject);
        }
        
        previewObjects.Clear();
        previewObjects.Add(previewTemplate);
    }

    /// <summary>
    /// Creates a preview object out of the objects prefab and swaps its material to a transparent one
    /// </summary>
    /// <param name="placedObject"></param>
    /// <returns></returns>
    private GameObject CreatePreviewObject(GameObject placedObject)
    {
        GameObject preview = Instantiate(placedObject, transform);
        preview.transform.position = Vector3.zero;
        previewObjectRenderer = preview.GetComponent<Renderer>();
        if(previewObjectRenderer == null)
            foreach (var renderer in preview.GetComponentsInChildren<Renderer>())
            {
                PreparePreviewPrefab(renderer);
            }
        else
            PreparePreviewPrefab(previewObjectRenderer);
        return preview;
    }

    /// <summary>
    /// Assigns a transparent material to a preview objects
    /// so that we can make it any color we wante (white and red in this case)
    /// </summary>
    /// <param name="renderer"></param>
    private void PreparePreviewPrefab(Renderer renderer)
    {
        previewObjectRenderer = renderer;
        Material[] newMaterialArray = new Material[previewObjectRenderer.materials.Length];
        for (int i = 0; i < newMaterialArray.Length; i++)
        {
            newMaterialArray[i] = transparentMaterialInstance;
        }
        previewObjectRenderer.materials = newMaterialArray;
    }

    /// <summary>
    /// Switches the white (correct placement) preview material feedback to red (not allowed placement feedback)
    /// </summary>
    /// <param name="val"></param>
    public void ShowPlacementFeedback(bool val)
    {
        if (val)
            PlacementFeedbackPositive();
        else
            PlacementFeedbackNegative();
    }

    private void PlacementFeedbackPositive()
    {
        transparentMaterialInstance.color = defautlColor;
    }

    private void PlacementFeedbackNegative()
    {
        Color c = Color.red;
        c.a = defautlColor.a;
        transparentMaterialInstance.color = c;
        //previewObject.GetComponent<Renderer>().material.color = c;
    }
}
