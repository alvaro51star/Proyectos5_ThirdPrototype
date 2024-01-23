using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuInput : MonoBehaviour
{
    [SerializeField] UIMenus uIMenus;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            if (uIMenus.isAMenuOrPanel == false)
                uIMenus.PauseMenu();
        }
    }
}
