using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IBuildingState 
{
    void EndState();
    void OnAction(Vector3Int gridPosition);
    void UpdateState(Vector3Int gridPosition);
}
