using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;

public class PlayerBehavior : MonoBehaviour
{
    public float moveSpeed = 1f;
    public float turnSpeed = 90f;
    private Animator animator;
    private Rigidbody rb;

    private PlayerInput playerInput;
    private InputAction moveAction;
    private InputAction interactAction;
    private InputAction useAction;
    private InputAction debugAction;

    private bool busy = false;
    private bool inDialogue = false;
    private bool inMonologue = false;
    private bool canFocus = true;

    private Entity currentEntity;
    private CameraTrigger currentCamera;
    private Camera mainCamera;
    private float gametimer;
    private int gametickrate = 1;

    private string savePath;

    public int DebugModifier = 4;
    public bool debugMode = false;

    public Identity identity;

    public Item heldItem;
    public GameObject heldItem3d;

<<<<<<< Updated upstream
=======
    private JSONObject playerJson;


>>>>>>> Stashed changes
    private string feelings = "Fine";
    //private string complaintDescription = "I feel fine.";
    public Resource resourceIssueDefault;
    private Resource resourceIssue;

    PersistentData persistentData;

    public Resource[] resources;

    public GameObject consoleReference;
<<<<<<< Updated upstream

    private Dictionary<string,int> resourceLevels;
     private Dictionary<string,int> resourceTresholds;

=======
    private GameObject terminalRef;
    public Dictionary<string,int> resourceLevels;
     public Dictionary<string,int> resourceTresholds;

     public int gameState = 1;
        //0 = debug
        //1 = game
        //2 = paused

     public AudioSource bgmDronedAudioSource;
     public AudioSource bgmHappyAudioSource;
     public AudioSource bgmAngelsAudioSource;
     public AudioSource sfxSelectAudioSource;
     public AudioSource confirmAudioSource;
     public AudioSource denyAudioSource;

     public GameObject[] entities;

     public bool mania;
     public Resource maniaResource;

    void Awake()
    {
        persistentData = GameObject.Find("PersistentData").GetComponent<PersistentData>();
        persistentData.player = this.gameObject;
        if(persistentData.last_doormat_id.Length > 2)
        {
            GameObject goal = GameObject.Find(persistentData.last_doormat_id);
            this.transform.position = goal.transform.position;
        }
        if(persistentData.last_cam_point_id.Length > 2)
        {
            GameObject goal = GameObject.Find(persistentData.last_cam_point_id);
            mainCamera = Camera.main;
            mainCamera.transform.position = goal.transform.position;
        }

        if (persistentData.loadGame)
        {
            persistentData.Load();
            if(persistentData.last_event)
                doEvent(persistentData.last_event, true);



            gameState = 1;
        }
    }

>>>>>>> Stashed changes
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

        rb = GetComponent<Rigidbody>();
        playerInput = GetComponent<PlayerInput>();
        animator = GetComponent<Animator>();
        moveAction = playerInput.actions["Move"];
        interactAction = playerInput.actions["Interact"];
        debugAction = playerInput.actions["Debug"];
        useAction = playerInput.actions["Use"];
        resourceLevels = new Dictionary<string,int>();
        resourceTresholds = new Dictionary<string,int>();
        resourceIssue = resourceIssueDefault;
        mainCamera = Camera.main;

        initItem();
        gameState = 1;


