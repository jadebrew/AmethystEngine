using UnityEngine;

[CreateAssetMenu(fileName = "Item", menuName = "AmethystObjects/Item", order = 2)]
public class Item : ScriptableObject
{
    public string itemName;
    public string description;

    public Resource resource;
    public int modifier;

    public bool requiresFocus;

    public string animationState;
    public string itemID;

    public GameObject model;
}
