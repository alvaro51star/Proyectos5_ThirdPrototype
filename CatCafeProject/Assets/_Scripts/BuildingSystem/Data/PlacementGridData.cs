using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This class store cell and edge placement
/// In the project we use multiple of those data class to store
/// data for different placement systems (wall , inwall, objects) separatelly.
/// This allows us to place Furniture on top of floor cells without getting
/// "Position already occupied" error. At the same time we can check if 
/// an object is already there or remove only floor tiles without the furniture.
/// </summary>
public class PlacementGridData
{
    Dictionary<Vector3Int, PlacedCellObjectData> gridCellsDictionary;
    Dictionary<Edge, PlacedEdgeObjectData> gridEdgesDictionary;

    int xGridBoundMin, xGridBoundMax, zGridBoundMin, zGridBoundMax;
    public PlacementGridData(int xMin, int xMax, int zMin, int zMax)
    {
        gridCellsDictionary = new();
        gridEdgesDictionary = new();
        xGridBoundMax = xMax;
        zGridBoundMax = zMax;
        xGridBoundMin = xMin;
        zGridBoundMin = zMin;
    }

    /// <summary>
    /// Checks if the given positions are apty in our grid
    /// </summary>
    /// <param name="currentTilePosition"></param>
    /// <param name="objectSize"></param>
    /// <param name="rotation"></param>
    /// <param name="edgePlacement"></param>
    /// <returns></returns>
    public bool IsSpaceFree(Vector3Int currentTilePosition, Vector2Int objectSize, int rotation, bool edgePlacement)
    {
        if(edgePlacement) 
        { 
            List<Edge> edges = GetEdgePositions(currentTilePosition, objectSize,rotation);
            return gridEdgesDictionary.Keys.Any(edgePos => edges.Any(x => x == edgePos)) == false;
        }
        else
        {
            List<Vector3Int> positionsToOccupy = GetCellPositions(currentTilePosition, objectSize, rotation);
            return gridCellsDictionary.Keys.Any(pos => positionsToOccupy.Any(x => x == pos)) == false;
        }
        
    }

    /// <summary>
    /// Checks if give spaces are occupied
    /// </summary>
    /// <param name="currentTilePosition"></param>
    /// <param name="objectSize"></param>
    /// <param name="rotation"></param>
    /// <param name="edgePlacement"></param>
    /// <returns></returns>
    public bool IsSpaceOccupied(Vector3Int currentTilePosition, Vector2Int objectSize, int rotation, bool edgePlacement)
    {
        if (edgePlacement)
        {
            List<Edge> edges = GetEdgePositions(currentTilePosition, objectSize, rotation);
            return edges.All(edgePos => gridEdgesDictionary.Keys.Contains(edgePos));
        }
        else
        {
            List<Vector3Int> positionsToOccupy = GetCellPositions(currentTilePosition, objectSize, rotation);
            return positionsToOccupy.All(pos => gridCellsDictionary.Keys.Contains(pos));
        }

    }

    /// <summary>
    /// Checks if given positions are inside the bounds of the Grid of ours
    /// </summary>
    /// <param name="currentTilePosition"></param>
    /// <param name="objectSize"></param>
    /// <param name="rotation"></param>
    /// <param name="edgePlacement"></param>
    /// <returns></returns>
    internal bool IsSpaceValid(Vector3Int currentTilePosition, Vector2Int objectSize, int rotation, bool edgePlacement)
    {
        if (edgePlacement)
        {
            List<Edge> edges = GetEdgePositions(currentTilePosition, objectSize, rotation);
            //If one position is invalid the Any returns tru so the method returns false
            return edges.Any(edgePos => IsCellAt(edgePos.smallerPoint) == false) == false;
        }
        else
        {
            List<Vector3Int> positionsToOccupy = GetCellPositions(currentTilePosition, objectSize, rotation);
            //If one position is invalid the Any returns tru so the method returns false
            return positionsToOccupy.Any(pos => IsCellAt(pos) == false) == false;
        }
    }

