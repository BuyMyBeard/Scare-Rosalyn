using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] GameObject pauseMenu;
    [SerializeField] float inputBuffer = 0.1f;
    [SerializeField] GameStateManager gameStateManager;
    InputAction runAction, roarAction, interactAction, pauseAction;
    InputMap inputs;
    void Awake()
    {
        inputs = new InputMap();
        runAction = inputs.FindAction("Run");
        roarAction = inputs.FindAction("Roar");
        interactAction = inputs.FindAction("Interact");
        pauseAction = inputs.FindAction("Pause");
    }
    private void OnEnable()
    {
        inputs.Enable();
        runAction.started += _ => { RunInput = true; StartCoroutine(TapRun()); };
        runAction.canceled += _ => RunInput = false;
        roarAction.started += _ => { RoarInput = true; StartCoroutine(BufferRoar()); };
        roarAction.canceled += _ => RoarInput = false;
        interactAction.started += _ => { InteractInput = true; StartCoroutine(BufferInteract()); };
        interactAction.canceled += _ => InteractInput = false;
        pauseAction.started += _ =>
        {
            if (gameStateManager.GameEnded)
                return;
            if (Time.timeScale == 0)
            {
                pauseMenu.SetActive(false);
                Cursor.lockState = CursorLockMode.Locked;
                Cursor.visible = false;
                Time.timeScale = 1;

            } else
            {
                pauseMenu.SetActive(true);
                Cursor.lockState = CursorLockMode.None;
                Cursor.visible = true;
                Time.timeScale = 0;
            }
        };
    }

    private void OnDisable()
    {
        inputs.Disable();
    }
    public Vector2 MoveInput 
    {
        get => inputs.Player.Move.ReadValue<Vector2>();
    }
    public Vector2 LookDelta
    {
        get => inputs.Player.Look.ReadValue<Vector2>();
    }
    public bool RunInput { get; private set; } = false;
    public bool RunTap { get; private set; } = false;

    public bool InteractInput { get; private set; } = false;
    public bool InteractPress { get; private set; } = false;

    public bool RoarInput { get; private set; } = false;
    public bool RoarPress { get; private set; } = false;
    IEnumerator BufferRoar()
    {
        RoarPress = true;
        yield return new WaitForSecondsRealtime(inputBuffer);
        RoarPress = false;
    }
    IEnumerator BufferInteract()
    {
        InteractPress = true;
        yield return new WaitForSecondsRealtime(inputBuffer);
        InteractPress = false;
    }
    IEnumerator TapRun()
    {
        RunTap = true;
        yield return new WaitForEndOfFrame();
        RunTap = false;
    }
}
