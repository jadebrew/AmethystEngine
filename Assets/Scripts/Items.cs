using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "AmethystObjects/Item", order = 2)]
public class Item : ScriptableObject
{
    public string itemName;
    public string description;

    public Resource resource;
    public int modifier;

    public string animationState;
    public string itemID;
}
