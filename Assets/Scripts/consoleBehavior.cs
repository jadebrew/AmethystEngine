using UnityEngine;
using UnityEngine.InputSystem;
using TMPro;

public class consoleBehavior : MonoBehaviour
{
    public GameObject consoleOutputRef;
    public GameObject consoleInput;
    private PlayerInput playerInput;
    private InputAction submitAction;

    void Awake()
    {
        submitAction = playerInput.actions["Submit"];
    }

    void Update()
    {
        if (submitAction.WasPressedThisFrame())
        {
            consoleOutputRef.GetComponent<TMP_Text>().text += "\n" + consoleInput.GetComponent<TMP_InputField>().text;
            consoleInput.GetComponent<TMP_InputField>().text = "";
        }

    }
}