    /// <summary>
    /// Allows us to find the cell positions recquired for an item or certain size to be placed here
    /// startng at the currentTileposition and going in the direction (towards Bottom-Right, Top-Left or any other way)
    /// based on the rotation.
    /// </summary>
    /// <param name="currentTilePosition"></param>
    /// <param name="objectSize"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public List<Vector3Int> GetCellPositions(Vector3Int currentTilePosition, Vector2Int objectSize, int rotation)
    {
        IEnumerable<int> xRange = null;
        IEnumerable<int> zRange = null;
        objectSize -= Vector2Int.one;
        if (rotation == 0)
        {
            xRange = GridSelectionHelper.MoveMinToMaxInclusive(0, objectSize.x, 1);
            zRange = GridSelectionHelper.MoveMinToMaxInclusive(0, objectSize.y, 1);
        }
        else if (rotation == 90)
        {
            objectSize = new Vector2Int(objectSize.y, objectSize.x);
            xRange = GridSelectionHelper.MoveMinToMaxInclusive(0, objectSize.x, 1);
            zRange = GridSelectionHelper.MoveMaxToMinInclusive(-objectSize.y, 0, 1);
        }
        else if (rotation == 180)
        {
            xRange = GridSelectionHelper.MoveMaxToMinInclusive(-objectSize.x, 0, 1);
            zRange = GridSelectionHelper.MoveMaxToMinInclusive(-objectSize.y, 0, 1);
        }
        else if (rotation == 270)
        {
            objectSize = new Vector2Int(objectSize.y, objectSize.x);
            xRange = GridSelectionHelper.MoveMaxToMinInclusive(-objectSize.x, 0, 1);
            zRange = GridSelectionHelper.MoveMinToMaxInclusive(0, objectSize.y, 1);
        }

        List<Vector3Int> positions = new List<Vector3Int>();
        foreach (int x in xRange)
        {
            foreach (int z in zRange)
            {
                Vector3Int offset = new Vector3Int(x, 0, z);
                positions.Add(currentTilePosition + offset);
            }
        }
        return positions;
    }

    /// <summary>
    /// Allows us to get Edges starting at the currentTilePosition and based on the rotation (size is for walls 1x1 but could be 2x1)
    /// Probably will not work for an edge objects with size that has y size value greater than 1 with x value also greate than 1.
    /// </summary>
    /// <param name="currentTilePosition"></param>
    /// <param name="size"></param>
    /// <param name="rotation"></param>
    /// <returns></returns>
    public List<Edge> GetEdgePositions(Vector3Int currentTilePosition, Vector2Int size, int rotation)
    {
        IEnumerable<int> xRange = null;
        IEnumerable<int> zRange = null;
        Vector3Int edgeDirection = Vector3Int.zero;
        
        //If we rotate object by 90 or 270 we need to swap size x with y and vice versa
        //We subtract Vector2.One since GridSelectionHelper uses Inclusive bounds
        Vector2Int calculatedSize = size - Vector2Int.one;
        if(rotation == 0)
        {
            xRange = GridSelectionHelper.MoveMinToMaxInclusive(0, calculatedSize.x, 1);
            zRange = GridSelectionHelper.MoveMinToMaxInclusive(0, calculatedSize.y, 1);
            edgeDirection = Vector3Int.right;
        }
        else if(rotation == 90)
        {
            calculatedSize = new Vector2Int(calculatedSize.y, calculatedSize.x);
            xRange = GridSelectionHelper.MoveMinToMaxInclusive(0, calculatedSize.x, 1);
            zRange = GridSelectionHelper.MoveMaxToMinInclusive(-calculatedSize.y, 0, 1);
            edgeDirection = Vector3Int.back;
        }
        else if(rotation == 180)
        {
            xRange = GridSelectionHelper.MoveMaxToMinInclusive(-calculatedSize.x, 0, 1);
            zRange = GridSelectionHelper.MoveMaxToMinInclusive(-calculatedSize.y, 0, 1);
            edgeDirection = Vector3Int.left;
        }
        else if(rotation == 270)
        {
            calculatedSize = new Vector2Int(calculatedSize.y, calculatedSize.x);
            xRange = GridSelectionHelper.MoveMaxToMinInclusive(-calculatedSize.x, 0, 1);
            zRange = GridSelectionHelper.MoveMinToMaxInclusive(0, calculatedSize.y, 1);
            edgeDirection = Vector3Int.forward;
        }
        List<Edge> positions = new();

        //To be able to compare edges we always want to create a new Edge record
        //starting with the point that has smaller x or y
        Vector3Int min = Vector3Int.zero, max = Vector3Int.zero;
        foreach (int x in xRange)
        {
            foreach (int z in zRange)
            {
                Vector3Int offset = new Vector3Int(x, 0, z);
                Vector3Int point_1 = currentTilePosition + offset;
                Vector3Int point_2 = currentTilePosition + offset + edgeDirection;

                if (point_1.x > point_2.x || point_1.z > point_2.z) { 
                min = point_2; 
                max = point_1;
                }
                else
                {
                    min = point_1;
                    max = point_2;
                }
                positions.Add( new Edge(min, max));
            }
        }
        return positions;
    }

