using UnityEngine;

[CreateAssetMenu(fileName = "itemAttributes", menuName = "Item/New Item")]
public class ItemObject : ScriptableObject
{
    public float weight;
    public int priceValue;
}