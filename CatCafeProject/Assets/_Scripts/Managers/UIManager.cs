using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public GameObject player;

    [SerializeField]
    private AudioSource audioSource;

    [SerializeField] private GameObject[] MenusAndPanels;

    public GameObject initialMenu;
    public GameObject pauseMenu;
    public GameObject creditsPanel;

    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
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

            Cursor.lockState = CursorLockMode.Locked;
            Cursor.visible = false;
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
}
