using System.Linq;
using UnityEngine;

/// <summary>
/// Allows to select a box between Start and End position of the selection
/// </summary>
public class BoxSelection : SelectionStrategy
{
    //Start position is the first point that we select
    protected Vector3Int? startposition;

    delegate void ProcessPositionAction(SelectionData selectionData, Vector3Int tempPos, int x, int y);

    public BoxSelection(PlacementGridData placementData, GridManager gridManager) : base(placementData, gridManager)
    {
    }

    public override bool ModifySelection(Vector3 mousePosition, SelectionData selectionData)
    {
        //This runs before the StartPosition is set (since it is used to update the preview)
        //So we need a separate branch of code that will add only 1 currently selected tile unless
        //the start position is populated
        Vector3Int tempPos = gridManager.GetCellPosition(mousePosition, selectionData.PlacedItemData.objectPlacementType);
        if (lastDetectedPosition.TryUpdatingPositon(tempPos))
        {
            //clear old selection
            selectionData.Clear();

            //popuate lists with new selection
            if (startposition.HasValue) 
            {
                Vector2Int minPoint = new(Mathf.Min(startposition.Value.x, tempPos.x), Mathf.Min(startposition.Value.z, tempPos.z));
                Vector2Int maxPoint = new(Mathf.Max(startposition.Value.x, tempPos.x), Mathf.Max(startposition.Value.z, tempPos.z));
                 
                //Select grid positions in such a way that the start position is always included 
                //This will help us avoid inconsistency when placing a bigger object than 1x1
                if (startposition.Value.x == maxPoint.x && startposition.Value.z == maxPoint.y)
                {
                    SelectFromTopRightCorner(selectionData, tempPos, minPoint, maxPoint);
                }
                else if (startposition.Value.z == maxPoint.y && startposition.Value.x < maxPoint.x )
                {
                    SelectFromTopLeftCorner(selectionData, tempPos, minPoint, maxPoint);
                }
                else if (startposition.Value.x == maxPoint.x && startposition.Value.z < maxPoint.y)
                {
                    SelectFromBottomRightCorner(selectionData, tempPos, minPoint, maxPoint);
                }
                else
                {
                    SelectFromBottomLeftCorner(selectionData, tempPos, minPoint, maxPoint);
                }
            }
            else
            {
                selectionData.AddToGridPositions(lastDetectedPosition.GetPosition());
                selectionData.AddToWorldPositions(gridManager.GetWorldPosition(lastDetectedPosition.GetPosition()));
            }
            selectionData.SetGridCheckRotation(selectionData.GetSelectedGridPositions().Select(rotation => Quaternion.identity).ToList());

            selectionData.PlacementValidity = ValidatePlacement(selectionData);
            return true;
        }
        return false;
    }

    /// <summary>
    /// Strategy to select cells starting at the Bottom-Left corner.
    /// This is important as we always want to place the Floor object where we have clikced.
    /// In case its size is bigger then 1 we would see inconsistenacy in selection / placement
    /// if we didnt ensure that.
    /// </summary>
    /// <param name="selectionData"></param>
    /// <param name="tempPos"></param>
    /// <param name="minPoint"></param>
    /// <param name="maxPoint"></param>
    private void SelectFromBottomLeftCorner(SelectionData selectionData, Vector3Int tempPos, Vector2Int minPoint, Vector2Int maxPoint)
    {
        foreach (int x in GridSelectionHelper.MoveMinToMaxInclusive(minPoint.x,maxPoint.x,selectionData.PlacedItemData.size.x))
        {
            foreach (int y in GridSelectionHelper.MoveMinToMaxInclusive(minPoint.y, maxPoint.y, selectionData.PlacedItemData.size.y))
            {
                Vector3Int pos = new Vector3Int(x, tempPos.y, y);
                AddToSelection(pos, selectionData);
            }
        }
    }

    /// <summary>
    /// Strategy to select cells starting at the Bottom-Right corner.
    /// This is important as we always want to place the Floor object where we have clikced.
    /// In case its size is bigger then 1 we would see inconsistenacy in selection / placement
    /// if we didnt ensure that.
    /// </summary>
    /// <param name="selectionData"></param>
    /// <param name="tempPos"></param>
    /// <param name="minPoint"></param>
    /// <param name="maxPoint"></param>
    private void SelectFromBottomRightCorner(SelectionData selectionData, Vector3Int tempPos, Vector2Int minPoint, Vector2Int maxPoint)
    {
        foreach (int x in GridSelectionHelper.MoveMaxToMinInclusive(minPoint.x, maxPoint.x, selectionData.PlacedItemData.size.x))
        {
            foreach (int y in GridSelectionHelper.MoveMinToMaxInclusive(minPoint.y, maxPoint.y, selectionData.PlacedItemData.size.y))
            {
                Vector3Int pos = new Vector3Int(x, tempPos.y, y);
                AddToSelection(pos, selectionData);
            }
        }
    }

