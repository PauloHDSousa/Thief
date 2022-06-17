using UnityEngine;

public class StealableItem : MonoBehaviour
{

    [SerializeField]
    ItemObject itemObject;

    float _weight;
    int _priceValue;

    public float Weight { get { return _weight; } }
    public int PriceValue { get { return _priceValue; } }

    void Start()
    {
        _weight = itemObject.weight;
        _priceValue = itemObject.priceValue;
    }
}
