using System;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Abstract definition of a selection strategy
/// </summary>
public abstract class SelectionStrategy
{
    protected GridManager gridManager;
    protected LastDetectedPositon lastDetectedPosition;
    protected PlacementGridData placementData;

    public SelectionStrategy(PlacementGridData placementData, GridManager gridManager)
    {
        this.lastDetectedPosition = new LastDetectedPositon();
        this.placementData = placementData;
        this.gridManager = gridManager;
    }

    /// <summary>
    /// Called on every update of PlacementManager to get the most recent position of the mouse 
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <param name="selectionData"></param>
    /// <returns></returns>
    public abstract bool ModifySelection(Vector3 mousePosition, SelectionData selectionData);

    /// <summary>
    /// Allows us to recheck placement validity - ex after placing an object we still are selecting the same space
    /// but now we can't place the object here again.
    /// </summary>
    /// <param name="selectionData"></param>
    public void RefreshSelection(SelectionData selectionData)
    {
        selectionData.PlacementValidity = ValidatePlacement(selectionData);
    }

    /// <summary>
    /// Allows each strategy to reset all the internally stored variables used for the selection (ex startPosition)
    /// </summary>
    /// <param name="selectionData"></param>
    public abstract void FinishSelection(SelectionData selectionData);

    /// <summary>
    /// Happens the moment we press the Left mouse button to start the selection
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <param name="selectionData"></param>
    public abstract void StartSelection(Vector3 mousePosition, SelectionData selectionData);

    /// <summary>
    /// Since rotation can be different for different obejcts but has a default behaviour of accepting the input rotation 
    /// it is a virtual method.
    /// </summary>
    /// <param name="rotation"></param>
    /// <param name="selectionData"></param>
    /// <returns></returns>
    public virtual Quaternion HandleRotation(Quaternion rotation, SelectionData selectionData)
    {
        return rotation;
    }

    /// <summary>
    /// Each placement system has a different validation logic - ex NearWallobjects needs to check if they have wall nearby
    /// </summary>
    /// <param name="selectionData"></param>
    /// <returns></returns>
    protected abstract bool ValidatePlacement(SelectionData selectionData);
}