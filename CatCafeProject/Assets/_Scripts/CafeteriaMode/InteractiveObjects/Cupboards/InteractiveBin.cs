using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBin : InteractiveObject
{
    [SerializeField] private AudioClip clip;
    [SerializeField] private AudioSource source;

    private FoodController playerFoodController;
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<FoodController>() != null && (other.GetComponent<FoodController>().foodType != FoodTypes.Nothing))
        {
            playerFoodController = other.GetComponent<FoodController>();

            base.OnTriggerEnter(other);
        }
    }
    protected override void Interaction()
    {
        playerFoodController.EnableFoodGO(false);
        playerFoodController.foodType = FoodTypes.Nothing;

        SoundManager.instance.ReproduceSound(clip,source);
    }
}
