using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Constraing the furniture placement to be near wall (wall needs to be in the (0,0,1) direction from the objects prefab
/// </summary>
public class NearWallPlacementStrategy : FreeObjectPlacementStrategy
{
    public NearWallPlacementStrategy(PlacementGridData placementData, PlacementGridData wallPlacementData, PlacementGridData inWallPlacementData, GridManager gridManager) : base(placementData, wallPlacementData, inWallPlacementData, gridManager)
    {
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
        //Checks if the position is valid
        bool validity = PlacementValidator.CheckIfPositionsAreValid(
            selectionData.GetSelectedGridPositions(),
            placementData,
            selectionData.PlacedItemData.size,
            selectionData.GetSelectedPositionsGridRotation(),
            selectionData.PlacedItemData.objectPlacementType.IsEdgePlacement());

        //Perform check only if the last one returned TRUE
        if (validity)
        {
            validity = PlacementValidator.CheckIfPositionsAreFree(
            selectionData.GetSelectedGridPositions(),
            placementData,
            selectionData.PlacedItemData.size,
            selectionData.GetSelectedPositionsGridRotation(),
            selectionData.PlacedItemData.objectPlacementType.IsEdgePlacement());
        }
        if (validity)
        {
            //checks if we are trying to place the object near wall
            //this checks in up/forward when the rotation is 0 degrees
            validity = PlacementValidator.CheckIfPositionsAreNearWall(
                selectionData.GetSelectedGridPositions(),
                wallPlacementData,
                selectionData.PlacedItemData.size,
                selectionData.GetSelectedPositionsGridRotation(),
                selectionData.PlacedItemData.objectPlacementType.IsEdgePlacement());
            if(!validity)
            {
                //Allows to place object near windows and doors
                validity = PlacementValidator.CheckIfPositionsAreNearWall(
                selectionData.GetSelectedGridPositions(),
                inWallPlacementData,
                selectionData.PlacedItemData.size,
                selectionData.GetSelectedPositionsGridRotation(),
                selectionData.PlacedItemData.objectPlacementType.IsEdgePlacement());
            }
        }
        if (validity)
        {
            //Checks if we are not crossing any WALLS
            validity = PlacementValidator.CheckIfNotCrossingEdgeObject(
                selectionData.GetSelectedGridPositions(),
                wallPlacementData,
                selectionData.PlacedItemData.size,
                selectionData.GetSelectedPositionsGridRotation(),
                selectionData.PlacedItemData.objectPlacementType.IsEdgePlacement());
        }
        if (validity)
        {
            //Checks if we are not crossing any In WALL objects
            validity = PlacementValidator.CheckIfNotCrossingEdgeObject(
                selectionData.GetSelectedGridPositions(),
                inWallPlacementData,
                selectionData.PlacedItemData.size,
                selectionData.GetSelectedPositionsGridRotation(),
                selectionData.PlacedItemData.objectPlacementType.IsEdgePlacement());
        }
        return validity;
    }

    public override Quaternion HandleRotation(Quaternion rotation, SelectionData selectionData)
    {
        selectionData.SetObjectRotation(new() { rotation });
        selectionData.SetGridCheckRotation(new() { rotation });
        return rotation;
    }
}
