using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Allows to place Wall objects on the map
/// </summary>
public class WallPlacementStrategy : SelectionStrategy
{
    protected Vector3Int? startposition;
    protected PlacementGridData objectPlacementData, inWallPlacementData;
    public WallPlacementStrategy(PlacementGridData wallPlacementData, PlacementGridData inWallPlacementData, PlacementGridData objectPlacementData, GridManager gridManager) : base(wallPlacementData, gridManager)
    {
        this.objectPlacementData = objectPlacementData;
        this.inWallPlacementData = inWallPlacementData;
    }

    public override void StartSelection(Vector3 mousePosition, SelectionData selectionData)
    {
        selectionData.Clear();
        startposition = gridManager.GetCellPosition(mousePosition, selectionData.PlacedItemData.objectPlacementType);
        selectionData.AddToWorldPositions(gridManager.GetWorldPosition(startposition.Value));

        selectionData.PlacementValidity = true;

        lastDetectedPosition.TryUpdatingPositon(startposition.Value);
        Debug.Log($"Selection {lastDetectedPosition.GetPosition()}");
        if (selectionData.PlacementValidity == false)
            startposition = null;
    }

    public override bool ModifySelection(Vector3 mousePosition, SelectionData selectionData)
    {
        Vector3Int tempPos = gridManager.GetCellPosition(mousePosition, selectionData.PlacedItemData.objectPlacementType);
        if (lastDetectedPosition.TryUpdatingPositon(tempPos))
        {
            selectionData.Clear();
            if (startposition.HasValue && startposition.Value != lastDetectedPosition.lastPosition.Value)
            {
                List<Vector3Int> path = GridSelectionHelper.AStar(startposition.Value, tempPos, placementData);

                Vector3 worldPos;
                //We subtract 1 so that we place walls up to the cursor position (not on the position where the cursor is now)
                for (int i = 0; i < path.Count - 1; i++)
                {
                    worldPos = gridManager.GetWorldPosition(path[i]);
                    selectionData.AddToWorldPositions(worldPos);
                    selectionData.AddToPreviewPositions(worldPos);
                    selectionData.AddToGridPositions(path[i]);
                }
                //For wall preview we need to add the last selected position to show the preview correctly
                worldPos = gridManager.GetWorldPosition(lastDetectedPosition.lastPosition.Value);
                selectionData.AddToPreviewPositions(worldPos);

                //We use here the path since we need the last position (coursor position) for rotation calculation
                List<Quaternion> rotationValues = GridSelectionHelper.CalculateRotation(path);

                //To correctly show wall preview we need to add the rotation for the last position on the path
                //that was added earlier
                rotationValues.Add(rotationValues[^1]);

                selectionData.SetObjectRotation(rotationValues);
                selectionData.SetGridCheckRotation(rotationValues);
                selectionData.PlacementValidity = ValidatePlacement(selectionData);
            }
            else
            {
                selectionData.AddToWorldPositions(gridManager.GetWorldPosition(lastDetectedPosition.GetPosition()));
                selectionData.PlacementValidity = true;
                //selectionData.AddToGridPositions(lastDetectedPosition.GetPosition());
            }
            return true;
        }
        return false;
    }

    protected override bool ValidatePlacement(SelectionData selectionData)
    {
        //checks if the placement position is valid
        bool validity = PlacementValidator.CheckIfPositionsAreValid(
            selectionData.GetSelectedGridPositions(),
            placementData,
            selectionData.PlacedItemData.size,
            selectionData.GetSelectedPositionsGridRotation(),
            selectionData.PlacedItemData.objectPlacementType.IsEdgePlacement());
        if (validity)
        {
            //Checks if no other wall are at those positions
            validity = PlacementValidator.CheckIfPositionsAreFree(
            selectionData.GetSelectedGridPositions(),
            placementData,
            selectionData.PlacedItemData.size,
            selectionData.GetSelectedPositionsGridRotation(),
            selectionData.PlacedItemData.objectPlacementType.IsEdgePlacement());
        }
        if(validity)
        {
            //Checks if no other IN Wall objects are at those positions
            validity = PlacementValidator.CheckIfPositionsAreFree(
            selectionData.GetSelectedGridPositions(),
            inWallPlacementData,
            selectionData.PlacedItemData.size,
            selectionData.GetSelectedPositionsGridRotation(),
            selectionData.PlacedItemData.objectPlacementType.IsEdgePlacement());
        }
        if(validity)
        {
            //Checks if we are not trying to place wall inside a furniture object
            validity = PlacementValidator.CheckIfNotCrossingMultiCellObject(
                selectionData.GetSelectedGridPositions(),
                objectPlacementData,
                selectionData.PlacedItemData.size,
                selectionData.GetSelectedPositionsGridRotation(),
                selectionData.PlacedItemData.objectPlacementType.IsEdgePlacement());
        }
        return validity;
    }

    public override void FinishSelection(SelectionData selectionData)
    {
        startposition = null;
        lastDetectedPosition.Reset();
    }
}
