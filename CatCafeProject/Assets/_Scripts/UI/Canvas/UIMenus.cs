using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIMenus : MonoBehaviour
{
    [HideInInspector] public bool isPaused = false;

    private UIManager m_UIManager;
    private int m_initialScene = 0;
    private int m_gameScene = 1;

    void Start()
    {
        m_UIManager = GetComponent<UIManager>();

        /*
        if (m_player)
        {
            m_player.SetActive(true);
            isAMenuOrPanel = false;
        }
        */

        if (SceneManager.GetActiveScene().buildIndex == m_initialScene)
        {
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;

            m_UIManager.ActivateUIGameObjects(m_UIManager.initialMenu, true);
            m_UIManager.ActivateUIGameObjects(m_UIManager.creditsPanel, false);
        }
        else
        {
            m_UIManager.IsInGame(true);
            m_UIManager.ActivateUIGameObjects(m_UIManager.pauseMenu, false);
        }
    }

    public void QuitGame()
    {
        // Application.Quit();
        Debug.Log("Quit Game");
    }

    public void PlayGame()
    {
        // SceneManager.LoadScene(m_gameScene);
        Debug.Log("Play Game");
    }

    public void GoToInitialMenu()
    {
        m_UIManager.ActivateUIGameObjects(m_UIManager.initialMenu, true);
        m_UIManager.ActivateUIGameObjects(m_UIManager.creditsPanel, false);
        Debug.Log("Initial Active");
    }

    public void GoToCreditsPanel()
    {
        m_UIManager.ActivateUIGameObjects(m_UIManager.initialMenu, false);
        m_UIManager.ActivateUIGameObjects(m_UIManager.creditsPanel, true);   
        Debug.Log("Credits Panel Active");
    }
}
