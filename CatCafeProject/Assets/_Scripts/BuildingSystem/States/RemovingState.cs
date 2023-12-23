using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.Windows;

/// <summary>
/// Allows to remove objets from the map.
/// Those Selections are differnt only that they don't validate placement but rather allows you to select anything on the map
/// and later OnFinish perform Removal of any objet that was selected.
/// </summary>
public class RemovingState : BuildingState
{
    public RemovingState(GridManager gridManager, GridData gridData, ItemData itemData) : base(gridManager, gridData, itemData)
    {
        this.ItemData = itemData;
        
        selectionData = new SelectionData(itemData);
        if (itemData.objectPlacementType == PlacementType.Wall)
        {
            currentPlacementData = gridData.WallPlacementData;
            placementSelection = new(new WallRemovalStrategy(currentPlacementData, gridData.InWallPlacementData, gridData.ObjectPlacementData, gridManager), selectionData);
        }
        if (itemData.objectPlacementType == PlacementType.InWalls)
        {
            currentPlacementData = gridData.InWallPlacementData;
            placementSelection = new(new InWallRemovalStrategy(gridData.WallPlacementData, currentPlacementData, gridManager), selectionData);
        }
        if (itemData.objectPlacementType == PlacementType.Floor)
        {
            currentPlacementData = gridData.FloorPlacementData;
            placementSelection = new(new FloorRemovalStrategy(currentPlacementData, gridManager), selectionData);
        }
        if (itemData.objectPlacementType == PlacementType.NearWallObject || itemData.objectPlacementType == PlacementType.FreePlacedObject)
        {
            currentPlacementData = gridData.ObjectPlacementData;
            placementSelection = new(new ObjectRemovalStrategy(currentPlacementData, gridData.WallPlacementData, gridData.InWallPlacementData, gridManager), selectionData);
        }

        ConnectToPlacementSelection();
    }

}
