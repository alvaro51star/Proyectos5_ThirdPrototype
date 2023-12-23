using System;
using UnityEngine;

/// <summary>
/// Helps us make the code more efficient by not updating the 
/// position if it is the same as previously
/// </summary>
public class LastDetectedPositon
{
    public Vector3Int? lastPosition;

    public Vector3Int GetPosition()
    {
        if (lastPosition.HasValue)
            return lastPosition.Value;
        throw new Exception("LastDetectedPositon position was never Updated. " +
            "Check if you are calling TryUpdatingPositon on the instance of LastDetectedPosition scrip.");
    }
    public bool TryUpdatingPositon(Vector3Int tempPos)
    {
        if (lastPosition.HasValue && lastPosition == tempPos)
            return false;

        lastPosition = tempPos;
        return true;
    }

    public void Reset() => lastPosition = null;
}
