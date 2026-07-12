using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.InputSystem;
using TMPro;

public class LoadScene : MonoBehaviour
{
    public string sceneName;
    public TMP_Text loading_label;
    public TMP_Text brieving_label;
    private PlayerInput playerInput;
    private InputAction startAction;
    public GameObject rotatingCD;
    PersistentData persistentData;

    public float timer = 0;
    public float minDelay = 2;

    private GameObject player;

    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        startAction = playerInput.actions["Submit"];
        loading_label.text = "Please Wait";
        player = GameObject.Find("player");
        persistentData = GameObject.Find("PersistentData").GetComponent<PersistentData>();
        brieving_label.text = persistentData.currentBrief;
        if (player != null)
        {
            player.GetComponent<PlayerBehavior>().gameState = 2;

        }
    }

    void FixedUpdate()
    {
        if (timer < minDelay)
        {
            timer += Time.fixedDeltaTime;
            rotatingCD.GetComponent<rotate>().rotateSpeed = (minDelay-timer) * 10;
        }
        else
        {
            loading_label.text = "Press Start";
            if (startAction.WasPressedThisFrame())
            {
            StartCoroutine(LoadSceneAsync());

            }
        }
    }

    void DoneLoading()
    {

    }

    IEnumerator LoadSceneAsync()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(sceneName);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            loading_label.text = "Please Wait";
            rotatingCD.GetComponent<rotate>().rotateSpeed += asyncLoad.progress * 100;
            yield return new WaitUntil(() => asyncLoad.isDone);
            DoneLoading();
        }
    }
}
