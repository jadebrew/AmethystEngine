using UnityEngine;

[CreateAssetMenu(fileName = "Events", menuName = "AmethystObjects/Events")]
public class Events : ScriptableObject
{
    public bool teleport;
    public bool toggleActive;
    public bool doCamMove;
    public string target_id;
    public string goal_id;
    public string postCamPoint_id;
    public bool changeMusic;
    public string nextMusic;
}
