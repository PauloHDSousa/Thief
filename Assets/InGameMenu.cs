using UnityEngine;
using UnityEngine.SceneManagement;

public class InGameMenu : MonoBehaviour
{
    [SerializeField]
    AudioClip onHoverSound;
    [SerializeField]
    AudioClip onClickSound;

    AudioSource audioSource;
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }


    public void GoToNextLevel()
    {
        GameManager.Instance.GoToNextLevel();
    }

    public void OnHoverSound()
    {
        audioSource.PlayOneShot(onHoverSound);
    }


    public void Restart()
    {
        LoadScene(SceneManager.GetActiveScene().name);
    }
    void LoadScene(string scene)
    {
        Time.timeScale = 1f;
        audioSource.PlayOneShot(onClickSound);
        SceneManager.LoadScene(scene);
    }


    #region MainMenu
    public void Play()
    {
        LoadScene("Tutorial");
    }
    #endregion
}
