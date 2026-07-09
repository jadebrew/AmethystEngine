using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.SceneManagement;

public class mainmenu : MonoBehaviour
{
    private PlayerInput playerInput;
    private InputAction startAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        playerInput = GetComponent<PlayerInput>();
        startAction = playerInput.actions["Start"];
    }

    // Update is called once per frame
    void Update()
    {
        if (startAction.WasPressedThisFrame())
        {
            SceneManager.LoadScene("GameScene", LoadSceneMode.Single);
        }
    }
}
