using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DebugManager : MonoBehaviour
{
    [SerializeField] private bool enableLogs = true;

    private void Awake()
    {
        Debug.unityLogger.logEnabled = enableLogs;
    }

    
}
