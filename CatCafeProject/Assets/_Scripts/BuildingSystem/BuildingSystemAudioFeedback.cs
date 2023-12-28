using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Audio feedback system for the Placement Manager
/// </summary>
public class BuildingSystemAudioFeedback : MonoBehaviour
{
    [SerializeField] 
    private AudioSource audioSource;

    [SerializeField]
    private AudioClip undoClip, placeConstructionObjectClip, placeFurnitureClip, removeObjectClip, rotateClip, wrongPlacementClip;

    public void PlayUndoSound() => PlayPlaceObject(undoClip);

    public void PlayPlaceFurniture() => PlayPlaceObject(placeFurnitureClip);
    public void PlayPlaceConstruction() => PlayPlaceObject(placeConstructionObjectClip);
    public void PlayRemove() => PlayPlaceObject(removeObjectClip);
    public void PlayRotate() => PlayPlaceObject(rotateClip);
    public void PlayWrongPlacement() => PlayPlaceObject(wrongPlacementClip);

    private void PlayPlaceObject(AudioClip clip)
    {
        if(clip != null)
            audioSource.PlayOneShot(clip);
    }
}

