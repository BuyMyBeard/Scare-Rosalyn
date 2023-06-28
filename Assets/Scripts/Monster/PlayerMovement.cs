using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.InputSystem;

enum MonsterAnimations { Idle, Walking };

[RequireComponent(typeof(PlayerInputs))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 1;
    [SerializeField] float runMultiplier = 2;
    [SerializeField] float frozenLength = 3;
    public bool frozen = false;
    bool runToggleMode, runToggled = false;
    PlayerInputs inputs;
    CharacterController controller;
    Camera cam;
    Animator animator;
    float mouseSensitivity;
    float verticalRotation = 0;
    [SerializeField] Transform head;
    MonsterAnimations currentAnimation;
    [SerializeField] AudioClip walkSound, runSound;
    [SerializeField] int verticalClamp = 90;
    //CapsuleCollider collider;
    //SphereCollider sphereCollider;
    //NavMeshObstacle obstacle;
    AudioSource audioSource;
    void Awake()
    {
        inputs = GetComponent<PlayerInputs>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        //collider = GetComponent<CapsuleCollider>();
        //sphereCollider = GetComponentInChildren<SphereCollider>();
        cam = Camera.main;
        audioSource = GetComponent<AudioSource>();
        //obstacle = GetComponent<NavMeshObstacle>();
        UpdateSettings();
    }

    void Update()
    {
        if (frozen)
            return;
        Vector2 delta = mouseSensitivity * Time.deltaTime * Mouse.current.delta.ReadValue();

        verticalRotation -= delta.y;
        verticalRotation = Mathf.Clamp(verticalRotation, -verticalClamp, verticalClamp);
        head.localRotation = Quaternion.Euler(verticalRotation, 0, 0);

        transform.Rotate(Vector3.up * delta.x);
        //Vector3 lookDirection = new Vector3(0, cam.transform.eulerAngles.y - 6.72f, 0);
        //transform.eulerAngles = lookDirection;
        Vector3 movement = new Vector3(inputs.MoveInput.x, 0, inputs.MoveInput.y);
        if (movement ==  Vector3.zero)
        {
            runToggled = false;
            SetAnimation(MonsterAnimations.Idle);
            audioSource.Stop();
            return;
        }
        SetAnimation(MonsterAnimations.Walking);
        float currentSpeed = walkSpeed;
        if ((runToggled || inputs.RunInput) && movement.z > 0)
        {
            if (runToggleMode && inputs.RunTap)
            {
                runToggled = !runToggled;
            }
            if (audioSource.clip != runSound || !audioSource.isPlaying)
            {
                audioSource.clip = runSound;
                audioSource.Play();
            }
            animator.speed = runMultiplier;
            movement.z *= runMultiplier;
            currentSpeed *= runMultiplier;
        }
        else
        {
            runToggled = false;
            animator.speed = 1;
            if (audioSource.clip != walkSound || !audioSource.isPlaying)
            {
                audioSource.clip = walkSound;
                audioSource.Play();
            }
        }
        movement = cam.transform.forward * movement.z + cam.transform.right * movement.x;
        movement.y = 0;
        controller.SimpleMove(currentSpeed * movement.normalized);
    }
    void SetAnimation(MonsterAnimations animation)
    {
        if (currentAnimation != animation)
        {
            currentAnimation = animation;
            animator.Play(currentAnimation.ToString());
        }
    }
    public void StartFreeze(Transform victimToLookAt)
    {
        StartCoroutine(FreezePlayer(victimToLookAt));
    }
    IEnumerator FreezePlayer(Transform victimToLookAt)
    {
        audioSource.Stop();
        //obstacle.enabled = true;
        SetAnimation(MonsterAnimations.Idle);
        frozen = true;
        //firstPersonCamera.LookAt = victimToLookAt;
        
        yield return new WaitForSeconds(frozenLength);
        frozen = false;
        //firstPersonCamera.LookAt = null;
        //obstacle.enabled = false;
    }
    public void EnterCar()
    {
        frozen = true;
        //collider.enabled = false;
        //sphereCollider.enabled = false;
    }
    public void ExitCar()
    {
        frozen = false;
        //collider.enabled = true;
        //sphereCollider.enabled = true;
    }
    public void UpdateSettings()
    {
        runToggleMode = GameSettings.ToggleRun;
        mouseSensitivity = GameSettings.MouseSensitivity;
    }
}
