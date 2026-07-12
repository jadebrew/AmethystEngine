using UnityEngine;

[CreateAssetMenu(fileName = "Quest", menuName = "AmethystObjects/Quest", order = 3)]
public class Quest : ScriptableObject
{
    public string description;
    public string question;
    public string confirmation;
    public Resource resource;
    public int required;
    public string clearedMessage;
    public bool known;

    public bool doEvent;
    public Events eventRef;
}
