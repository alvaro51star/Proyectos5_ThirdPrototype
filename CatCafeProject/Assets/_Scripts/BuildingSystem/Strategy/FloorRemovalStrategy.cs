using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows to remove Wall objects from the map
/// </summary>
public class FloorRemovalStrategy : BoxSelection
{
    public FloorRemovalStrategy(PlacementGridData placementData, GridManager gridManager) : base(placementData, gridManager)
    {
    }

    /// <summary>
    /// Ensures that we can always perform Removal by returning TRUE
    /// </summary>
    /// <param name="selectionData"></param>
    /// <returns></returns>
    protected override bool ValidatePlacement(SelectionData selectionData)
    {
        return true;
    }
}
