using System.Collections;
using System.Collections.Generic;
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
        SceneManager.LoadScene(scene);
    }

}
