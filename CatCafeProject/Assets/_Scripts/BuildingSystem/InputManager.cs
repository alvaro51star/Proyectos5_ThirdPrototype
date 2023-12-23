using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Input manager. We could ponentialy split the Raycast functionality outside of it and add the camera input ehre
/// to make it more universal and preserve Single Responsibility Rule better
/// </summary>
public class InputManager : MonoBehaviour
{
    [SerializeField]
    private Camera sceneCamera;

    private Vector3 lastPosition;

    [SerializeField]
    private LayerMask placementLayermask;

    public event Action OnMousePressed, OnMouseReleased, OnCancle, OnUndo;

    public event Action<int> OnRotate;

    public event Action<bool> OnToggleDelete;

    public Vector3 GetSelectedMapPosition()
    {
        Vector3 mousePos = Input.mousePosition;
        mousePos.z = sceneCamera.nearClipPlane;
        Ray ray = sceneCamera.ScreenPointToRay(mousePos);
        RaycastHit hit;
        if (Physics.Raycast(ray, out hit, 100, placementLayermask))
        {
            lastPosition = hit.point;
        }
        return lastPosition;
    }

    public bool IsInteractingWithUI()
        => EventSystem.current.IsPointerOverGameObject();

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
            OnCancle?.Invoke();
        if(Input.GetKeyDown(KeyCode.R))
            OnUndo?.Invoke();

        if (Input.GetMouseButtonDown(0))
            OnMousePressed?.Invoke();
        if (Input.GetMouseButtonUp(0))
            OnMouseReleased?.Invoke();
        if (Input.GetKeyDown(KeyCode.Comma))
            OnRotate?.Invoke(-1);
        if (Input.GetKeyDown(KeyCode.Period))
            OnRotate?.Invoke(1);

        if (Input.GetKeyDown(KeyCode.LeftControl))
            OnToggleDelete?.Invoke(true);
        if (Input.GetKeyUp(KeyCode.LeftControl))
            OnToggleDelete?.Invoke(false);
    }


}
