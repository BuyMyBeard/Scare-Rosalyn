using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputs : MonoBehaviour
{
    [SerializeField] float inputBuffer = 0.1f;
    InputAction runAction, roarAction, interactAction;
    InputMap inputs;
    int i = 0;
    void Awake()
    {
        inputs = new InputMap();
        runAction = inputs.FindAction("Run");
        roarAction = inputs.FindAction("Roar");
        interactAction = inputs.FindAction("Interact");
    }
    private void OnEnable()
    {
        inputs.Enable();
        runAction.started += _ => RunInput = true;
        runAction.canceled += _ => RunInput = false;
        roarAction.started += _ => { RoarInput = true; StartCoroutine(BufferRoar()); };
        roarAction.canceled += _ => RoarInput = false;
        interactAction.started += _ => { InteractInput = true; StartCoroutine(BufferInteract()); };
        interactAction.canceled += _ => InteractInput = false;
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
    public bool RunInput { get; private set; }
    public bool InteractInput { get; private set; }
    public bool InteractPress { get; private set; }

    public bool RoarInput { get; private set; }
    public bool RoarPress { get; private set; }
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
}
