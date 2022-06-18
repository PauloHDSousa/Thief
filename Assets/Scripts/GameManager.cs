using System;
using TMPro;
using UnityEngine;
using UnityEngine.SceneManagement;

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


    AudioSource audioSource;
    string currentLevel;
    bool runSongIsPlaying = false;

    public int timesPLayerSeenByGuards = 0;
    private void Awake()
    {
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
        if (isPaused)
            return;

        currentTime = currentTime + Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        timerText.text = time.ToString(@"mm\:ss");
    }

    #region MENU/PAUSE
    public void ShowMenu()
    {
        TogglePause();

        audioSource.PlayOneShot(showMenuSFX);
        panelGameOver.SetActive(isPaused);
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
        SceneManager.LoadScene(currentLevel);
    }

    public void OnExit()
    {
        mapTimeText.text = "";
        timesSeenByGuardsText.text = "";
        itensStolenText.text = "";
        totalGoldStolenText.text = "";


        audioSource.PlayOneShot(showMenuSFX);
        panelExitMap.SetActive(true);
    }

    public void GoToNextLevel()
    {
        if (currentLevel == "Tutorial")
            currentLevel = "1-Map";

        SceneManager.LoadScene(currentLevel);
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
    public void AddWeightOnBar(float weight)
    {
        //if (weightBar.transform.localScale.x >= 1)
        //    return;

        //float nexWeight = weightBar.transform.localScale.x + weight;
        //LeanTween.scaleX(weightBar, nexWeight, .6f);
    }

    public void AddGoldValue(int gold)
    {
        string[] currentGoldValue = goldText.text.Split(" ");
        goldText.text = $"{int.Parse(currentGoldValue[0]) + gold} gold";

    }

    #endregion
}
