using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.AI.Navigation;
using UnityEngine;
using Random = UnityEngine.Random;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    [Header("Date Variables")]
    public int currentDay = 0;
    public int week = 0;

    [Space]
    [Header("Modes Variables")]
    [SerializeField] private GameObject player;
    [SerializeField] private Transform initialPlayerPosition;
    [SerializeField] private GameObject CafeteriaMode;
    [SerializeField] private GameObject DecorationMode;
    [SerializeField] private NavMeshSurface navMeshSurface;
    public GameModes initialGameMode;
    public GameModes currentGameMode;
    public bool isPaused = false;
    [SerializeField] private ClientDestroyer clientDestroyer;

    public static event Action<GameModes> OnGameModeChange;
    public static event Action<List<CatDataSO>, List<CatDataSO>> OnCatListCreated;
    public delegate void OnVariableChangeDelegate(float newVal);
    public static event OnVariableChangeDelegate OnTimeChange;


    [Space]
    [Header("Cats Variables")]
    public int maxCatsPerDay = 6;
    public List<CatDataSO> catDataList;
    public List<CatDataSO> catsForTheDay;
    [SerializeField] private CatListManager catListManager;

    [SerializeField] private float easyCatsPercentage = 0.6f;
    [SerializeField] private float normalCatsPercentage = 0.4f;
    [SerializeField] private float hardCatsPercentage = 0f;
    [SerializeField] private float richCatsPercentage = 0f;

    [Space]
    [Header("Time Variables")]
    public float maxTimeLevel;
    [SerializeField] private float baseLevelTime = 30f;
    [SerializeField] private float timePerCat = 5f;
    private float tempTime;
    [HideInInspector] public float currentTime;

    [Space]
    [Header("Feedback Variables")]
    public UIManager UIManager;

    [SerializeField] private AudioClip decorationAudioClip;
    [SerializeField] private AudioClip cafeteriaAudioClip;
    [SerializeField] private AudioSource audioSource;

    private bool isDone = false;

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

        ChangeDay();
        ChangeWeekNumber();
    }


    private void OnEnable()
    {
        InputManager.OnCancel += TogglePause;
    }

    private void OnDisable()
    {
        InputManager.OnCancel -= TogglePause;
    }

    void Start()
    {
        ChangeGameMode(initialGameMode);
        //m_maxTimeLevel = maxTimeLevel;
        // currentTime = maxTimeLevel;
        // tempTime = currentTime;
    }

    private void Update()
    {
        if (currentGameMode == GameModes.Cafeteria)
        {
            if (currentTime > 0)
            {
                currentTime -= Time.deltaTime;
            }

            if (currentTime <= 0 && !isDone)
            {
                currentTime = 0;
                isDone = true;
                EndDay();
                EconomyManager.instance.PrintReceipt();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
            }
            CheckTimerValueChange();
        }
    }

    private void CheckTimerValueChange()
    {
        if (tempTime != currentTime && OnTimeChange != null)
        {
            tempTime = currentTime;
            OnTimeChange(currentTime);
        }
    }

    public void ChangeGameMode(GameModes gameMode)
    {
        switch (gameMode)
        {
            case GameModes.Decoration:
                currentGameMode = GameModes.Decoration;
                ResetPlayerPosition();
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                DecorationMode?.SetActive(true);
                CafeteriaMode?.SetActive(false);
                catListManager.ResetCatList();
                SetCatsForTheDay();
                FurnitureManager.instance.ResetFurnitureManagerData();
                UIManager.instance.timerSlider.SetActive(false);
                audioSource?.Stop();
                audioSource.clip = decorationAudioClip;
                audioSource?.Play();
                break;

            case GameModes.Cafeteria:
                maxTimeLevel = baseLevelTime + timePerCat * catsForTheDay.Count;
                currentTime = maxTimeLevel;
                tempTime = currentTime;
                currentGameMode = GameModes.Cafeteria;
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                //FurnitureManager.instance.SetFurnitureData();
                isDone = false;
                currentTime = maxTimeLevel;
                DecorationMode?.SetActive(false);
                CafeteriaGameMode();
                UIManager.instance.timerSlider.SetActive(true);
                audioSource?.Stop();
                audioSource.clip = cafeteriaAudioClip;
                audioSource?.Play();
                break;

        }
        OnGameModeChange?.Invoke(gameMode);
    }

    private void CafeteriaGameMode()
    {
        if (!DecorationMode.activeSelf)
        {
            navMeshSurface.BuildNavMesh();
            CafeteriaMode?.SetActive(true);
            clientDestroyer.ResetClientDestroyed();
        }
    }

    public void SetCatsForTheDay()
    {
        catsForTheDay.Clear();
        int catsNumber;

        int easyCatNumber;
        int normalCatNumber;
        int hardCatNumber;
        int richCatNumber;
        if (currentDay > 1)
        {
            ChangeCatNumberPerDay();
        }

        if (IsSeventhDay())
        {

            ChangeCatPercentagePerWeek();

            catsNumber = Random.Range(maxCatsPerDay - 2, maxCatsPerDay + 1);
            easyCatNumber = Mathf.RoundToInt(catsNumber * 0.1f);
            normalCatNumber = Mathf.RoundToInt(catsNumber * 0.1f);
            hardCatNumber = Mathf.RoundToInt(catsNumber * 0.5f);
            richCatNumber = Mathf.RoundToInt(catsNumber * 0.3f);
        }
        else
        {
            catsNumber = Random.Range(maxCatsPerDay - 2, maxCatsPerDay + 1);
            easyCatNumber = Mathf.RoundToInt(catsNumber * easyCatsPercentage);
            normalCatNumber = Mathf.RoundToInt(catsNumber * normalCatsPercentage);
            hardCatNumber = 0;
            richCatNumber = 0;
        }

        for (int i = 0; i < easyCatNumber; i++)
        {
            catsForTheDay.Add(catDataList[Random.Range(0, 3)]);
        }
        for (int i = 0; i < normalCatNumber; i++)
        {
            catsForTheDay.Add(catDataList[Random.Range(3, 5)]);
        }
        for (int i = 0; i < hardCatNumber; i++)
        {
            catsForTheDay.Add(catDataList[Random.Range(5, 7)]);
        }
        for (int i = 0; i < richCatNumber; i++)
        {
            catsForTheDay.Add(catDataList[7]);
        }

        catsForTheDay = catsForTheDay.OrderBy(i => Guid.NewGuid()).ToList();
        OnCatListCreated?.Invoke(catsForTheDay, catDataList);
    }

    private bool IsSeventhDay()
    {
        if (currentDay % 7 == 0)
        {
            return true;
        }
        return false;
    }

    private void ChangeCatPercentagePerWeek()
    {
        if (week % 2 != 0 && week % 3 != 0)
            return;


        if (week % 2 == 0)
        {
            easyCatsPercentage -= 0.1f;
            normalCatsPercentage += 0.1f;
        }

        if (week % 3 == 0)
        {
            easyCatsPercentage += 0.05f;
            normalCatsPercentage -= 0.05f;
        }

        Math.Clamp(easyCatsPercentage, 0f, 1f);
        Math.Clamp(normalCatsPercentage, 0f, 1f);
    }

    private void ChangeCatNumberPerDay()
    {
        maxCatsPerDay += Random.Range(0, 3);
    }

    public void ChangeWeekNumber()
    {
        week = (currentDay / 7) + 1;
    }

    public void ChangeDay()
    {
        currentDay++;
    }


    public void EndDay()
    {
        catsForTheDay.Clear();
        UIManager.IsInGame(false);
        //UIManager.ActivateUIGameObjects(UIManager.nextDayMenu, true);
    }

    public void FinishBuildingPhase()
    {
        FurnitureManager.instance.SetFurnitureData();
        if (FurnitureManager.instance.tableNumber > 0)
        {
            ChangeGameMode(GameModes.Cafeteria);
            ClientManager.instance.SpawnCats();
            StartCoroutine(ClientManager.instance.TestCats());
        }
        else
        {
            StartCoroutine(UIManager.instance.ShowNoFurnitureText());
        }
    }

    public void TogglePause()
    {
        isPaused = !isPaused;

        if (isPaused)
        {
            Time.timeScale = 0;
        }
        else
        {
            Time.timeScale = 1;
        }

        UIManager.instance.SetPauseMenu(isPaused);
    }

    public void ResetPlayerPosition()
    {
        player.transform.position = initialPlayerPosition.position;
    }
}
