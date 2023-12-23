using System.Collections.Generic;
using System.Linq;
using UnityEngine;

/// <summary>
/// Stores the positions and rotation of the objects that we want to place
/// </summary>
public class SelectionData
{
    private List<Vector3Int> SelectedGridPositions;
    private List<Vector3> PreviewPositions;
    private List<Vector3> selectedWorldPositions;
    private List<Quaternion> selectedPositionsRotation, selectedPositionGridCheckRotation;
    public ItemData PlacedItemData { get; private set; } 
    public bool PlacementValidity { set; get; }

    public Quaternion Rotation { set; get; } = Quaternion.identity;
    

    public SelectionData(ItemData placedItemData)
    {
        this.PlacedItemData = placedItemData;
        SelectedGridPositions = new();
        selectedWorldPositions = new();
        selectedPositionsRotation = new();
        selectedPositionGridCheckRotation = new();
        PreviewPositions = new();
        this.PlacementValidity = false;
    }

    public void Clear()
    {
        SelectedGridPositions.Clear();
        selectedWorldPositions.Clear();
        selectedPositionsRotation.Clear();
        selectedPositionGridCheckRotation.Clear();
        PreviewPositions.Clear();
        this.PlacementValidity = false;
    }
    
    public void AddToGridPositions(Vector3Int pos) => SelectedGridPositions.Add(pos);
    public void AddToWorldPositions(Vector3 pos) => selectedWorldPositions.Add(pos);
    public void AddToPreviewPositions(Vector3 pos) => PreviewPositions.Add(pos);

    public List<Vector3Int> GetSelectedGridPositions() => new(this.SelectedGridPositions);
    public List<Vector3> GetSelectedWorldPositions() => new(this.selectedWorldPositions);
    public List<Vector3> GetPreviewGridPositions() => PreviewPositions.Count > 0 ? new(this.PreviewPositions) : GetSelectedWorldPositions();
    public List<Quaternion> GetSelectedPositionsObjectRotation() 
        => this.selectedPositionsRotation.Count > 0 ? new(this.selectedPositionsRotation) : selectedWorldPositions.Select(x => this.Rotation).ToList();
    public List<Quaternion> GetSelectedPositionsGridRotation()
        => this.selectedPositionGridCheckRotation.Count > 0 ? new(this.selectedPositionGridCheckRotation) : SelectedGridPositions.Select(x => this.Rotation).ToList();
    
    /// <summary>
    /// To be sure that I can store the data inside a Command (or to even later add some multithreading/async functionality) I create copies of the Lists of data 
    /// so that I am sure that it never changes in the copy of the SelectionResults that i have sent
    /// </summary>
    /// <returns></returns>
    public SelectionResult GetData() => new SelectionResult
    {
        selectedGridPositions = GetSelectedGridPositions(),
        selectedPositions = GetSelectedWorldPositions(),
        selectedPreviewPositions = GetPreviewGridPositions(),
        placementValidity = this.PlacementValidity,
        size = PlacedItemData.size,
        isEdgeStructure = PlacedItemData.objectPlacementType.IsEdgePlacement(),
        selectedPositionsObjectRotation = GetSelectedPositionsObjectRotation(),
        selectedPositionGridCheckRotation = GetSelectedPositionsGridRotation()
    };

    public void SetObjectRotation(List<Quaternion> objectsRotation)
    {
        this.selectedPositionsRotation = objectsRotation;
    }
    public void SetGridCheckRotation(List<Quaternion> gridCheckRotation)
    {
        this.selectedPositionGridCheckRotation = gridCheckRotation;
    }
}
