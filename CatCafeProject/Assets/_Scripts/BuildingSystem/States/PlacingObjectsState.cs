using CommandSystem;
using System;
using System.Collections;
using System.Linq;
using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// Allows to place different objects on the map by selecting an appropriate Selection strategy for object placement
/// </summary>
public class PlacingObjectsState : BuildingState
{
    public PlacingObjectsState(GridManager gridManager, GridData gridData, ItemData itemData) : base(gridManager, gridData, itemData)
    {
        selectionData = new SelectionData(itemData);
        if (itemData.objectPlacementType == PlacementType.Floor)
        {
            currentPlacementData = gridData.FloorPlacementData;
            placementSelection = new(new BoxSelection(currentPlacementData, gridManager), selectionData);
        }
        else if (itemData.objectPlacementType == PlacementType.Wall)
        {
            currentPlacementData = gridData.WallPlacementData;
            placementSelection = new(new WallPlacementStrategy(currentPlacementData, gridData.InWallPlacementData, gridData.ObjectPlacementData, gridManager), selectionData);
        }
        else if (itemData.objectPlacementType == PlacementType.InWalls)
        {
            currentPlacementData = gridData.InWallPlacementData;
            placementSelection = new(new InWallPlacementStrategy(gridData.WallPlacementData, currentPlacementData, gridManager), selectionData);
        }
        else if (itemData.objectPlacementType == PlacementType.FreePlacedObject)
        {
            currentPlacementData = gridData.ObjectPlacementData;
            placementSelection = new(new FreeObjectPlacementStrategy(currentPlacementData, gridData.WallPlacementData, gridData.InWallPlacementData, gridManager), selectionData);
        }
        else if (itemData.objectPlacementType == PlacementType.NearWallObject)
        {
            currentPlacementData = gridData.ObjectPlacementData;
            placementSelection = new(new NearWallPlacementStrategy(currentPlacementData, gridData.WallPlacementData, gridData.InWallPlacementData, gridManager), selectionData);
        }
        else
        {
            Debug.LogWarning($"Placement unsupported for the placementtype {itemData.objectPlacementType}");
            return;
        }
        ConnectToPlacementSelection();
    }

}
