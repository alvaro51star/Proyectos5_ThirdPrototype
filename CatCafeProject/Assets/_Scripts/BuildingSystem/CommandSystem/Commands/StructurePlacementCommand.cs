using CommandSystem;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Command that allows to place object on the map and remove them if we need to undo this operation.
/// </summary>
public class StructurePlacementCommand : ICommand
{
    PlacementManager placementManager;
    PlacementGridData placementData;
    ItemData itemData;
    SelectionResult selectionResult;    

    public StructurePlacementCommand(
        PlacementManager placementManager, 
        PlacementGridData placementData,
        ItemData itemData, 
        SelectionResult selectionResult)
    {
        this.placementManager = placementManager;
        this.selectionResult = selectionResult;
        this.placementData = placementData;
        this.itemData = itemData;
    }

    public bool CanExecute()
    {
        return selectionResult.placementValidity;
    }

    public void Execute()
    {
        placementManager.PlaceStructureAt(selectionResult,placementData, this.itemData);

    }

    public void Undo()
    {
        placementManager.RemoveStructureAt(selectionResult, placementData);
    }
}
