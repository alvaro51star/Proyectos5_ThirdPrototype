using System;
using System.Collections.Generic;
using UnityEngine;

public static class PlacementValidator
{
    public static bool CheckIfPositionsAreOccupied(List<Vector3Int> selectedPositions, PlacementGridData placementData, Vector2Int objectSize, List<Quaternion> selectedPositionsRotation, bool edgePlacement)
    {
        for (int i = 0; i < selectedPositions.Count; i++)
        {
            if (placementData.IsSpaceOccupied(selectedPositions[i], objectSize, Mathf.RoundToInt(selectedPositionsRotation[i].eulerAngles.y), edgePlacement) == false)
            {
                return false;
            }
        }
        return true;
    }

    public static bool CheckIfPositionsAreFree(List<Vector3Int> selectedPositions, PlacementGridData placementData, Vector2Int objectSize, List<Quaternion> selectedPositionsRotation, bool edgePlacement)
    {
        for (int i = 0; i < selectedPositions.Count; i++)
        {
            if (placementData.IsSpaceFree(selectedPositions[i], objectSize, Mathf.RoundToInt(selectedPositionsRotation[i].eulerAngles.y), edgePlacement) == false)
            {
                return false;
            }
        }
        return true;
    }

    public static bool CheckIfPositionsAreValid(List<Vector3Int> selectedPositions, PlacementGridData placementData, Vector2Int objectSize, List<Quaternion> selectedPositionsRotation, bool edgePlacement)
    {
        for (int i = 0; i < selectedPositions.Count; i++)
        {
            if (placementData.IsSpaceValid(selectedPositions[i], objectSize, Mathf.RoundToInt(selectedPositionsRotation[i].eulerAngles.y), edgePlacement) == false)
            {
                return false;
            }
        }
        return true;
    }

    internal static bool CheckIfNotCrossingMultiCellObject(List<Vector3Int> selectedPositions, PlacementGridData placementData, Vector2Int objectSize, List<Quaternion> selectedPositionsRotation, bool edgePlacement)
    {
        for (int i = 0; i < selectedPositions.Count; i++)
        {
            if (placementData.IsSpaceOccupiedByMultitileObject(selectedPositions[i], objectSize, Mathf.RoundToInt(selectedPositionsRotation[i].eulerAngles.y), edgePlacement))
            {
                return false;
            }
        }
        return true;
    }

    internal static bool CheckIfNotCrossingEdgeObject(List<Vector3Int> selectedPositions, PlacementGridData placementData, Vector2Int objectSize, List<Quaternion> selectedPositionsRotation, bool edgePlacement)
    {
        for (int i = 0; i < selectedPositions.Count; i++)
        {
            if (placementData.IsSpaceOccupiedByEdgeObject(selectedPositions[i], objectSize, Mathf.RoundToInt(selectedPositionsRotation[i].eulerAngles.y), edgePlacement))
            {
                return false;
            }
        }
        return true;
    }

    internal static bool CheckIfPositionsAreNearWall(List<Vector3Int> selectedPositions, PlacementGridData placementData, Vector2Int objectSize, List<Quaternion> selectedPositionsRotation, bool edgePlacement)
    {
        int rotationEulerY = Mathf.RoundToInt(selectedPositionsRotation[0].eulerAngles.y);
        //Check if there are no walls where we want to place the object
        HashSet<Edge> edges = new();
        foreach (Vector3Int pos in selectedPositions)
        {
            //It realy doesnt matter that we are using "wall placement data" to select those. Those are the same for any PlacementGridData as
            //those are calculated
            List<Vector3Int> cellsToOccupy = placementData.GetCellPositions(pos, objectSize, rotationEulerY);
            foreach (var cellPosition in cellsToOccupy)
            {
                //Algorithm that gets all the edges that the placed object crosses (possible walls)
                Vector3Int offset = cellPosition - pos;
                if (rotationEulerY == 0)
                {
                    //We need to check if the edge is valid
                    if (offset.z == objectSize.y - 1 && placementData.IsCellAt(cellPosition + Vector3Int.forward))
                    {
                        //we need the edge for the above cell
                        edges.UnionWith(placementData.GetEdgePositions(cellPosition + Vector3Int.forward, Vector2Int.one, 0));
                    }
                }
                else if (rotationEulerY == 90 && placementData.IsCellAt(cellPosition + Vector3Int.right))
                {
                    if (offset.x == objectSize.x - 1)
                    {
                        edges.UnionWith(placementData.GetEdgePositions(cellPosition + Vector3Int.right, Vector2Int.one, 270));
                    }
                }
                else if (rotationEulerY == 180)
                {
                    if (offset.z == 0)
                    {
                        edges.UnionWith(placementData.GetEdgePositions(cellPosition, Vector2Int.one, 0));
                    }
                }
                else
                {
                    if (offset.x == 0)
                    {
                        edges.UnionWith(placementData.GetEdgePositions(cellPosition, Vector2Int.one, 270));
                    }
                }
            }
        }
        //Should check if the wall is near all the edges of the object (edge is dependand on the rotation
        // but by default with 0 rotation it is the top / forward direction)
        foreach (var edgePos in edges)
        {
            if (placementData.IsEdgeObjectAt(edgePos) == false)
            {
                return false;
            }
        }
        return true;
    }
}
