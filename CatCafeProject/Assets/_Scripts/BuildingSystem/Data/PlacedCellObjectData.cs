using System.Collections.Generic;
using UnityEngine;
/// <summary>
/// Data saved when we place a Cell object on our grid
/// </summary>
public class PlacedCellObjectData
{
    public IEnumerable<Vector3Int> PositionsOccupied { get; private set; }
    public int gameObjectIndex, structureID;
    public Vector3Int origin;

    public PlacedCellObjectData(int gameObjectIndex,int structureID, IEnumerable<Vector3Int> positionsOccupied, Vector3Int origin)
    {
        this.gameObjectIndex = gameObjectIndex;
        this.PositionsOccupied = positionsOccupied;
        this.structureID = structureID;
        this.origin = origin;
    }

}
