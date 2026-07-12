using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;

public class PersistentData : MonoBehaviour
{

    public string last_music_id;
    public string last_doormat_id;
    public string last_briefing;
    public string last_cam_point_id;

    public Events last_event;

    public Color skyColor;

    public bool loadGame;

    public GameObject player;

    public string currentBrief = "Chapter 1\n\n"+

    "We were married for about five, six years.\n"+
    "Of course we weren't happy, but who was?\n"+

    "Economy took a hit after AmethystCorps bubble broke,\n"+
    "Jobs are scarce, homelessness is on the rise.\n"+
    "We were home more often than not, without any money.\n"+

    "I remember the day it all ended like it was yesterday...\n";


    public void Save()
    {
        string savePath = Path.Combine(Application.persistentDataPath, "save.json");
        JSONArray entityArray = new JSONArray();
        GameObject[] objs = GameObject.FindGameObjectsWithTag("Interactable");

        foreach (GameObject entityObject in objs)
        {
            if (entityObject.GetComponent<Entity>() != null)
                entityArray.Add(entityObject.GetComponent<Entity>().Save());
        }

        JSONObject saveData = new JSONObject();
        saveData["entities"] = entityArray;

        JSONObject jsonResources = new JSONObject();

        foreach (var pair in player.GetComponent<PlayerBehavior>().resourceLevels)
        {
            jsonResources[pair.Key] = pair.Value;
        }

        saveData["resources"] = jsonResources;

        string json = saveData.ToString(2);
        File.WriteAllText(savePath, saveData.ToString(2));
    }

    public void Load()
    {
        GameObject[] plyrs = GameObject.FindGameObjectsWithTag("Player");
        player = plyrs[0];

        string savePath = Path.Combine(Application.persistentDataPath, "save.json");
        if (!File.Exists(savePath))
        {
            Debug.Log("No save file found.");
            return;
        }

        string jsonString = File.ReadAllText(savePath);


        JSONNode saveData = JSON.Parse(jsonString);

        GameObject[] entityObjects = GameObject.FindGameObjectsWithTag("Interactable");


        foreach (JSONNode entityData in saveData["entities"].AsArray)
        {
            string id = entityData["id"];

            foreach (GameObject entity in entityObjects)
            {
                if (entity.GetComponent<Entity>() != null && entity.GetComponent<Entity>().id == id)
                {
                    entity.GetComponent<Entity>().Load(entityData);
                    Debug.Log("Loaded from Json: " + entity.GetComponent<Entity>().id);
                    break;
                }
            }
        }

        // foreach (string key in player.GetComponent<PlayerBehavior>().resourceLevels.Keys)
        // {
        //     if (saveData["resources"].HasKey(key))
        //     {
        //         player.GetComponent<PlayerBehavior>().resourceLevels[key] = saveData["resources"][key].AsInt;
        //         Debug.Log("Loaded from Json: " + key + " as " + player.GetComponent<PlayerBehavior>().resourceLevels[key]);
        //     }
        // }
    }
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        GameObject[] objs = GameObject.FindGameObjectsWithTag("PersistentData");

        if (objs.Length > 1)
        {
            Destroy(this.gameObject);
        }

        DontDestroyOnLoad(this.gameObject);

    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
