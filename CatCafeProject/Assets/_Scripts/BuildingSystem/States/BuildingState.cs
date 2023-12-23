using System;
using System.Linq;
using UnityEngine;

/// <summary>
/// Abstract class for any functionality related to placing objets on the map
/// </summary>
public abstract class BuildingState
{
    protected PlacementGridData currentPlacementData;
    public PlacementGridData CurrentPlacementData => currentPlacementData;
    protected GridData gridData;
    protected GridManager gridManager;
    protected ItemData ItemData;
    protected PlacementSelector placementSelection;
    protected SelectionData selectionData;
    public SelectionData SelectionData => selectionData;

    //To inform other objects about the changes in the selection or when we want to finish selectino
    //we will send those 2 events
    public Action<SelectionResult> OnFinished, OnSelectionChanged;

    public BuildingState(GridManager gridManager, GridData gridData, ItemData itemData)
    {
        this.gridManager = gridManager;
        this.gridData = gridData;
        this.ItemData = itemData;
    }

    protected void ConnectToPlacementSelection()
    {
        placementSelection.OnSelectionChanged += (selectionResult)=> OnSelectionChanged?.Invoke(selectionResult);
        placementSelection.OnSelectionFinished += (selectionResult) => OnFinished?.Invoke(selectionResult);
    }

    public virtual void HandleSelectionFinished()
        =>placementSelection.HandleSelectionFinished();
    

    public virtual void HandleSelectionStarted(Vector3 selectedMapPosition)
        => placementSelection.HandleSelectionStarted(selectedMapPosition);

    /// <summary>
    /// Rotation can only be 0, 90, 180, 270 so that is why we use a modifier 0-3 and multiply it by 90
    /// </summary>
    /// <param name="modifier"></param>
    public virtual void HandleRotation(int modifier)
    {
        placementSelection.HandleRotation(Quaternion.Euler(0, 90 * modifier, 0));
        placementSelection.Refresh();
    }

    public virtual void HandleSelectionChanged(Vector3 selectedMapPosition)
        => placementSelection.HandleMouseMovement(selectedMapPosition);

    /// <summary>
    /// Allows us to refres the selection after placement / removing of an object to show
    /// if we can still place object here or not.
    /// </summary>
    public void RefreshSelection()
        => placementSelection.Refresh();

}