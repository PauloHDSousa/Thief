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
    GameObject weightBar;


    [SerializeField]
    GameObject panelGameOver;
    [Header("Songs")]
    [SerializeField]
    AudioSource emergencyAudioPlayer;

    [SerializeField]
    AudioClip mainSong;
    [SerializeField]
    AudioClip runSong;


    bool runSongIsPlaying = false;
    private void Awake()
    {
        if (Instance != null && Instance != this)
            Destroy(gameObject);
        else
            Instance = this;

        DontDestroyOnLoad(gameObject);
    }


    void Update()
    {
        currentTime = currentTime + Time.deltaTime;
        TimeSpan time = TimeSpan.FromSeconds(currentTime);
        timerText.text = time.ToString(@"mm\:ss\:ff");
    }

    #region MENU/PAUSE
    public void ShowMenu()
    {
        panelGameOver.SetActive(true);
    }

    public void ResetLevel()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
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
        if (weightBar.transform.localScale.x >= 1)
            return;

        float nexWeight = weightBar.transform.localScale.x + weight;
        LeanTween.scaleX(weightBar, nexWeight, .6f);
    }

    public void AddGoldValue(int gold)
    {
        string[] currentGoldValue = goldText.text.Split(" ");
        goldText.text = $"{int.Parse(currentGoldValue[0]) + gold} gold";

    }

    #endregion
}