    /// <summary>
    /// Strategy to select cells starting at the Top-Left corner.
    /// This is important as we always want to place the Floor object where we have clikced.
    /// In case its size is bigger then 1 we would see inconsistenacy in selection / placement
    /// if we didnt ensure that.
    /// </summary>
    /// <param name="selectionData"></param>
    /// <param name="tempPos"></param>
    /// <param name="minPoint"></param>
    /// <param name="maxPoint"></param>
    private void SelectFromTopLeftCorner(SelectionData selectionData, Vector3Int tempPos, Vector2Int minPoint, Vector2Int maxPoint)
    {
        foreach (int x in GridSelectionHelper.MoveMinToMaxInclusive(minPoint.x, maxPoint.x, selectionData.PlacedItemData.size.x))
        {
            foreach (int y in GridSelectionHelper.MoveMaxToMinInclusive(minPoint.y, maxPoint.y, selectionData.PlacedItemData.size.y))
            {
                Vector3Int pos = new Vector3Int(x, tempPos.y, y);
                AddToSelection(pos, selectionData);
            }
        }
    }

    /// <summary>
    /// Strategy to select cells starting at the Top-Right corner.
    /// This is important as we always want to place the Floor object where we have clikced.
    /// In case its size is bigger then 1 we would see inconsistenacy in selection / placement
    /// if we didnt ensure that.
    /// </summary>
    /// <param name="selectionData"></param>
    /// <param name="tempPos"></param>
    /// <param name="minPoint"></param>
    /// <param name="maxPoint"></param>
    private void SelectFromTopRightCorner(SelectionData selectionData, Vector3Int tempPos, Vector2Int minPoint, Vector2Int maxPoint)
    {
        foreach (int x in GridSelectionHelper.MoveMaxToMinInclusive(minPoint.x, maxPoint.x, selectionData.PlacedItemData.size.x))
        {
            foreach (int y in GridSelectionHelper.MoveMaxToMinInclusive(minPoint.y, maxPoint.y, selectionData.PlacedItemData.size.y))
            {
                Vector3Int pos = new Vector3Int(x, tempPos.y, y);
                AddToSelection(pos, selectionData);
            }
        }
    }

    private void AddToSelection(Vector3Int position, SelectionData selectionData)
    {
        selectionData.AddToGridPositions(position);
        selectionData.AddToWorldPositions(gridManager.GetWorldPosition(position));
    }

    /// <summary>
    /// Handles selecting first position where we want to place a floor cell
    /// </summary>
    /// <param name="mousePosition"></param>
    /// <param name="selectionData"></param>
    public override void StartSelection(Vector3 mousePosition, SelectionData selectionData)
    {
        selectionData.Clear();
        startposition = gridManager.GetCellPosition(mousePosition, selectionData.PlacedItemData.objectPlacementType);
        //Debug.Log($"Start position {startposition.Value}");

        selectionData.AddToGridPositions(startposition.Value);
        selectionData.AddToWorldPositions(gridManager.GetWorldPosition(startposition.Value));

        selectionData.SetGridCheckRotation(selectionData.GetSelectedGridPositions().Select(rotation => Quaternion.identity).ToList());

        selectionData.PlacementValidity = ValidatePlacement(selectionData);
        lastDetectedPosition.TryUpdatingPositon(startposition.Value);
        Debug.Log($"Selection {lastDetectedPosition.GetPosition()}");
        if (selectionData.PlacementValidity == false)
            startposition = null;
    }

    protected override bool ValidatePlacement(SelectionData selectionData)
    {
        //Checks if the object(floor that by default is 2x2) will fit - won't go over the grid space
        bool validity = PlacementValidator.CheckIfPositionsAreValid(
            selectionData.GetSelectedGridPositions(),
            placementData,
            selectionData.PlacedItemData.size,
            selectionData.GetSelectedPositionsGridRotation(),
            selectionData.PlacedItemData.objectPlacementType.IsEdgePlacement());
        
        //Only if the previous check resulted in TRUE
        if (validity)
        {
            //Check if there are no floor objects already placed on the selected area
            validity = PlacementValidator.CheckIfPositionsAreFree(
            selectionData.GetSelectedGridPositions(),
            placementData,
            selectionData.PlacedItemData.size,
            selectionData.GetSelectedPositionsGridRotation(),
            selectionData.PlacedItemData.objectPlacementType.IsEdgePlacement());
        }

        return validity;
    }

    /// <summary>
    /// Resets the start position so that we can start selection again a new box
    /// </summary>
    /// <param name="selectionData"></param>
    public override void FinishSelection(SelectionData selectionData)
    {
        startposition = null;
        lastDetectedPosition.Reset();
    }
}

