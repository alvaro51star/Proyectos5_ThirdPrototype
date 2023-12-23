using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows to remve furniture objects from the map
/// </summary>
public class ObjectRemovalStrategy : FreeObjectPlacementStrategy
{
    public ObjectRemovalStrategy(PlacementGridData placementData, PlacementGridData wallPlacementData, PlacementGridData inWallPlacementData, GridManager gridManager) : base(placementData, wallPlacementData, inWallPlacementData, gridManager)
    {
    }

    protected override bool ValidatePlacement(SelectionData selectionData)
    {
        return true;
    }
}
