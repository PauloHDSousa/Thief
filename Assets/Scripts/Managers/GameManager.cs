using System;
using System.Collections;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{

    public static GameManager Instance { get; private set; }

    public float currentTime;

    [Space(10)]
    [Header("UI")]
    [SerializeField]
    TextMeshProUGUI timerText;
    [SerializeField]
    TextMeshProUGUI goldText;

    [SerializeField]
    GameObject panelExitMap;

    [SerializeField]
    GameObject panelGameOver;

    [Header("Songs")]
    [SerializeField]
    AudioSource emergencyAudioPlayer;



    [SerializeField]
    AudioClip showMenuSFX;

    [SerializeField]
    AudioClip mainSong;
    [SerializeField]
    AudioClip runSong;
    [SerializeField]
    AudioClip finishedMapSong;


    [Space(10)]
    [Header("UI Texts")]
    [SerializeField]
    TextMeshProUGUI mapTimeText;
    [SerializeField]
    TextMeshProUGUI timesSeenByGuardsText;
    [SerializeField]
    TextMeshProUGUI itensStolenText;
    [SerializeField]
    TextMeshProUGUI totalGoldStolenText;


    [Space(10)]
    [Header("Loading")]
    [SerializeField]
    GameObject loadingPanel;
    [SerializeField]
    Slider loadingSlider;


    AudioSource audioSource;
    string currentLevel;
    bool runSongIsPlaying = false;

    public int timesPLayerSeenByGuards = 0;
    public int itensStolen = 0;

    private void Awake()
    {
        Time.timeScale = 1f;
        Instance = this;
    }

    public bool isPaused = false;
    private void Start()
    {
        audioSource = GetComponent<AudioSource>();
        currentLevel = SceneManager.GetActiveScene().name;
    }

    void Update()
    {
        if (isPaused || timerText == null)
            return;

        currentTime = currentTime + Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        timerText.text = time.ToString(@"mm\:ss");
    }

    #region MENU/PAUSE
    public void ShowMenuOnPause()
    {
        TogglePause();
        audioSource.PlayOneShot(showMenuSFX);
        panelGameOver.SetActive(isPaused);
    }

    public void ShowMenuAfterDeath()
    {
        panelGameOver.SetActive(true);
    }

    void TogglePause()
    {
        isPaused = !isPaused;
        if (isPaused)
            Time.timeScale = 0f;
        else
            Time.timeScale = 1f;
    }

    public void ResetLevel()
    {
        LoadLevel(currentLevel);
    }

    public void OnExit()
    {
        PlayerPrefsManager prefsManager = new PlayerPrefsManager();
        
        if (currentLevel == "Tutorial")
            prefsManager.SaveInt(PlayerPrefsManager.PrefKeys.PlayedTutorial, 1);

        TogglePause();
        StopRunSong();
        audioSource.Stop();
        audioSource.PlayOneShot(finishedMapSong);
        //UI
        mapTimeText.text = $"Map Time  {timerText.text}";
        timesSeenByGuardsText.text = $"times seen by guards: {timesPLayerSeenByGuards}";
        itensStolenText.text = $"itens stolen: {itensStolen}";

        string goldStolen = goldText.text.Split(" ")[0];

        totalGoldStolenText.text = $"Total Gold stolen: {goldStolen}";

        prefsManager.IncrementInt(PlayerPrefsManager.PrefKeys.GoldStolen, int.Parse(goldStolen));
        prefsManager.IncrementInt(PlayerPrefsManager.PrefKeys.SawByGuards, timesPLayerSeenByGuards);
        prefsManager.IncrementInt(PlayerPrefsManager.PrefKeys.ItensStolen, itensStolen);


        audioSource.PlayOneShot(showMenuSFX);
        panelExitMap.SetActive(true);
    }

    public void GoToMenu()
    {
        Time.timeScale = 1f;
        LoadLevel("Menu");
    }
    public void GoToNextLevel()
    {
        if (currentLevel == "Tutorial")
            currentLevel = "1-Map";
        else
        {
            string[] currentLevelSplited = currentLevel.Split('-');
            int levelNumber = int.Parse(currentLevelSplited[0]);
            currentLevel = $"{levelNumber + 1}-{currentLevelSplited[1]}";
        }

        LoadLevel(currentLevel);
    }

    public void LoadLevel(string scene)
    {
      StartCoroutine(LoadAsync(scene));
    }

    IEnumerator LoadAsync(string scene)
    {
        AsyncOperation operation = SceneManager.LoadSceneAsync(scene);

        loadingPanel.SetActive(true);
        while (!operation.isDone)
        {

            float progress = Mathf.Clamp01(operation.progress / .9f);
            loadingSlider.value = progress;
            
            yield return null;
        }
    }
    #endregion

    #region Map Mood
    public void PlayRunSong()
    {
        if (!runSongIsPlaying)
        {
            emergencyAudioPlayer.PlayOneShot(runSong);
            runSongIsPlaying = true;
        }
    }

    public void StopRunSong()
    {
        emergencyAudioPlayer.Stop();
        runSongIsPlaying = false;
    }
    #endregion

    #region UI
    public void AddGoldValue(int gold)
    {
        string[] currentGoldValue = goldText.text.Split(" ");
        goldText.text = $"{int.Parse(currentGoldValue[0]) + gold} gold";

    }

    #endregion

}

