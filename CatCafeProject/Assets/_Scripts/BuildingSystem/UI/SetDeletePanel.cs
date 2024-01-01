using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetDeletePanel : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;

    [SerializeField] public GameObject DeletePanel;

    private void OnEnable()
    {
        inputManager.OnToggleDelete += ToggleDeletePanel;
    }

    private void OnDisable()
    {
        inputManager.OnToggleDelete -= ToggleDeletePanel;
    }

    private void ToggleDeletePanel(bool status)
    {
        DeletePanel.SetActive(status);
    }
}
