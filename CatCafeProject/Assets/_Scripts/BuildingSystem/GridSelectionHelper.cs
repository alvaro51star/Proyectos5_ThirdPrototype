using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Helper methods for A* algorithm and other algorithm that needs to find / check data present in our GridData structure
/// </summary>
public static class GridSelectionHelper
{
    public static IEnumerable<int> MoveMinToMaxInclusive(int minVal, int maxVal, int step)
    {
        for (int i = minVal; i <= maxVal; i += step)
        {
            yield return i;
        }
    }

    public static IEnumerable<int> MoveMaxToMinInclusive(int minVal, int maxVal, int step)
    {
        for (int i = maxVal; i >= minVal; i -= step)
        {
            yield return i;
        }
    }

    //I am assuming that object that recquires A* is of size 1x1
    public static List<Vector3Int> AStar(Vector3Int startPos, Vector3Int endPos, PlacementGridData placementData)
    {
        List<Vector3Int> path = new();
        List<Vector3Int> openList = new();
        Dictionary<Vector3Int, Vector3Int> childParentDictionary = new();
        Dictionary<Vector3Int,int> costDictionary = new();

        openList.Add(startPos);
        costDictionary[startPos] = ManhattanDistance(startPos, endPos);

        Vector3Int currentPosition = startPos;

        if(placementData.IsCellAt(endPos) == false)
            return new List<Vector3Int>();

        while(openList.Count > 0)
        {
            openList = openList.OrderBy(x => costDictionary[x]).ToList();
            currentPosition = openList[0];
            openList.RemoveAt(0);

            //Do when we reach the end
            if(currentPosition == endPos)
            {
                //get patern
                //currentPosition = childParentDictionary[currentPosition];
                //path.Add(endPos);
                while (currentPosition != startPos)
                {
                    path.Add(currentPosition);
                    currentPosition = childParentDictionary[currentPosition];
                }
                path.Add(startPos);
                path.Reverse();
                break;
            }

            //Do if we need to still search
            List<Vector3Int> neighbours = FindNeighbours(currentPosition, placementData);
            foreach (var neighbourposition in neighbours)
            {
                if (costDictionary.ContainsKey(neighbourposition))
                    continue;
                childParentDictionary[neighbourposition] = currentPosition;
                costDictionary[neighbourposition] = ManhattanDistance(neighbourposition, endPos);
                if (openList.Contains(neighbourposition) == false)
                    openList.Add(neighbourposition);
            }
        }
        return path;
    }

    private static List<Vector3Int> FindNeighbours(Vector3Int currentPosition, PlacementGridData placementData)
    {
        List<Vector3Int> neighbours = new();
        foreach (var direction in Directions)
        {
            Vector3Int tempPos = currentPosition + direction;
            if (placementData.IsCellAt(tempPos))
            {
                neighbours.Add(tempPos);
            }
        }
        return neighbours;
    }

    private static int ManhattanDistance(Vector3Int startPoint, Vector3Int endPoint)
    { 
        return Mathf.Abs(startPoint.x - endPoint.x) + Mathf.Abs(startPoint.z - endPoint.z);
    }

    internal static List<Quaternion> CalculateRotation(List<Vector3Int> gridPositions)
    {
        List<Quaternion> returnValues = new();
        for (int i = 0; i < gridPositions.Count-1; i++)
        {
            Vector3Int direction = gridPositions[i+1] - gridPositions[i];
            returnValues.Add(Quaternion.Euler(0,Mathf.RoundToInt(Vector3.SignedAngle(Vector3.right, direction,Vector3.up)),0));
        }
        return returnValues;
    }

    public static List<Vector3Int> Directions = new()
    {
        new Vector3Int(1,0,0),
        new Vector3Int(-1,0,0),
        new Vector3Int(0,0,1),
        new Vector3Int(0,0,-1)
    };
}