using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class UIMenus : MonoBehaviour
{
    private GameObject m_player;
    private GameObject m_uiInput;
    private UIManager m_UIManager;
    public Slider m_slider;

    private int m_initialScene = 0;
    private int m_gameScene = 1;

    [HideInInspector] public bool isAMenuOrPanel = false;
    [HideInInspector] public bool isPaused = false;

    void Start()
    {
        m_UIManager = GetComponent<UIManager>();
        m_player = m_UIManager.player;
        m_uiInput = m_UIManager.uiInput;
        

        if (m_player)
        {
            m_player.SetActive(true);
            isAMenuOrPanel = false;
        }

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
        SceneManager.LoadScene(m_gameScene);
    }

    public void GoToInitialMenu()
    {
        m_uiInput.SetActive(false);
        m_UIManager.DesactivateAllUIGameObjects();
        m_UIManager.ActivateUIGameObjects(m_UIManager.initialMenu, true);
    }

    public void GoToCreditsPanel()
    {
        m_UIManager.DesactivateAllUIGameObjects();
        m_UIManager.ActivateUIGameObjects(m_UIManager.creditsPanel, true);
        Time.timeScale = 1;
    }
    public void PauseMenu()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            if (m_player)
            {
                isAMenuOrPanel = true;
            }

            m_UIManager.IsInGame(false);
            m_UIManager.DesactivateAllUIGameObjects();
            m_UIManager.ActivateUIGameObjects(m_UIManager.pauseMenu, true);
            isPaused = !isPaused;
        }
        else
        {
            if (m_player)
            {
                isAMenuOrPanel = false;
            }
            Resume();
        }
    }

    public void Resume()
    {
        m_UIManager.DesactivateAllUIGameObjects();
        m_UIManager.IsInGame(true);
    }

    public void GameModeDecoration()
    {
        GameManager.instance.ChangeGameMode(GameModes.Decoration);
        m_UIManager.DesactivateAllUIGameObjects();        
    }

    public void EndDay()
    {       
        GameManager.instance.ChangeDay();
        GameManager.instance.ChangeWeekNumber();

        //pantalla de carga

        GameManager.instance.SetCatsForTheDay();
        GameManager.instance.ChangeGameMode(GameModes.Decoration);
        GameManager.instance.ChangeDay();
        GameManager.instance.ChangeWeekNumber();
    }

    public void LoadingPanel(int gameSceneNum)
    {
        StartCoroutine(LoadAsync(gameSceneNum));
    }

    IEnumerator LoadAsync(int gameSceneNum)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(gameSceneNum);

        m_UIManager.ActivateUIGameObjects(m_UIManager.loadingPanel, true);

        while (!operation.isDone)
        {
            float progress = Mathf.Clamp01(operation.progress / 0.9f);
            Debug.Log(progress);
            m_slider.value = progress;
            yield return null;
        }
    }
}
