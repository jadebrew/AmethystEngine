using UnityEngine;
using SimpleJSON;

public class Entity : MonoBehaviour
{
    public Identity identity;
    public string id;
    public double spawnTime;
    public Quest[] quests;
    public int currentQuest;
    public Item[] inventory;
    public bool done;
    public bool met = false;

<<<<<<< Updated upstream
    public void Update()
=======
    public bool forceView;
    public GameObject CameraPoint;
    public JSONObject entityJson;


    void Start()
>>>>>>> Stashed changes
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Interactable");
        var pos = transform.position;
        if (spawnTime == 0)
        {
            id = this.name + "_" + Mathf.Round(pos.x) + Mathf.Round(pos.y) + Mathf.Round(pos.z);
            spawnTime = Time.realtimeSinceStartupAsDouble;
        }
        entityJson = new JSONObject();

        foreach (GameObject GO in objs)
        {
            Entity lookalike = GO.GetComponent<Entity>();
            if (lookalike != null && lookalike.id == this.id && lookalike.spawnTime < this.spawnTime)
            {
                Destroy(this.gameObject);
            }

        }
        DontDestroyOnLoad(this.gameObject);
    }

    public JSONObject Save()
    {
        JSONObject json = new JSONObject();

        json["id"] = id;
        json["currentQuest"] = currentQuest;

        JSONObject pos = new JSONObject();
        pos["x"] = transform.position.x;
        pos["y"] = transform.position.y;
        pos["z"] = transform.position.z;

        json["position"] = pos;

        return json;
    }

    public void Load(JSONNode json)
    {
        id = json["id"];
        currentQuest = json["currentQuest"].AsInt;

        Vector3 pos = new Vector3(
            json["position"]["x"].AsFloat,
            json["position"]["y"].AsFloat,
            json["position"]["z"].AsFloat
        );

        transform.position = pos;
    }
}
