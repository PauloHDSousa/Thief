using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    [Space(10)]
    [Header("Tutorial")]
    [SerializeField]
    GameObject tutorialButton;

    [Space(10)]
    [Header("Panels")]
    [SerializeField]
    GameObject mainMenuPanel;
    [SerializeField]
    GameObject optionsPanel;
    [SerializeField]
    GameObject statsPanel;

    [Space(10)]
    [Header("SFX")]
    [SerializeField]
    AudioClip onHoverSound;
    [SerializeField]
    AudioClip onClickSound;

    AudioSource audioSource;

    bool showOptions = false;
    bool showStats = false;
    PlayerPrefsManager prefsManager = new PlayerPrefsManager();

    void Start()
    {
        audioSource = GetComponent<AudioSource>();

        if (prefsManager.GetInt(PlayerPrefsManager.PrefKeys.PlayedTutorial) == 1 && tutorialButton != null)
            tutorialButton.SetActive(true);
    }

    public void Restart()
    {
        GameManager.Instance.LoadLevel(SceneManager.GetActiveScene().name);
    }

    public void GoToNextLevel()
    {
        GameManager.Instance.GoToNextLevel();
    }

    public void GoToMenu()
    {
        GameManager.Instance.GoToMenu();
    }

    void LoadScene(string scene)
    {
        Time.timeScale = 1f;
        audioSource.PlayOneShot(onClickSound);
        GameManager.Instance.LoadLevel(scene);
    }

    public void OnHoverSound()
    {
        audioSource.PlayOneShot(onHoverSound);
    }


    #region MainMenu
    public void Play()
    {
        if (prefsManager.GetInt(PlayerPrefsManager.PrefKeys.PlayedTutorial) == 1)
            LoadScene("1-Map");
        else
            LoadScene("Tutorial");
    }

    public void LoadTutorial()
    {
        LoadScene("Tutorial");
    }

    public void ToggleOptions()
    {
        showOptions = !showOptions;
        optionsPanel.SetActive(showOptions);
        mainMenuPanel.SetActive(!showOptions);
    }


    public void ToggleStats()
    {
        showStats = !showStats;
        statsPanel.SetActive(showStats);
        mainMenuPanel.SetActive(!showStats);
    }

    public void Exit()
    {
        Application.Quit();
    }
    #endregion
}
