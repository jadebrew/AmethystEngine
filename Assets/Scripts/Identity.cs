using UnityEngine;

[CreateAssetMenu(fileName = "Identity", menuName = "AmethystObjects/Identity")]
public class Identity : ScriptableObject
{
    public string fullname;
    public string nickname;
    public string defaultMessage;
    public GameObject model;
}
