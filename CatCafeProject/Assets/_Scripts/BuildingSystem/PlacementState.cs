using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlacementState : IBuildingState
{
    private int selectedObjectIndex = -1;
    int ID;
    Grid grid;
    PreviewSystem previewSystem;
    ObjectsDatabaseSO database;
    GridData floorData;
    GridData furnitureData;
    ObjectPlacer objectPlacer;

    public PlacementState(int iD, Grid grid, PreviewSystem previewSystem, ObjectsDatabaseSO database, GridData floorData, GridData furnitureData, ObjectPlacer objectPlacer)
    {
        ID = iD;
        this.grid = grid;
        this.previewSystem = previewSystem;
        this.database = database;
        this.floorData = floorData;
        this.furnitureData = furnitureData;
        this.objectPlacer = objectPlacer;

        selectedObjectIndex = database.objectData.FindIndex(data => data.ID == ID);
        if (selectedObjectIndex > -1)
        {
            previewSystem.StartShowingPlacementPreview(
                database.objectData[selectedObjectIndex].Prefab,
                database.objectData[selectedObjectIndex].Size);
        }
        else
        {
            throw new System.Exception($"No Object in ID {iD}");
        }
    }

    public void EndState()
    {
        previewSystem.StopShowingPreview();
    }

    public void OnAction(Vector3Int gridPosition) //!Aqui es donde iria la comprobacion del dinero
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);
        if (placementValidity == false)
        {
            //Aqui va el sonido de no se puede colocar
            return;
        }

        //Aqui el de se coloca correctamente
        int index = objectPlacer.PlaceObject(
            database.objectData[selectedObjectIndex].Prefab,
            grid.CellToWorld(gridPosition),
            database.objectData[selectedObjectIndex]);

        GridData selectedData = database.objectData[selectedObjectIndex].furnitureType == FurnitureType.Carpet ? floorData : furnitureData;

        selectedData.AddObjectAt(gridPosition,
            database.objectData[selectedObjectIndex].Size,
            database.objectData[selectedObjectIndex].ID,
            index);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), false);
    }

    private bool CheckPlacementValidity(Vector3Int gridPosition, int selectedObjectIndex)
    {
        GridData selectedData = database.objectData[selectedObjectIndex].furnitureType == FurnitureType.Carpet ? floorData : furnitureData;

        return selectedData.CanPlaceObjectAt(gridPosition, database.objectData[selectedObjectIndex].Size);
    }

    public void UpdateState(Vector3Int gridPosition)
    {
        bool placementValidity = CheckPlacementValidity(gridPosition, selectedObjectIndex);

        previewSystem.UpdatePosition(grid.CellToWorld(gridPosition), placementValidity);
    }
}
