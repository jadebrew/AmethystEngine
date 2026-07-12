using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;
using System.Collections.Generic;
using System.IO;
using SimpleJSON;
using TMPro;

public class mainmenu : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction startAction;
    private InputAction navigateAction;

    public GameObject button;
    public GameObject buttonParent;

    private Dictionary<int,GameObject> buttons = new Dictionary<int,GameObject>();
    private int selected = 0;
    private int options;

    private bool isPressed;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        startAction = playerInput.actions["Submit"];
        navigateAction = playerInput.actions["Navigate"];
        string[] menu = {"New Game"};
        SetMenu(menu);
    }

    public void SetMenu(string[] menu)
    {
        buttons = new Dictionary<int,GameObject>();
        options = menu.Length;
        foreach (Transform child in buttonParent.transform) {
            GameObject.Destroy(child.gameObject);
        }
        selected = 0;
        foreach (string item in menu)
        {
            buttons.Add(selected,Instantiate(button,buttonParent.transform));
            buttons[selected].transform.Find("MenuItemName").gameObject.GetComponent<TMP_Text>().text = item;
            selected++;
        }
        selected = 0;
    }

    // Update is called once per frame
    void Update()
    {

        if (!isPressed && navigateAction.WasPressedThisFrame())
        {
            isPressed = true;
            Vector2 navi = navigateAction.ReadValue<Vector2>();
            if (navi.y<-0.1f)
                selected++;
            if (navi.y>0.1f)
                selected--;
        } else if (isPressed && !navigateAction.WasPressedThisFrame()) {
            isPressed = false;
        }
        if (selected >= options) {
            selected = 0;
        }
        if (selected < 0) {
            selected = options-1;
        }
        if (startAction.WasPressedThisFrame())
        {
            if (selected == 0)
            {
                SceneManager.LoadScene("LoadingScene", LoadSceneMode.Single);
            } else if (selected == 1)
            {
                SceneManager.LoadScene("LoadingScene", LoadSceneMode.Single);
            }
        }
        foreach( KeyValuePair<int, GameObject> btn in buttons )
        {
            btn.Value.transform.Find("Icon").gameObject.SetActive(btn.Key == selected);
        }
    }
}
