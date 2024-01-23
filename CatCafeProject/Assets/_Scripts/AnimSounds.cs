using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AnimSounds : MonoBehaviour
{
    [SerializeField] private AudioClip chipiChapaSound;
    [SerializeField] private AudioSource audioSource;

    private void ChipiChapaSound()
    {
        SoundManager.instance.ReproduceSound(chipiChapaSound, audioSource);
    }
}
