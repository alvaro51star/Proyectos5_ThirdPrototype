using UnityEngine;
using UnityEngine.Events;

/// <summary>
/// Controller to separate the UI elements (buttons) from directly being called by the Placement system (any system)
/// This allows us to reuse the ui without having to import the PlacementManager and all it depends on
/// The downside is that it communicates using events so it is harder to debug the connections
/// </summary>
public class UIPlacementController : MonoBehaviour
{
    public UnityEvent<int> OnObjectSelected;
    public UnityEvent OnUndoRequested, OnMoveRequest, OnResetMovementButton, OnCancelPlacement, OnMovementStateEntered;

    public void SelectObjectWithIndex(int index)
        => OnObjectSelected?.Invoke(index); 

    public void RequestUndo() 
        => OnUndoRequested?.Invoke();

    public void MoveRequest()
        => OnMoveRequest?.Invoke();

    public void ResetMovementButton()
        => OnResetMovementButton?.Invoke();

    public void CancelPlacementRequested()
        => OnCancelPlacement?.Invoke();

    public void EnterMovementState()
        => OnMovementStateEntered?.Invoke();
}
