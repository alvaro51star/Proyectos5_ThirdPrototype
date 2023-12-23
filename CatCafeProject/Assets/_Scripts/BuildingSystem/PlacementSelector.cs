using CommandSystem;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// High level method that connects the data "Selection Data" to a selection strategyu
/// while keeping the strategies disconnected from the PlacementManager
/// </summary>
public class PlacementSelector
{
    SelectionData selectionData;

    SelectionStrategy selectionStrategy;
    public SelectionStrategy CurrentSelectionStrategy => selectionStrategy;
    public event Action<SelectionResult> OnSelectionChanged, OnSelectionFinished;

    public PlacementSelector(SelectionStrategy placementStrategy, SelectionData selectionData)
    {
        this.selectionStrategy = placementStrategy;
        this.selectionData = selectionData;

    }

    /// <summary>
    /// Sends selection started message to a Selection Strategy
    /// also clearing the old selection data
    /// </summary>
    /// <param name="mousePosition"></param>
    public void HandleSelectionStarted(Vector3 mousePosition)
    {
        //This might be duplocated in the selection strategies
        selectionData.Clear();
        selectionStrategy.StartSelection(mousePosition, selectionData);
        SelectionResult data = selectionData.GetData();
        if(data.selectedGridPositions.Count > 0)
            OnSelectionChanged?.Invoke(data);
    }

    /// <summary>
    /// Ensures that the finished selection data is sent to other classes (PlacementManager)
    /// Also Resets the old selecton data
    /// </summary>
    public void HandleSelectionFinished()
    {
        SelectionResult data = selectionData.GetData();
        //Wall placement doesn't put anything in the gridpositions list.
        //This prevents from creating the placement process if the placement data is not correct
        if(data.selectedGridPositions.Count>0)
            OnSelectionFinished?.Invoke(data);
        ResetSelection();
    }

    /// <summary>
    /// Sends movement information to the Selecton Strategy while
    /// also updating the PlacementManager about the results
    /// </summary>
    /// <param name="mousePosition"></param>
    public void HandleMouseMovement(Vector3 mousePosition)
    {
        //If we havent modified the selection don't send any updates to other classes
        if (selectionStrategy.ModifySelection(mousePosition, selectionData))
        {
            SelectionResult data = selectionData.GetData();
            OnSelectionChanged?.Invoke(data);
        }

    }

    //Handlse the rotation (when player click , or . buttons)
    public void HandleRotation(Quaternion rotationAmount)
    {
        selectionData.Rotation *= rotationAmount;
        selectionData.Rotation  = selectionStrategy.HandleRotation(selectionData.Rotation, selectionData);
        Refresh();
        Debug.Log(selectionData.Rotation.eulerAngles);
    }

    /// <summary>
    /// Allows to refresh the selection in case the placement became valid / invalid
    /// </summary>
    internal void Refresh()
    {
        if (selectionData.GetSelectedGridPositions().Count <= 0)
            return;
        selectionStrategy.RefreshSelection(selectionData);
        OnSelectionChanged?.Invoke(selectionData.GetData());
    }

    /// <summary>
    /// Clears the selection and informas the strategy to finish
    /// </summary>
    public void ResetSelection()
    {
        selectionData.Clear();
        selectionStrategy.FinishSelection(selectionData);
    }
}


