using UnityEngine;

[CreateAssetMenu(fileName = "Events", menuName = "AmethystObjects/Events")]
public class Events : ScriptableObject
{
    public bool teleport;
    public bool toggleActive;
    public bool doCamMove;

    public bool camFollow;

    public string target_id;
    public string goal_id;
    public string postCamPoint_id;
<<<<<<< Updated upstream
=======
    public bool changeMusic;
    public string nextMusic;

    public Material skybox;

    public bool changeLight;
    public Color color;

    public bool changeScene;
    public string sceneTitle;

    public bool loadScreen;
    public string loadScreenText;

    public bool setMania;
    public Resource maniaResource;
>>>>>>> Stashed changes
}
