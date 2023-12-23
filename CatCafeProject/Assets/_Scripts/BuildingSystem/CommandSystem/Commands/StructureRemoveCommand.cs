using CommandSystem;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UIElements;

/// <summary>
/// Command that allows to remove objet and undo the removal if needed
/// </summary>
public class StructureRemoveCommand : ICommand
{
    PlacementManager placementManager;
    PlacementGridData placementData;
    ItemData itemData;
    GridManager gridManager;
    SelectionResult selectionResult, selectionResultToRestore;

    public StructureRemoveCommand(GridManager gridManager, PlacementManager placementManager, PlacementGridData placementData, ItemData itemData, SelectionResult selectionResult)
    {
        this.placementManager = placementManager;
        this.selectionResult = selectionResult;
        this.placementData = placementData;
        this.itemData = itemData;
        this.gridManager = gridManager;
    }

    public bool CanExecute()
    {
        GenerateUndoData();
        //If there is nothing to remove (remove selection selects empty spaces)
        //we don't want to perform this command and add it to a stack of commands to undo
        return selectionResultToRestore.selectedGridPositions.Count > 0;
    }

    public void Execute()
    {
        if(selectionResultToRestore.selectedGridPositions == null)
            GenerateUndoData();

        placementManager.RemoveStructureAt(selectionResult, placementData);

    }

    private void GenerateUndoData()
    {
        //We need to save all this data to be able to undo the remove operation
        List<Vector3Int> occupiedCellsGridPositions = new();
        List<Vector3> occupiedCellsPosition = new();
        List<Quaternion> occupiedCellsRotation = new();
        List<Quaternion> occupiedCellsGridCheckRotation = new();
        if (itemData.objectPlacementType.IsEdgePlacement())
        {

            HashSet<Edge> checkedEdges = new HashSet<Edge>();
            //We need to access all the edges to ocupy
            foreach (var pos in selectionResult.selectedGridPositions)
            {
                int rotation = Mathf.RoundToInt(selectionResult.selectedPositionGridCheckRotation[0].eulerAngles.y);
                List<Edge> edgesToCheck = this.placementData.GetEdgePositions(pos, itemData.size, rotation);

                foreach (var edge in edgesToCheck)
                {
                    if (checkedEdges.Contains(edge))
                        continue;
                    if (this.placementData.IsEdgeObjectAt(edge))
                    {
                        int savedRotation = 270;
                        if (rotation == 0 || rotation == 180)
                            savedRotation = 0;

                        List<Edge> edges =  this.placementData.GetEdgesOccupiedForEdgeObject(edge);
                        checkedEdges.AddRange(edges);

                        Vector3Int newOrigin = edges.OrderBy(e => e.smallerPoint.x).ThenBy(e => e.smallerPoint.z).First().smallerPoint;
                        occupiedCellsRotation.Add(Quaternion.Euler(0, savedRotation, 0));
                        occupiedCellsGridCheckRotation.Add(Quaternion.Euler(0, savedRotation, 0));
                        occupiedCellsGridPositions.Add(newOrigin);
                        occupiedCellsPosition.Add(this.gridManager.GetWorldPosition(newOrigin));

                        ////If we have rotated the Edge objects we need to consider which edge vertex is actuall selected
                        ////in the Remove State selection result (we can be selectiong from left to right but the Edge
                        ////record saves the vertices from Right to Left - smaller first and bigger later)
                        //Vector3Int newOriginVertex = edge.smallerPoint;
                        //if (rotation == 90 || rotation == 180)
                        //    newOriginVertex = edge.biggerPoint;

                        ////If our selection doesnt incldue this edge we want to ignore this edge as it is probably
                        ////part of a bigget edge object plcamanet
                        //if (this.selectionResult.selectedGridPositions.Contains(newOriginVertex) == false)
                        //    continue;

                        ////Even though the original edge object was placed with rotation  0 if we are removing this edge object
                        ////by selecting from Right to Left (rotation = 180) we can place it again with that rotation 
                        ////if we want to Undo the remove command
                        //occupiedCellsGridPositions.Add(newOriginVertex);
                        //occupiedCellsPosition.Add(this.gridManager.GetWorldPosition(newOriginVertex));
                        ////For the wall placement we set the same rotation for cell rotation and the Grid check rotation
                        //occupiedCellsRotation.Add(Quaternion.Euler(0, rotation, 0));
                        //occupiedCellsGridCheckRotation.Add(Quaternion.Euler(0, rotation, 0));
                    }
                }
            }
        }
        else
        {

            foreach (var pos in selectionResult.selectedGridPositions)
            {
                List<Vector3Int> cellsToCheck = this.placementData.GetCellPositions(pos, itemData.size, Mathf.RoundToInt(selectionResult.selectedPositionGridCheckRotation[0].eulerAngles.y));
                foreach (var cell in cellsToCheck)
                {
                    if (this.placementData.IsCellObjectAt(cell) && this.selectionResult.selectedGridPositions.Contains(cell))
                    {
                        Vector3Int placementOriginPosition = this.placementData.GetOriginForCellObject(cell).Value;
                        occupiedCellsGridPositions.Add(placementOriginPosition);
                        int index = selectionResult.selectedGridPositions.IndexOf(cell);
                        occupiedCellsPosition.Add(this.gridManager.GetWorldPosition(placementOriginPosition));
                        occupiedCellsRotation.Add(selectionResult.selectedPositionsObjectRotation[index]);
                        occupiedCellsGridCheckRotation.Add(selectionResult.selectedPositionGridCheckRotation[index]);
                    }
                }
            }
        }

        //We save the existing objects data in case we need to undo this remvoe step
        selectionResultToRestore = new SelectionResult
        {
            isEdgeStructure = itemData.objectPlacementType.IsEdgePlacement(),
            placementValidity = true,
            selectedGridPositions = occupiedCellsGridPositions,
            selectedPositions = occupiedCellsPosition,
            selectedPositionGridCheckRotation = occupiedCellsGridCheckRotation,
            selectedPositionsObjectRotation = occupiedCellsRotation
        };
    }

    public void Undo()
    {
        placementManager.PlaceStructureAt(selectionResultToRestore, placementData, itemData);
    }
}
