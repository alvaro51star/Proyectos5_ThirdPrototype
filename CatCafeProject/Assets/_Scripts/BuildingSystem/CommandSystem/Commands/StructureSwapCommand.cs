using CommandSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// This is specific for inWallPlacement. The command allows us to remove the wall objet and place the door / window object.
/// It also implements the reverse operation to undo the InWall placement
/// </summary>
public class StructureSwapCommand : ICommand
{
    PlacementManager placementManager;
    PlacementGridData placementData, previousPlacementData;
    ItemData itemData, previousItemData;
    GridManager gridManager;
    SelectionResult selectionResult, previousStructuresResult;

    public StructureSwapCommand(PlacementManager placementManager, PlacementGridData placementData, PlacementGridData previousPlacementData, GridManager gridManager, ItemData itemData, SelectionResult selectionResult, ItemData previousItemData)
    {
        this.placementManager = placementManager;
        this.selectionResult = selectionResult;
        this.placementData = placementData;
        this.previousPlacementData = previousPlacementData;
        this.gridManager = gridManager;
        this.itemData = itemData;
        this.previousItemData = previousItemData;
    }

    public bool CanExecute()
    {
        return selectionResult.placementValidity;
    }

    public void Execute()
    {
        //We need to access all the edges to ocupy
        List<Edge> edgesOccupied = this.previousPlacementData.GetEdgePositions(selectionResult.selectedGridPositions[0], itemData.size, Mathf.RoundToInt(selectionResult.selectedPositionGridCheckRotation[0].eulerAngles.y));
        //Since in our Data we store edges from BottomLeft to TopRight we need to calculate rotation to be either Up (270) or Right (0)
        //This is needed to find the wall edges to remove / replace
        int objectsEulerRotationY = Mathf.RoundToInt(selectionResult.selectedPositionGridCheckRotation[0].eulerAngles.y);
        int objectToRemoveRotation = objectsEulerRotationY == 90 || objectsEulerRotationY == 270 ? 270 : 0;
        Quaternion newRotation = Quaternion.Euler(0, objectToRemoveRotation, 0);

        //We need this data to create the SelectionResult object that will work with our placement / remove functionality
        List<Vector3Int> positionsToClear = new();
        List<Vector3> placementPositions = new();
        foreach (Edge edge in edgesOccupied)
        {
            positionsToClear.Add(edge.smallerPoint);
            placementPositions.Add(gridManager.GetWorldPosition(edge.smallerPoint));
        }
        List<Quaternion> newRotationValues = positionsToClear.Select(x => newRotation).ToList();


        previousStructuresResult = new SelectionResult
        {
            selectedGridPositions = positionsToClear,
            selectedPositions = placementPositions,
            selectedPositionsObjectRotation = newRotationValues,
            selectedPositionGridCheckRotation = newRotationValues,
            isEdgeStructure = selectionResult.isEdgeStructure,
            placementValidity = true,
            size = this.previousItemData.size
        };

        //Removes walls (using the previousPlacementData) and places the InWall objects in the placementData
        //We keep this data separate to ensure that we can't again place an inwall object on top of another inwall object
        placementManager.RemoveStructureAt(previousStructuresResult, this.previousPlacementData);
        placementManager.PlaceStructureAt(selectionResult, placementData, this.itemData);

    }

    public void Undo()
    {
        //Reverse operation of placing wall and removing the inwall object
        placementManager.RemoveStructureAt(selectionResult, placementData);
        placementManager.PlaceStructureAt(previousStructuresResult, previousPlacementData, this.previousItemData);
    }
}
