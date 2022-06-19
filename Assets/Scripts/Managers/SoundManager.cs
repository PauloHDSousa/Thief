using UnityEngine;
using UnityEngine.UI;
public class SoundManager : MonoBehaviour
{

    [SerializeField]
    Slider volumeSlider;


    PlayerPrefsManager prefsManager = new PlayerPrefsManager();
    void Start()
    {
        float volumeValue = 1;

        if (!prefsManager.HasKey(PlayerPrefsManager.PrefKeys.Volume))
            prefsManager.SaveFloat(PlayerPrefsManager.PrefKeys.Volume, volumeValue);
        
        volumeValue = prefsManager.GetFloat(PlayerPrefsManager.PrefKeys.Volume);

        volumeSlider.value = volumeValue;
    }

    public void ChangeVolume()
    {
        float volumeValue = volumeSlider.value;
        prefsManager.SaveFloat(PlayerPrefsManager.PrefKeys.Volume, volumeValue);
        AudioListener.volume = volumeValue;
    }
}
