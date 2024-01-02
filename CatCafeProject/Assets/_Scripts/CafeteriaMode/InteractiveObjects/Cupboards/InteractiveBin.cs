using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractiveBin : InteractiveObject
{
    private PlayerFoodController playerFoodController;
    protected override void OnTriggerEnter(Collider other)
    {
        if (other.GetComponent<PlayerFoodController>() != null)
        {
            playerFoodController = other.GetComponent<PlayerFoodController>();
        }
        base.OnTriggerEnter(other);
    }
    protected override void Interaction()
    {
        playerFoodController.EnableFoodGO(false);
    }
}