    public void AddCellObject(int index, int ID, Vector3Int currentTilePosition, Vector2Int objectSize, int rotation)
    {
        List<Vector3Int> positionsToOccupy = GetCellPositions(currentTilePosition, objectSize, rotation);
        PlacedCellObjectData data = new(index, ID, positionsToOccupy, currentTilePosition);
        foreach (Vector3Int pos in positionsToOccupy)
        {
            //Debug.Log($"Placing object at {pos}");
            gridCellsDictionary.Add(pos, data);
        }
    }

    public void AddEdgeObject(int index, int ID, Vector3Int currentTilePosition, Vector2Int objectSize, int rotation)
    {
        List<Edge> edgesToOccupy = GetEdgePositions(currentTilePosition, objectSize, rotation);
        PlacedEdgeObjectData data = new(index, ID, edgesToOccupy, currentTilePosition);
        foreach (Edge pos in edgesToOccupy)
        {
            //Debug.Log($"Placing object at {pos}");
            gridEdgesDictionary.Add(pos, data);
        }
    }

    internal bool IsCellObjectAt(Vector3Int currentTilePosition)
    {
        return gridCellsDictionary.ContainsKey(currentTilePosition);
    }

    internal bool IsEdgeObjectAt(Edge edgePosition)
    {
        return gridEdgesDictionary.ContainsKey(edgePosition);
    }

    internal int GetIndexForCellObject(Vector3Int currentTilePosition)
    {
        if (gridCellsDictionary.ContainsKey(currentTilePosition) == false)
            return -1;
        return gridCellsDictionary[currentTilePosition].gameObjectIndex;
    }

    internal int GetIndexForEdgeObject(Vector3Int currentTilePosition, int rotation)
    {
        List<Edge> edgePositions = GetEdgePositions(currentTilePosition, Vector2Int.one, rotation);
        if (gridEdgesDictionary.ContainsKey(edgePositions[0]) == false)
            return -1;
        return gridEdgesDictionary[edgePositions[0]].gameObjectIndex;
    }

    public List<Edge> GetEdgesOccupiedForEdgeObject(Edge selectedEdge)
    {
        if (gridEdgesDictionary.ContainsKey(selectedEdge) == false)
            return null;
        return new(gridEdgesDictionary[selectedEdge].PositionsOccupied);
    }

    public List<Vector3Int> GetPositionsOccupiedForCellObject(Vector3Int currentTilePosition)
    {
        if (gridCellsDictionary.ContainsKey(currentTilePosition) == false)
            return null;
        return new(gridCellsDictionary[currentTilePosition].PositionsOccupied);
    }

    internal Vector3Int? GetOriginForCellObject(Vector3Int currentTilePosition)
    {
        if (gridCellsDictionary.ContainsKey(currentTilePosition) == false)
            return null;
        return gridCellsDictionary[currentTilePosition].origin;
    }

    internal Vector3Int? GetOriginForEdgeObject(Edge selectedEdge)
    {
        if (gridEdgesDictionary.ContainsKey(selectedEdge) == false)
            return null;
        return gridEdgesDictionary[selectedEdge].origin;
    }

    public int GetStructureIDForEdgeObject(Vector3Int currentTilePosition, int rotation)
    {
        List<Edge> edgePositions = GetEdgePositions(currentTilePosition, Vector2Int.one, rotation);
        if (gridEdgesDictionary.ContainsKey(edgePositions[0]) == false)
            return -1;
        return gridEdgesDictionary[edgePositions[0]].structureID;
    }

    public int GetStructureIDForCellObject(Vector3Int currentTilePosition)
    {
        if (gridCellsDictionary.ContainsKey(currentTilePosition) == false)
            return -1;
        return gridCellsDictionary[currentTilePosition].structureID;
    }

    internal void RemoveCellObject(Vector3Int currentTilePosition)
    {
        PlacedCellObjectData data = gridCellsDictionary[currentTilePosition];
        foreach (Vector3Int position in data.PositionsOccupied)
        {
            if (gridCellsDictionary.ContainsKey(position))
            {
                PlacedCellObjectData placementData = gridCellsDictionary[position];
                foreach (var item in placementData.PositionsOccupied)
                {
                    gridCellsDictionary.Remove(item);
                }
            }
        }
    }

