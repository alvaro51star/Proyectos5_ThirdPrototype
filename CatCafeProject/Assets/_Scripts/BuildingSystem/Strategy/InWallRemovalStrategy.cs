using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows to remove inWall Objects like doors and walls
/// </summary>
public class InWallRemovalStrategy : InWallPlacementStrategy
{
    public InWallRemovalStrategy(PlacementGridData inWallObjectsPlacementData, PlacementGridData placementData, GridManager gridManager) : base(inWallObjectsPlacementData, placementData, gridManager)
    {
    }

    /// <summary>
    /// Always allows the Remover to try removing objects on this selected space
    /// </summary>
    /// <param name="selectionData"></param>
    /// <returns></returns>
    protected override bool ValidatePlacement(SelectionData selectionData)
    {
        return true;
    }
}
