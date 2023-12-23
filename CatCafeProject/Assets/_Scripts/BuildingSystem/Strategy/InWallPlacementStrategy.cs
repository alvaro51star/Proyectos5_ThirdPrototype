using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows to plcace Door and Window objects inside (instead of wall objects)
/// </summary>
public class InWallPlacementStrategy : SelectionStrategy
{
    protected PlacementGridData wallPlacementData;
    public InWallPlacementStrategy(PlacementGridData wallPlacementData, PlacementGridData placementData, GridManager gridManager) : base(placementData, gridManager)
    {
        this.wallPlacementData = wallPlacementData;
    }

    public override void StartSelection(Vector3 mousePosition, SelectionData selectionData)
    {
        this.lastDetectedPosition.Reset();
        ModifySelection(mousePosition, selectionData);
    }

    public override bool ModifySelection(Vector3 mousePosition, SelectionData selectionData)
    {
        Vector3Int tempPos = gridManager.GetCellPosition(mousePosition, selectionData.PlacedItemData.objectPlacementType);
        if (lastDetectedPosition.TryUpdatingPositon(tempPos))
        {
            //Clear selection data
            selectionData.Clear();

            selectionData.AddToWorldPositions(gridManager.GetWorldPosition(lastDetectedPosition.GetPosition()));

            selectionData.AddToGridPositions(lastDetectedPosition.GetPosition());
            selectionData.PlacementValidity = ValidatePlacement(selectionData);   
            return true;
        }
        return false;
    }

    protected override bool ValidatePlacement(SelectionData selectionData)
    {
        //Here we don't have to check if positions is valid because we can only place over the existing wall edge objects
        bool valid = PlacementValidator.CheckIfPositionsAreOccupied(
            selectionData.GetSelectedGridPositions(), 
            this.wallPlacementData, 
            selectionData.PlacedItemData.size, 
            selectionData.GetSelectedPositionsGridRotation(), 
            selectionData.PlacedItemData.objectPlacementType.IsEdgePlacement());
        //if (valid)
        //{
        //    //Checks if 
        //    valid = PlacementValidator.CheckIfPositionsAreFree(
        //    selectionData.GetSelectedGridPositions(),
        //    placementData,
        //    selectionData.PlacedItemData.size,
        //    selectionData.GetSelectedPositionsGridRotation(),
        //    selectionData.PlacedItemData.objectPlacementType.IsEdgePlacement());
        //}
        return valid;
    }

    public override void FinishSelection(SelectionData selectionData)
    {
        lastDetectedPosition.Reset();
    }
}