    internal void RemoveEdgeObject(Vector3Int currentTilePosition, Vector2Int size, int rotation)
    {
        List<Edge> edgePositions = GetEdgePositions(currentTilePosition, size, rotation);
        foreach (Edge position in edgePositions)
        {
            if (gridEdgesDictionary.ContainsKey(position))
            {
                PlacedEdgeObjectData data = gridEdgesDictionary[position];
                foreach (var item in data.PositionsOccupied)
                {
                    gridEdgesDictionary.Remove(item);
                }
            }
            
        }
    }

    internal bool IsCellAt(Vector3Int tempPos)
    {
        if (tempPos.x >= xGridBoundMin && tempPos.x <= xGridBoundMax && tempPos.z >= zGridBoundMin && tempPos.z <= zGridBoundMax)
            return true;
        return false;
    }

    /// <summary>
    /// Checks if Cells on both sides of the Edge are occupied by the same object and returns true if yes or false if no.
    /// </summary>
    /// <param name="currentTilePosition"></param>
    /// <param name="objectSize"></param>
    /// <param name="rotation"></param>
    /// <param name="edgePlacement"></param>
    /// <returns></returns>
    /// <exception cref="NotImplementedException"></exception>
    internal bool IsSpaceOccupiedByMultitileObject(Vector3Int currentTilePosition, Vector2Int objectSize, int rotation, bool edgePlacement)
    {
        if (edgePlacement)
        {
            List<Edge> edges = GetEdgePositions(currentTilePosition, objectSize, rotation);
            //Based on the rotation we know that if edge goes forward or backwards (Z axis) we need to check the tile on the right and on the left
            //Otherwise we need to check tiles above and below the edge as it is horizontal (X axis)
            Vector3Int offset;
            if (rotation == 0 || rotation == 180)
                offset = Vector3Int.back;
            else
                offset = Vector3Int.left;
            foreach (Edge edge in edges)
            {
                //If there is no object on this cell contiune
                if (gridCellsDictionary.ContainsKey(edge.smallerPoint) == false || gridCellsDictionary.ContainsKey(edge.smallerPoint + offset) == false)
                    continue;
                if (gridCellsDictionary[edge.smallerPoint].gameObjectIndex == gridCellsDictionary[edge.smallerPoint + offset].gameObjectIndex)
                    return true;

            }
            return false;
        }
        throw new NotImplementedException();
    }

    internal bool IsSpaceOccupiedByEdgeObject(Vector3Int currentTilePosition, Vector2Int objectSize, int rotation, bool edgePlacement)
    {
        HashSet<Edge> edges = new();
        List<Vector3Int> cellsToOccupy = GetCellPositions(currentTilePosition, objectSize, rotation);
        foreach (var cellPosition in cellsToOccupy)
        {
            //Algorithm that gets all the edges that the placed object crosses (possible walls)
            Vector3Int offset = cellPosition - currentTilePosition;
            
            if (offset.x == 0 && offset.z == 0) //We use Z coordinate because we are working in a 3D space
                continue;
            if (offset.z == 0 && offset.x <= objectSize.x)
                edges.UnionWith(GetEdgePositions(cellPosition, Vector2Int.one, 270));
            else if (offset.x == 0 && offset.z <= objectSize.y)
                edges.UnionWith(GetEdgePositions(cellPosition, Vector2Int.one, 0));
            else
            {
                edges.UnionWith(GetEdgePositions(cellPosition, Vector2Int.one, 0));
                edges.UnionWith(GetEdgePositions(cellPosition, Vector2Int.one, 270));
            }
        }
        foreach (var edgePos in edges)
        {
            if (IsEdgeObjectAt(edgePos))
            {
                return true;
            }
        }
        return false;
    }

}

// Edges are saved in our code defined as as Bottom to Top or Left to Right just for an ease of working with them
//
//   |_|_|_
//   |_|_|_ <-- Since cell position is its bottm-left corner we store only bottom and left edge per each Cell for the ease of data storage / algorithms
//   |_|_|_
//

/// <summary>
/// Edge of our grid are defined as Record. 
/// We need the namespace System.Run... a the bottom for the Records to work in Unity
/// </summary>
/// <param name="smallerPoint">Bottm/Left most point</param>
/// <param name="biggerPoint">Top/Right most point</param>
public record Edge(Vector3Int smallerPoint, Vector3Int biggerPoint);

namespace System.Runtime.CompilerServices
{
    public class IsExternalInit
    {

    }
}