        foreach (Resource res in resources)
        {
            resourceLevels.Add(res.title,0);
            resourceTresholds.Add(res.title,0);
            Debug.Log("Initialized " + res.title + " as " + res.value);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (!inDialogue && other.TryGetComponent(out Entity entity))
            currentEntity = entity;

        if (!inDialogue && other.TryGetComponent(out CameraTrigger CameraPoint))
        {
<<<<<<< Updated upstream
            currentCamera = CameraPoint;
            mainCamera = Camera.main;
            mainCamera.transform.SetPositionAndRotation(currentCamera.GetObject().transform.position, currentCamera.GetObject().transform.rotation);
=======
            currentCamera = CameraPoint.GetObject();
>>>>>>> Stashed changes
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (!inDialogue && !busy && other.TryGetComponent(out Entity entity))
        {
            if (currentEntity == entity)
                currentEntity = null;
        }
    }
    void startDialogue(Entity entity)
    {
        inDialogue = true;
        entity.done = false;
        List<string> menu = new List<string>();
        string menutype = "Say";
        animator.SetBool("talking",true);
        if(entity.inventory.Length >0)
        {
            foreach (Item item in entity.inventory)
            {
                menu.Add(item.itemName);
            }
            menutype = "Take";
        }
        else if(entity.quests.Length>0)
        {
            menu.Add(heldItem.itemName + "?");
            menu.Add(entity.quests[entity.currentQuest].question);
            menu.Add(feelings);
            menu.Add("Bye");
            menutype = "Say";
        }
<<<<<<< Updated upstream
=======

        if (entity.CameraPoint != null)
        {
            previousCamera = currentCamera;
            currentCamera = entity.CameraPoint;
            if (entity.forceView)
            {
                mainCamera.transform.SetPositionAndRotation(currentCamera.transform.position, currentCamera.transform.rotation);
                mainCamera.GetComponent<lookat>().follow = false;
            }
        }
>>>>>>> Stashed changes
        string[] textmenu = menu.ToArray();

        DialogueUI.Instance.SetMenu(menutype,textmenu);
        DialogueUI.Instance.selected = 0;
        DialogueUI.Instance.Show(entity.identity.nickname,entity.identity.defaultMessage);
    }

    void startMonologue(string text)
    {
        inDialogue = true;
        inMonologue = true;
        List<string> menu = new List<string>();
        string menutype = "Menu";
        menu.Add(heldItem.itemName);
        menu.Add("Near");
        menu.Add("Feeling");
        menu.Add("Cancel");
        string[] textmenu = menu.ToArray();

        DialogueUI.Instance.SetMenu(menutype,textmenu);
        DialogueUI.Instance.selected = 0;
        DialogueUI.Instance.Show(identity.nickname,text);
    }

    void startBusy(Item item)
    {
        //behavior when starting your item activity
        inDialogue = true;
        busy = true;
        List<string> menu = new List<string>();
        string menutype = item.itemName;
        menu.Add("Stop");
        string[] textmenu = menu.ToArray();


        animator.SetBool(heldItem.animationState,true);

        gametimer = 0;

        DialogueUI.Instance.SetMenu(menutype,textmenu);
        DialogueUI.Instance.selected = 0;
        DialogueUI.Instance.Show(identity.nickname,item.resource.complaint + "\n" + item.description);
        updateBusy(item,false);
    }

    void updateBusy(Item item,bool gametick)
    {
        //behavior when using your item activity
        //initialize the local variables
        var resource = item.resource;
        int goal = resource.max;
        int totalThoughts = resource.thoughtProcess.Length;

        if (item.requiresFocus && !canFocus)
        {
            endBusy();
            endDialogue();
            startMonologue("I can't focus on " + item.resource.title + " because " + feelings);
            return;
        }

        //see if there are any tresholds for the resource?
        //(need questgiver to advance past treshold)
        if (resource.tresholds.Length>0)
        {
            var currentTreshold = resourceTresholds[resource.title];

            //if all tresholds are met, just continue.
            //this means all quests for this resource have been done
            if (currentTreshold<resource.tresholds.Length) {
                goal = resource.tresholds[currentTreshold].required;
            }
        }

        //increase the resource by the modifier, this means a modifier has to be always positive -> hunger is not a good resource bc eating shouldnt increase hunger, but eating does increase saturation, while a modifier can be negative, the goal is always the max value, so this can be changed later if wanted, but right now not a priority.
        if (gametick){
            int modifier = 1;
            if (debugMode)
                modifier = DebugModifier;
            resourceLevels[resource.title] += item.modifier * modifier;
        }

        //if you reached the max (which is the initial goal) set the resource to goal
        //this means the goal is either the current treshold, or the max value for the resource
        if (resourceLevels[resource.title] > goal)
        {
            resourceLevels[resource.title] = goal;
        }

        //fuckywucky code bc i dont remember how fractions work and i dropped out of kindergarten
        float percentage = 100/goal*resourceLevels[resource.title];
        int currentThought = Mathf.FloorToInt((totalThoughts-1)*(percentage/100));

        //see if the immersive output is possible, if the goal has reached, use the last immersive output, and if debug is enabled, add debug output
         string debugOutput = "goal: " + goal + ", currently: " + percentage + "% - " +  resourceLevels[resource.title];
        string output = "";
        bool complete = false;
        if ( currentThought >=0 && currentThought < resource.thoughtProcess.Length)
            output = resource.thoughtProcess[currentThought];
        if (goal == resourceLevels[resource.title])
            {
                output = resource.thoughtProcess[resource.thoughtProcess.Length-1];
                complete = true;
            }

        if (debugMode)
            output += "\n" + debugOutput;

        float dots = Time.realtimeSinceStartup % 4;
        string strDots = "";
        if (!complete)
        {
            for (float i = 1; i < dots; i++)
            {
                strDots += ".";
            }
        }
        DialogueUI.Instance.SetDialogueTitle(item.resource.title + strDots);
        DialogueUI.Instance.SetText(output);
    }

    void endBusy()
    {
        //behavior when ending your item activity
        busy = false;
        endDialogue();
        animator.SetBool(heldItem.animationState,false);
    }

    void doEvent(Events currentEvent, bool denyLoadScreen)
    {
        GameObject target = GameObject.Find(currentEvent.target_id);
        GameObject goal = GameObject.Find(currentEvent.goal_id);
        GameObject camPoint = GameObject.Find(currentEvent.postCamPoint_id);

        mainCamera.GetComponent<lookat>().moveTowards = currentEvent.camFollow;


        if (currentEvent.teleport)
        {

            target.transform.SetPositionAndRotation(goal.transform.position, goal.transform.rotation);
            if (target.tag == "Player")
                persistentData.last_doormat_id = currentEvent.goal_id;
            persistentData.Save();
        }
            else if (currentEvent.toggleActive)
        {
            target.SetActive(false);
        }

        if (currentEvent.doCamMove)
        {
            mainCamera.transform.position = camPoint.transform.position;
            persistentData.last_cam_point_id = currentEvent.postCamPoint_id;
            endDialogue();
        }
<<<<<<< Updated upstream
=======
        if (currentEvent.changeMusic)
        {
            if (currentEvent.nextMusic == "drone")
            {
                bgmHappyAudioSource.Stop();
                bgmDronedAudioSource.Play();
                bgmAngelsAudioSource.Stop();
                persistentData.last_music_id = "drone";
            }
            if (currentEvent.nextMusic == "angels")
            {
                bgmHappyAudioSource.Stop();
                bgmDronedAudioSource.Stop();
                bgmAngelsAudioSource.Play();
                persistentData.last_music_id = "angels";
            }
        }
        if (currentEvent.skybox != null)
        {

            RenderSettings.skybox=currentEvent.skybox;
        }

        if (currentEvent.changeLight)
        {
            GameObject[] objs = GameObject.FindGameObjectsWithTag("ColorableLight");
            foreach (GameObject lightGo in objs)
            {
                lightGo.GetComponent<Light>().color = currentEvent.color;
            }
            RenderSettings.fogColor = currentEvent.color;
        }
        if (currentEvent.changeScene)
        {
           SceneManager.LoadScene(currentEvent.sceneTitle, LoadSceneMode.Single);
        }

        if (currentEvent.setMania)
        {
            mania = true;
            maniaResource = currentEvent.maniaResource;
        }

        if (currentEvent.loadScreen)
        {
            endDialogue();
            persistentData.loadGame = true;
            mainCamera.GetComponent<lookat>().follow = true;
            mainCamera.GetComponent<lookat>().moveTowards = true;
            persistentData.currentBrief = currentEvent.loadScreenText;
            persistentData.Save();
            SceneManager.LoadScene("LoadingScene", LoadSceneMode.Single);
            gameState = 1;
        }

>>>>>>> Stashed changes
    }

    void updateDialogue(Entity entity, int topic)
    {
<<<<<<< Updated upstream
        if (topic == 1)
=======
        var quest = entity.quests[entity.currentQuest];
        if (topic == 2)
>>>>>>> Stashed changes
        {
            //topic is your held item

            //DialogueUI.Instance.SetText("Oh wow! Look at your " + heldItem.itemName + "!\nMaybe if you didn't care so much about " + heldItem.resource.title + ", you'd have a job by now.");
            if (quest != null)
            {
                if (mania)
                {
                     DialogueUI.Instance.SetText("Whatever helps you.");
                }
                else if (quest.known && heldItem.resource == quest.resource)
                {

                    // the item is right but your resource isnt high enough
<<<<<<< Updated upstream
                    DialogueUI.Instance.SetText(heldItem.itemName + " works!\nWork on your " + entity.quests[entity.currentQuest].resource.title + " and come back!");
=======
                    DialogueUI.Instance.SetText(heldItem.itemName + " works!\nWork on your " + quest.resource.title + " and come back!");
                    denyAudioSource.Play();
>>>>>>> Stashed changes

                } else {
                    // the item is wrong
                    DialogueUI.Instance.SetText("Oh, " + heldItem.itemName + ". Need some " + heldItem.resource.title + "?");
                }
            }
        } else if (topic == 3)
        {
            //complain about your feelings TODO
<<<<<<< Updated upstream
            DialogueUI.Instance.SetText("Make sure you work on your " + resourceIssue.title);
        } else if (topic == 2)
=======
            if (!mania)
            {
                DialogueUI.Instance.SetText("Make sure you work on your " + resourceIssue.title);
            } else {
                DialogueUI.Instance.SetText("You scare me.");
            }

            confirmAudioSource.Play();
        } else if (topic == 1)
>>>>>>> Stashed changes
        {

            if (quest != null)
            {
<<<<<<< Updated upstream
                if ( resourceLevels[entity.quests[entity.currentQuest].resource.title] >= entity.quests[entity.currentQuest].required)
                {
                    //the item is right, you are aware of the quest, and you have enough resource
                    DialogueUI.Instance.SetText(entity.quests[entity.currentQuest].clearedMessage);
                    resourceTresholds[entity.quests[entity.currentQuest].resource.title]++;
                    if (entity.quests[entity.currentQuest].doEvent)
                        doEvent(entity);

                    if (entity.quests.Length-1 > entity.currentQuest)
                        entity.currentQuest++;
                } else {
                    DialogueUI.Instance.SetText(entity.quests[entity.currentQuest].description);
                    entity.quests[entity.currentQuest].known = true;
=======

                if (!entity.met)
                {
                    entity.met = true;
                    quest.known = false;
                }
                if (quest.known && resourceLevels[quest.resource.title] >= quest.required)
                {
                    //the item is right, you are aware of the quest, and you have enough resource
                    DialogueUI.Instance.SetText(quest.clearedMessage);
                    resourceTresholds[quest.resource.title]++;

                    if (quest.rewardResource != null)
                    {
                        resourceLevels[quest.rewardResource.title] += quest.rewardAmount;
                        Debug.Log(quest.rewardResource.title + ": " + resourceLevels[quest.rewardResource.title]);

                    }
                    if (entity.quests.Length-1 > entity.currentQuest)
                    {
                        entity.currentQuest++;
                        PlayerPrefs.SetInt(entity.id, entity.currentQuest);
                        quest.known = false;
                        confirmAudioSource.Play();
                    }
                    if (quest.doEvent)
                    {
                        confirmAudioSource.Play();
                        doEvent(quest.eventRef, false);
                    }
                } else {
                    DialogueUI.Instance.SetText(quest.description);
                    quest.known = true;
                    confirmAudioSource.Play();
>>>>>>> Stashed changes
                }
            }
        } else if (topic == 4)
        {
            DialogueUI.Instance.SetText("Fine! Bye!");
            entity.done = true;
        }
    }


    void endDialogue()
    {
<<<<<<< Updated upstream
=======
        if (previousCamera != null)
        {
            currentCamera = previousCamera;
            mainCamera.GetComponent<lookat>().follow = true;
            previousCamera = null;
        }
>>>>>>> Stashed changes
        inDialogue = false;
        inMonologue = false;
        DialogueUI.Instance.Hide();
        animator.SetBool("talking",false);
<<<<<<< Updated upstream
=======
        foreach (Transform child in viewing.transform) {
            GameObject.Destroy(child.gameObject);
        }
        if (currentEntity != null && currentEntity.forceView)
        {
            mainCamera.transform.SetPositionAndRotation(currentCamera.transform.position, currentCamera.transform.rotation);
        }
>>>>>>> Stashed changes
    }

    void initItem()
    {
        GameObject.Find(heldItem.itemID).GetComponent<MeshRenderer>().enabled = true;
    }

    void LoadItem(Item item)
    {
        GameObject.Find(heldItem.itemID).GetComponent<MeshRenderer>().enabled = false;
        heldItem = item;
        initItem();
    }

    // Update is called once per frame
    void FixedUpdate()
    {
        mainCamera = Camera.main;
        if (gameState == 1)
            UpdateGame();

        if (viewing == null)
            viewing = GameObject.Find("Viewing");

        if (startAction.WasPressedThisFrame())
        {
            if (gameState == 2)
            {
                gameState = 1;
                animator.SetBool(heldItem.animationState,false);
            }
            else if (gameState == 1)
            {
                gameState = 2;
                animator.SetBool(heldItem.animationState,true);
            }
        }
    }


    void UpdateGame()
    {
        Vector2 move = moveAction.ReadValue<Vector2>();
        bool movingForward = move.y > 0.1f;
        bool movingBackward = move.y < -0.1f;
        bool turningLeft = move.x < -0.1f;
        bool turningRight = move.x > 0.1f;

<<<<<<< Updated upstream
        if (!inDialogue) {
=======

        if (mainCamera.GetComponent<lookat>() != null & !mainCamera.GetComponent<lookat>().moveTowards && currentCamera != null)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, currentCamera.transform.position,Time.fixedDeltaTime*5);
        }
        else if (currentCamera == null)
        {
            mainCamera.transform.position = Vector3.MoveTowards(mainCamera.transform.position, this.transform.position ,Time.fixedDeltaTime*5);
        }


        if (!inDialogue) {

>>>>>>> Stashed changes
            // Turn
            rb.MoveRotation(rb.rotation * Quaternion.Euler(0, move.x * turnSpeed * Time.fixedDeltaTime,     0));

            // Move
            Vector3 newPos = rb.position + transform.forward * move.y * moveSpeed * Time.fixedDeltaTime;
            rb.MovePosition(newPos);

            animator.SetBool("forward", movingForward);
            animator.SetBool("backward", movingBackward);
            animator.SetBool("left", turningLeft);
            animator.SetBool("right", turningRight);
        }


<<<<<<< Updated upstream


=======
>>>>>>> Stashed changes
        if (inDialogue && moveAction.WasPressedThisFrame()) {
            if(movingForward){
                DialogueUI.Instance.selected--;
                return;
            }
            else if (movingBackward) {
                DialogueUI.Instance.selected++;
                return;
            }
        }
        gametimer+=Time.fixedDeltaTime;
        if (gametimer>gametickrate)
        {
            resourceIssue = resourceIssueDefault;
            feelings = "I'm fine.";
            canFocus = true;
            // Apply all delta values for each Resource
            foreach (var r in resources)
            {

                resourceLevels[r.title] += r.delta;
                if (resourceLevels[r.title] < 0)
                {
                    resourceLevels[r.title] = 0;
                    resourceIssue = r;
                    feelings = r.complaint;
                    canFocus = false;
                }
                //playerJson[r.title] = resourceLevels[r.title];
            }
            if (mania)
            {
                canFocus = true;
                feelings = maniaResource.complaint;
            }
            gametimer = 0;
            if (busy)
            {
                // If busy apply tool
                updateBusy(heldItem,true);

            }
        }

        if (useAction.WasPressedThisFrame())
        {
            if (!inDialogue && heldItem != null)
            {
                startBusy(heldItem);

            }
        }

        if (debugAction.WasPressedThisFrame())
        {
            consoleReference.SetActive(!consoleReference.activeSelf);
        }

<<<<<<< Updated upstream
=======

>>>>>>> Stashed changes
        if (interactAction.WasPressedThisFrame())
        {
            if(!inDialogue)
            {
                startMonologue("Lets see...");
                return;
            }

            if(inDialogue) {
                if(!inMonologue){
                    if(busy){
                        endBusy();
                        return;
                    }
                    else if(currentEntity && currentEntity.inventory.Length >0)
                    {
                        LoadItem(currentEntity.inventory[DialogueUI.Instance.selected]);
                        endDialogue();
                        startMonologue("I took the " + heldItem.itemName);
                        return;
                    }
                    else if(currentEntity && currentEntity.quests.Length>0)
                    {
                        updateDialogue(currentEntity,DialogueUI.Instance.selected+1);

                    }
                }

                if(inMonologue)
                {
                    var option = DialogueUI.Instance.selected+1;
                    if (option == 1)
                    {
                        endDialogue();
                        startBusy(heldItem);
                        return;
                    }
                    else if (option == 2)
                    {
                        if(currentEntity != null)
                        {
                            endDialogue();
                            startDialogue(currentEntity);
                            return;
                        } else {
                            DialogueUI.Instance.SetText("Nobody is near.");
                        }

                    }
                    else if (option == 3)
                    {
                        DialogueUI.Instance.SetText(feelings);
                    }
                    else if (option == 4)
                    {
                        endDialogue();
                        return;
                    }
                }else if (currentEntity && currentEntity.done)
                {
                    endDialogue();
                    return;
                }
            }

        }
    }
}
