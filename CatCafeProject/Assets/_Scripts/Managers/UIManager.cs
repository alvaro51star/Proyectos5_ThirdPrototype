using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public static UIManager instance;

    public GameObject player;
    public GameObject uiInput;

    [SerializeField] private AudioClip buttonSound;
    [SerializeField] private AudioSource AudioSourceUI;

    [SerializeField] private GameObject[] MenusAndPanels;

    public GameObject initialMenu;
    public GameObject pauseMenu;
    public GameObject nextDayMenu;
    public GameObject blockMenu;
    public GameObject changeModeMenu;
    public GameObject loadingPanel;
    public GameObject creditsPanel;
    public GameObject timerSlider;

    [SerializeField] private GameObject noFurnitureText;
    private bool textVisible = false;

    private void Awake()
    {
        if (instance != null && instance != this)
        {
            Destroy(this);
            return;
        }
        else
        {
            instance = this;
        }
    }

    private void Start()
    {
        AudioSourceUI = GetComponent<AudioSource>();
    }

    public void ActivateUIGameObjects(GameObject gOToActivate, bool b)
    {
        gOToActivate.SetActive(b);
    }
    public void DesactivateAllUIGameObjects()
    {
        for (int i = 0; i < MenusAndPanels.Length; i++)
        {
            MenusAndPanels[i].SetActive(false);
        }
    }

    public void IsInGame(bool isInGame)
    {
        if (isInGame)
        {
            if (player != null)
            {
                player.SetActive(true);
            }
            Time.timeScale = 1;
            DesactivateAllUIGameObjects();
        }
        else
        {
            if (player != null)
            {
                player.SetActive(false);
            }

            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
            Time.timeScale = 0;
        }
    }

    public void SetPauseMenu(bool isPaused)
    {
        pauseMenu.SetActive(isPaused);
    }

    public void ButtonSound()
    {
        SoundManager.instance.ReproduceSound(buttonSound, AudioSourceUI);
    }

    public IEnumerator ShowNoFurnitureText()
    {
        
        if (!textVisible)
        {
            noFurnitureText.SetActive(true);
            textVisible = true;
            yield return new WaitForSeconds(1f);
            noFurnitureText.SetActive(false);
            textVisible = false;
        }

        yield return null;
    }
}
