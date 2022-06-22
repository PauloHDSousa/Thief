using TMPro;
using UnityEngine;

public class MenuStatsManagerFiller : MonoBehaviour
{
    [Space(10)]
    [Header("UI Texts")]
    [SerializeField]
    TextMeshProUGUI tmpGoldStolen;

    [SerializeField]
    TextMeshProUGUI tmpsawByGuards;

    [SerializeField]
    TextMeshProUGUI tmptransformedInABox;

    [SerializeField]
    TextMeshProUGUI tmpItensStolen;

    void Start()
    {
        PlayerPrefsManager prefsManager = new PlayerPrefsManager();

        int gold = prefsManager.GetInt(PlayerPrefsManager.PrefKeys.GoldStolen);
        tmpGoldStolen.text = $"Amount of gold stolen: {gold}";

        int sawByGuards = prefsManager.GetInt(PlayerPrefsManager.PrefKeys.SawByGuards);
        tmpsawByGuards.text = $"Times seen by guards: {sawByGuards}";


        int transformedInABox = prefsManager.GetInt(PlayerPrefsManager.PrefKeys.TransformedInABox);
        tmpGoldStolen.text = $"Times transformed in a box: {transformedInABox}";
      
        int stolenItens = prefsManager.GetInt(PlayerPrefsManager.PrefKeys.ItensStolen);
        tmpItensStolen.text = $"Number of itens stolen: {stolenItens}";
    }
}
