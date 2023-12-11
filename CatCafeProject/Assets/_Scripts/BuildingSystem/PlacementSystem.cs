using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;


public class PlacementSystem : MonoBehaviour
{
    [SerializeField]
    private GameObject mouseIndicator, cellIndicator;
    [SerializeField]
    private InputManager inputManager;
    [SerializeField]
    private Grid grid;

    [SerializeField]
    private ObjectsDatabaseSO database;
    private int SelectedObjectIndex = -1;

    [SerializeField]
    private GameObject gridVisualization;



    private void Update()
    {
        if (SelectedObjectIndex < 0)
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        mouseIndicator.transform.position = mousePosition;
        cellIndicator.transform.position = grid.CellToWorld(gridPosition);
    }

    private void Start()
    {
        StopPlacement();
    }

    private void StopPlacement()
    {
        SelectedObjectIndex = -1;
        gridVisualization.SetActive(false);
        cellIndicator.SetActive(false);
        inputManager.OnClicked -= PlaceStructure;
        inputManager.OnClicked -= StopPlacement;
    }

    public void StartPlacement(int ID)
    {
        StopPlacement();
        SelectedObjectIndex = database.objectDatas.FindIndex(data => data.ID == ID);
        if (SelectedObjectIndex < 0)
        {
            Debug.LogError($"No ID found {ID}");
            return;
        }

        gridVisualization.SetActive(true);
        cellIndicator.SetActive(true);
        inputManager.OnClicked += PlaceStructure;
        inputManager.OnClicked += StopPlacement;
    }

    private void PlaceStructure()
    {
        if (inputManager.IsPointerOverUI())
        {
            return;
        }
        Vector3 mousePosition = inputManager.GetSelectedMapPosition();
        Vector3Int gridPosition = grid.WorldToCell(mousePosition);
        GameObject newObject = Instantiate(database.objectDatas[SelectedObjectIndex].Prefab);
        newObject.transform.position = grid.CellToWorld(gridPosition);

    }
}
