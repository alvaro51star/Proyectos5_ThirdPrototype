using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Allows to place objects anywhere on the Cells of the map (with no other constraint than the fact if the area is free)
/// </summary>
public class FreeObjectPlacementStrategy : SelectionStrategy
{
    protected PlacementGridData wallPlacementData, inWallPlacementData;
    public FreeObjectPlacementStrategy(PlacementGridData placementData, PlacementGridData wallPlacementData, PlacementGridData inWallPlacementData, GridManager gridManager) : base(placementData, gridManager)
    {
        this.wallPlacementData = wallPlacementData;
        this.inWallPlacementData = inWallPlacementData;
    }

    /// <summary>
    /// In this placement strategy first selection is the last selection. We just sent the onFinished event when we let go of the mouse button
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <param name="selectionData"></param>
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

            if (selectionData.PlacedItemData.size.x == selectionData.PlacedItemData.size.y)
            {
                //for grid check purpose rotation remains 0
                selectionData.SetGridCheckRotation(new() { Quaternion.identity });
            }
            else
            {
 
                List<Quaternion> rotations = new() { HandleRotation(selectionData.Rotation,selectionData) };
                selectionData.SetGridCheckRotation(rotations);
                selectionData.SetObjectRotation(rotations);
            }

            selectionData.PlacementValidity = ValidatePlacement(selectionData);


            return true;
        }
        return false; 
    }
    protected override bool ValidatePlacement(SelectionData selectionData)
    {
        bool validity = PlacementValidator.CheckIfPositionsAreValid(
            selectionData.GetSelectedGridPositions(),
            placementData,
            selectionData.PlacedItemData.size,
            selectionData.GetSelectedPositionsGridRotation(),
            false);

        //Only if the previous check was TRUE
        if (validity)
        {
            //Checks if the placement position is free in the ObjectPlacementData
            validity = PlacementValidator.CheckIfPositionsAreFree(
            selectionData.GetSelectedGridPositions(),
            placementData,
            selectionData.PlacedItemData.size,
            selectionData.GetSelectedPositionsGridRotation(),
            false);
        }
        if(validity) //IN WALL Objects
        {
            //When placing objects we need to check if because of their size
            //ex 2x1 we are not trying to place the second tile inside a wall (crossing a wall)
            
            validity = PlacementValidator.CheckIfNotCrossingEdgeObject(
            selectionData.GetSelectedGridPositions(),
            inWallPlacementData, 
            selectionData.PlacedItemData.size,
            selectionData.GetSelectedPositionsGridRotation(),
            false);
        }
        if (validity) // WALL objects
        {
            //When placing objects we need to check if because of their size
            //ex 2x1 we are not trying to place the second tile inside a wall (crossing a wall)
            validity = PlacementValidator.CheckIfNotCrossingEdgeObject(
            selectionData.GetSelectedGridPositions(),
            wallPlacementData, 
            selectionData.PlacedItemData.size,
            selectionData.GetSelectedPositionsGridRotation(),
            false);
        }
        return validity;
    }

    /// <summary>
    /// Handles objects rotation. If object is 2x1 because of how our prefab is set up (we always start placement from the Bottom-Left corner of the object)
    /// we only allow the rotation to be 0 or 90. We can easily add "mirror" functionality to add this ability.
    /// This constraint is purely to keep the data storage easier.
    /// </summary>
    /// <param name="rotation"></param>
    /// <param name="selectionData"></param>
    /// <returns></returns>
    public override Quaternion HandleRotation(Quaternion rotation, SelectionData selectionData)
    {
        if (selectionData.PlacedItemData.size.x == selectionData.PlacedItemData.size.y)
            return rotation;

        int currentRotation = Mathf.RoundToInt(rotation.eulerAngles.y);

        Quaternion valueToReturn;
        //only allow to place object that hax size.X != size.Y horizontally Up or vertically to the Right
        if (selectionData.PlacedItemData.size.x > selectionData.PlacedItemData.size.y)
        {
            
            if (currentRotation == 0 || currentRotation == 180)
                valueToReturn = Quaternion.identity;
            else
                valueToReturn = Quaternion.Euler(0, 270, 0);
        }
        else
        {
            if (currentRotation == 0 || currentRotation == 180)
                valueToReturn = Quaternion.identity;
            else
                valueToReturn = Quaternion.Euler(0, 90, 0);
        }
        selectionData.SetObjectRotation(new() { valueToReturn });
        selectionData.SetGridCheckRotation(new() { valueToReturn });
        return valueToReturn;
    }

    public override void FinishSelection(SelectionData selectionData)
    {
        lastDetectedPosition.Reset();
    }

}
