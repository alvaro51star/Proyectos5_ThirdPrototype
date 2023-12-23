using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEngine;

/// <summary>
/// Allows to remove wall objects *not inWall objects
/// </summary>
public class WallRemovalStrategy : WallPlacementStrategy
{
    public WallRemovalStrategy(PlacementGridData wallPlacementData, PlacementGridData inWallPlacementData, PlacementGridData objectPlacementData, GridManager gridManager) : base(wallPlacementData,inWallPlacementData,objectPlacementData, gridManager)
    {
    }

    protected override bool ValidatePlacement(SelectionData selectionData)
    {
        return true;
    }
}
