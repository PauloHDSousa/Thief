using UnityEngine;

public class PlayerPrefsManager
{
    //I'm not sure if this class was necessary, BUT I did it anyway.
    #region Keys

    public enum PrefKeys
    {
        Volume,
        PlayedTutorial,
        GoldStolen,
        SawByGuards,
        TransformedInABox,
        ItensStolen
    }
    #endregion

    #region Set
    public void SaveFloat(PrefKeys key, float value)
    {
        PlayerPrefs.SetFloat(key.ToString(), value);
    }

    public void SaveInt(PrefKeys key, int value)
    {
        PlayerPrefs.SetInt(key.ToString(), value);
    }

    public void Save(PrefKeys key, string value)
    {
        PlayerPrefs.SetString(key.ToString(), value);
    }
    #endregion

    #region Get

    public bool HasKey(PrefKeys key)
    {
        return PlayerPrefs.HasKey(key.ToString());
    }

    public float GetFloat(PrefKeys key)
    {
        return PlayerPrefs.GetFloat(key.ToString());
    }

    public int GetInt(PrefKeys key)
    {
        return PlayerPrefs.GetInt(key.ToString());
    }

    public string Get(PrefKeys key)
    {
        return PlayerPrefs.GetString(key.ToString());
    }
    #endregion


    #region Increment
    public int IncrementInt(PrefKeys key, int value)
    {
        int currentValue = GetInt(key);
        int newValue = currentValue + value;

        PlayerPrefs.SetInt(key.ToString(), newValue);

        return newValue;
    }

    #endregion
}
