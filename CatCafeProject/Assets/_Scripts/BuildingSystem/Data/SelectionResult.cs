using System.Collections.Generic;
using UnityEngine;

public struct SelectionResult
{
    public List<Vector3> selectedPositions;
    public List<Vector3Int> selectedGridPositions;
    public List<Vector3> selectedPreviewPositions;
    public List<Quaternion> selectedPositionsObjectRotation, selectedPositionGridCheckRotation;
    public bool placementValidity;
    //public Quaternion rotation;
    public Vector2Int size;
    public bool isEdgeStructure;
}