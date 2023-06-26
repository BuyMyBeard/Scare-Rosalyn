using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using Unity.PlasticSCM.Editor.WebApi;
using Unity.VisualScripting.Antlr3.Runtime;
using UnityEngine;
using UnityEngine.AI;

enum MonsterAnimations { Idle, Walking };

[RequireComponent(typeof(PlayerInputs))]
[RequireComponent(typeof(CharacterController))]
public class PlayerMovement : MonoBehaviour
{
    [SerializeField] float walkSpeed = 1;
    [SerializeField] float runMultiplier = 2;
    [SerializeField] float frozenLength = 3;
    [SerializeField] CinemachineVirtualCamera firstPersonCamera;
    CinemachineInputProvider firstPersonCameraInputs;
    float fallingSpeed = 0;
    public bool frozen;
    PlayerInputs inputs;
    CharacterController controller;
    Camera cam;
    Animator animator;
    MonsterAnimations currentAnimation;
    //NavMeshObstacle obstacle;
    void Awake()
    {
        inputs = GetComponent<PlayerInputs>();
        controller = GetComponent<CharacterController>();
        animator = GetComponent<Animator>();
        cam = Camera.main;
        firstPersonCameraInputs = firstPersonCamera.GetComponent<CinemachineInputProvider>();
        //obstacle = GetComponent<NavMeshObstacle>();
    }

    void Update()
    {
        if (frozen)
            return;
        Vector3 lookDirection = new Vector3(0, cam.transform.eulerAngles.y - 6.72f, 0);
        transform.eulerAngles = lookDirection;
        Vector3 movement = new Vector3(inputs.MoveInput.x, 0, inputs.MoveInput.y);
        if (movement ==  Vector3.zero)
        {
            SetAnimation(MonsterAnimations.Idle);
            return;
        }
        SetAnimation(MonsterAnimations.Walking);
        float currentSpeed = walkSpeed;
        if (inputs.RunInput && movement.z > 0)
        {
            animator.speed = runMultiplier;
            movement.z *= runMultiplier;
            currentSpeed *= runMultiplier;
        }
        else
            animator.speed = 1;
        movement = cam.transform.forward * movement.z + cam.transform.right * movement.x;
        movement.y = 0;
        controller.SimpleMove(currentSpeed * movement.normalized + fallingSpeed * Vector3.down);
    }
    void SetAnimation(MonsterAnimations animation)
    {
        if (currentAnimation != animation)
        {
            currentAnimation = animation;
            animator.Play(currentAnimation.ToString());
        }
    }
    public void Freeze(Transform victimToLookAt)
    {
        StartCoroutine(FreezePlayer(victimToLookAt));
    }
    IEnumerator FreezePlayer(Transform victimToLookAt)
    {
        //obstacle.enabled = true;
        SetAnimation(MonsterAnimations.Idle);
        frozen = true;
        firstPersonCameraInputs.enabled = false;
        //firstPersonCamera.LookAt = victimToLookAt;
        
        yield return new WaitForSeconds(frozenLength);
        frozen = false;
        //firstPersonCamera.LookAt = null;
        firstPersonCameraInputs.enabled = true;
        //obstacle.enabled = false;
    }
}
