using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// The actual logic that modifies the button color and size to provide feedback
/// </summary>
public class ButtonHighlightController : MonoBehaviour
{
    [SerializeField]
    private List<ButtonFeedback> buttons = new();

    [SerializeField]
    AudioSource buttonClickSound;

    private void Awake()
    {
        if(buttons.Count == 0)
            buttons = new(GetComponentsInChildren<ButtonFeedback>());
        foreach(var button in buttons)
        {
            button.OnClicked += SelectionFeedback;
        }
    }

    public void ResetAll()
    {
        foreach (var button in buttons)
        {
            button.ResetButton();
        }
    }

    private void SelectionFeedback()
    {
        ResetAll();
        buttonClickSound.Play();
    }
}
