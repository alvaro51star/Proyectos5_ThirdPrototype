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
    private AudioClip undoClip, placeConstructionObjectClip, placeFurnitureClip, removeObjectClip, rotateClip;

    public void PlayUndoSound() => PlayPlaceobject(undoClip);

    public void PlayPlaceFurniture() => PlayPlaceobject(placeFurnitureClip);
    public void PlayPlaceConstruction() => PlayPlaceobject(placeConstructionObjectClip);
    public void PlayRemove() => PlayPlaceobject(removeObjectClip);
    public void PlayRotate() => PlayPlaceobject(rotateClip);

    private void PlayPlaceobject(AudioClip clip)
    {
        if(clip != null)
            audioSource.PlayOneShot(clip);
    }
}

