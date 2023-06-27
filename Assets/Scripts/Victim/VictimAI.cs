using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using System.Linq;
using System.Xml.Schema;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.UI;
enum Animations { Walking, LookingAround, Fleeing, Spooked, Interact, Idle, Sitting, Dying };

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Expression))]
public class VictimAI : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] Transform goal;
    [SerializeField] float detectionRate = 1;
    [SerializeField] float detectionDecayRate = 0.1f;
    [SerializeField] float detectionLingerLength = 2;
    [SerializeField] Slider detectionMeter;
    [SerializeField] LayerMask obstructionLayer;
    [SerializeField] LayerMask glassLayer;
    [SerializeField] float detectionSoftRange = 5;
    [SerializeField] float roarHearRange = 15;

    [Header("Spooked")]

    //[SerializeField] float escapeDistance = 20;
    [SerializeField] float escapeSpeed = 0;
    [SerializeField] float frozenLength = 1;
    [SerializeField] float fleeingLength = 5;
    [SerializeField] Slider fearMeter;
    [SerializeField] PlayerMovement monster;
    [SerializeField] LayerMask monsterLayer;


    [Header("Objective")]
    [SerializeField] Objective[] objectives;
    [SerializeField] float walkSpeed = 0;
    [SerializeField] float minDistanceFromTarget = 0.2f;
    [SerializeField] LayerMask groundLayer;
    //[SerializeField] float raycastStartHeight;
    //[SerializeField] float raycastLength;
    [SerializeField] float openDoorAnimationLength;
    [SerializeField] GameStateManager gameStateManager;
    

    [Header("Debug")]
    public float timeElapsedDetection = 0;
    public bool monsterInLOS = false, spooked = false, idle = false, lookingAround = false;
    public bool monsterHasRoared = false, lookingThroughGlass = false;
    bool interactingWithObjective = false;
    //int jumpscareCount = 0;
    float detectionProgress = 0;
    Animations currentAnimation = Animations.Idle;
    int nextObjective = 0;
    AudioManager audioManager;
    AudioSource audioSource;
    [SerializeField]AudioClip walkSound, runSound;

    public Objective CurrentObjective
    {
        get => objectives[nextObjective - 1];
    }
    float DetectionProgress 
    {
        get => detectionProgress;
        set
        {
            detectionProgress = value;
            detectionMeter.value = value;
        }
    }
    float fearLevel = 0;

    float FearLevel
    {
        get => fearLevel;
        set
        {
            fearLevel = value;
            fearMeter.value = value;
        }
    }

    float DistanceFromMonster
    {
        get => (transform.position - monster.transform.position).magnitude;
    }
    bool ReachedDestination
    {
        get => agent.remainingDistance < minDistanceFromTarget;
    }

    NavMeshAgent agent;
    Animator animator;
    Expression expression;
    IEnumerator detectionDecayCoroutine, periodicallyLookAroundCoroutine;
    Camera camera;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
        expression = GetComponent<Expression>();
        audioManager = GetComponent<AudioManager>();
        camera = GetComponentInChildren<Camera>();
        audioSource = GetComponent<AudioSource>();
    }
    private void OnEnable()
    {
        agent.destination = FindNewObjective().transform.position;
        agent.speed = walkSpeed;
        periodicallyLookAroundCoroutine = PeriodicallyLookAround();
        StartCoroutine(periodicallyLookAroundCoroutine);
    }

    // Update is called once per frame
    void Update()
    {
        UpdateTimeElapsed();
        if (!agent.isStopped)
        {
            if (spooked)
            {
                SetAnimation(Animations.Fleeing);
                if (audioSource.clip != runSound || !audioSource.isPlaying)
                {
                    audioSource.clip = runSound;
                    audioSource.Play();
                }
            }
            else
            {
                SetAnimation(Animations.Walking);
                if (audioSource.clip != walkSound || !audioSource.isPlaying)
                {
                    audioSource.clip = walkSound;
                    audioSource.Play();
                }
            }

            Vector3 lookDirection = new Vector3(agent.desiredVelocity.x, 0, agent.desiredVelocity.z);
            transform.rotation = Quaternion.LookRotation(lookDirection == Vector3.zero ? Vector3.forward : lookDirection, Vector3.up);
        }
        else if (audioSource.isPlaying)
            audioSource.Stop();
            
    }
    void UpdateTimeElapsed()
    {
        if (DetectionProgress <= 0)
        {
            timeElapsedDetection = 0;
            monsterHasRoared = false;
        }
        else if (!interactingWithObjective)
            timeElapsedDetection += Time.deltaTime;

    }
    IEnumerator PeriodicallyLookAround()
    {
        while (true)
        {
            float nextStop = Random.Range(8, 15);
            yield return new WaitForSeconds(nextStop);
            if (currentAnimation == Animations.Walking)
                StartCoroutine(LookAround());
        }
    }
    IEnumerator LookAround()
    {
        lookingAround = true;
        animator.Play(Animations.LookingAround.ToString());
        agent.isStopped = true;
        yield return new WaitForSeconds(3.75f);
        agent.isStopped = false;
        animator.Play(Animations.Walking.ToString());
        lookingAround = false;
    }


    public void DetectMonster(Collider other)
    {
        if (spooked)
            return;
        Vector3 direction = other.transform.position - transform.position;
        float length = (other.transform.position - transform.position).magnitude; 
        Physics.Raycast(transform.position + Vector3.up * 1.30f, direction, out RaycastHit obstructionHit, length, obstructionLayer);
        Physics.Raycast(transform.position + Vector3.up * 1.30f, direction, out RaycastHit glassHit, length, glassLayer);
        Debug.DrawRay(transform.position, direction);
        if (obstructionHit.collider != null)
        {
            if (monsterInLOS)
                StopDetectingMonster();
            return;
        }
        lookingThroughGlass = glassHit.collider != null;
        if (!monsterInLOS)
        {
            monsterInLOS = true;
            if (detectionDecayCoroutine != null)
            {
                StopCoroutine(detectionDecayCoroutine);
                detectionDecayCoroutine = null;
            }
        }
        if (monster.frozen)
            return;
        float multiplier = 1;
        if (direction.magnitude < detectionSoftRange)
            multiplier *= 4;
        else if (lookingAround)
            multiplier *= 8;
        DetectionProgress += Time.deltaTime * multiplier * detectionRate;
        CheckDetectionProgress();
    }
    public void CheckDetectionProgress()
    {
        if (DetectionProgress >= 1)
        {
            DetectionProgress = 1;
            StartCoroutine(QueueRunAway());
        }
    }
    IEnumerator QueueRunAway()
    {
        yield return new WaitUntil(() => !interactingWithObjective);
        if (dead)
            yield break;
        StopAllCoroutines();
        StartCoroutine(RunAway(monster.transform.position));
    }

    public void StopDetectingMonster()
    {   if (detectionDecayCoroutine == null && !spooked)
        {
            detectionDecayCoroutine = DetectionDecay();
            StartCoroutine(detectionDecayCoroutine);
        }
        monsterInLOS = false;
    }

    IEnumerator DetectionDecay()
    {            
        yield return new WaitForSeconds(detectionLingerLength);
        while (DetectionProgress > 0)
        {
            yield return null;
            DetectionProgress -= detectionDecayRate;
        }
        if (detectionDecayRate < 0)
            detectionDecayRate = 0;
    }
    void RaiseFear()
    {
        float increase = 0;
        Debug.Log(timeElapsedDetection);
        if (monsterHasRoared)
            increase += 0.02f;
        if (lookingThroughGlass)
            increase += 0.1f;
        if (DistanceFromMonster > 10)
            increase += 0.01f;
        else if (timeElapsedDetection > 1)
            increase += 0.01f;
        else if (timeElapsedDetection > 0.5f)
            increase += 0.05f;
        else
            increase += 0.2f;
        StartCoroutine(RaiseProgressBarOverTime(increase));
    }

    public IEnumerator RaiseProgressBarOverTime(float increase)
    {
        if (increase >= 0.15)
            audioManager.PlaySFX(2);
        else if (increase >= 0.10)
            audioManager.PlaySFX(1);
        else if (increase >= 0.05)
            audioManager.PlaySFX(0);
        const float Length = 2;
        for (float t = 0; t < Length; t += Time.deltaTime)
        {
            FearLevel += increase * Time.deltaTime / Length;
            if (FearLevel >= 1)
            {
                if (!dead)
                {
                    StopAllCoroutines();
                    StartCoroutine(Die());
                }

                yield break;
            }
            yield return null;
        }
    }

    IEnumerator RunAway(Vector3 monsterLocation)
    {
        RaiseFear();
        lookingThroughGlass = false;
        monster.StartFreeze(transform);
        spooked = true;
        agent.isStopped = true;
        Vector3 lookDirection = monsterLocation - transform.position;
        lookDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        animator.Play(Animations.Spooked.ToString());
        expression.ChangeExpression(Expressions.Scared);
        camera.enabled = true;
        yield return new WaitForSeconds(frozenLength);
        camera.enabled = false;
        agent.isStopped = false;
        agent.speed = escapeSpeed;

        yield return new WaitForSeconds(fleeingLength - frozenLength);
        DetectionProgress = 0;
        agent.speed = walkSpeed;
        spooked = false;
        expression.ChangeExpression(Expressions.Neutral);
    }

    //Vector3 FindEscapeLocation()
    //{
    //    const int MAX_ITERATIONS = 50;
    //    int i = 0;
    //    while (true)
    //    {
    //        i++;
    //        if (i >= MAX_ITERATIONS)
    //            return agent.destination;

    //        Vector2 direction = Random.insideUnitCircle.normalized * escapeDistance;
    //        Vector3 rayOrigin = transform.position + new Vector3(direction.x, raycastStartHeight, direction.y);
    //        if (Physics.Raycast(rayOrigin, Vector3.down, out var hit, raycastLength, groundLayer))
    //            return hit.point;
    //    }
    //}

    public void OpenDoor()
    {
        StartCoroutine(OpenDoorAnimation());
    }
    IEnumerator OpenDoorAnimation()
    {
        agent.isStopped = true;
        SetAnimation(Animations.Interact);
        yield return new WaitForSeconds(openDoorAnimationLength);
        agent.isStopped = false;
    }
    public void StartInteractWithObjectiveAnimation(Objective objective) => StartCoroutine(InteractWithObjectiveAnimation(objective));
    public void StartWaitAround(Objective objective) => StartCoroutine(WaitAround(objective));
    public void StartSit(Objective objective) => StartCoroutine(Sit(objective));
    IEnumerator InteractWithObjectiveAnimation(Objective objective)
    {
        agent.isStopped = true;
        transform.eulerAngles = new Vector3(0, objective.angleToLookAt, 0);
        SetAnimation(Animations.Interact);
        interactingWithObjective = true;
        yield return new WaitForSeconds(0.5f);
        objective.InteractionOver();
        yield return new WaitForSeconds(1);
        interactingWithObjective = false;
        objective.done = true;
        agent.destination = FindNewObjective().objectivePosition.position;
        agent.isStopped = false;
    }

    IEnumerator WaitAround(Objective objective)
    {
        if (spooked || FearLevel > 0.6f)
        {
            agent.destination = FindNewObjective().objectivePosition.position;
            objective.done = true;
            yield break;
        }
        agent.isStopped = true;
        SetAnimation(Animations.Idle);
        transform.eulerAngles = new Vector3(0, objective.angleToLookAt, 0);
        agent.destination = FindNewObjective().objectivePosition.position;
        objective.done = true;
        yield return new WaitForSeconds(objective.waitTime);
        agent.isStopped = false;
    }

    IEnumerator Sit(Objective objective)
    {
        if (spooked || FearLevel > 0.45f)
        {
            agent.destination = FindNewObjective().objectivePosition.position;
            objective.done = true;
            yield break;
        }
        transform.eulerAngles = new Vector3(0, objective.angleToLookAt, 0);
        agent.isStopped = true;
        SetAnimation(Animations.Sitting);
        objective.done = true;
        agent.destination = FindNewObjective().objectivePosition.position;
        yield return new WaitForSeconds(objective.waitTime);
        agent.isStopped = false;
    }

    Objective FindNewObjective()
    {
        return objectives[nextObjective++];
        
    }
    public void MonsterVeryClose()
    {
        if (!spooked)
        {
            lookingThroughGlass = false;
            DetectionProgress += 2 * Time.deltaTime;
            CheckDetectionProgress();
        }
    }
    public void DetectMonsterRoar()
    {
        float distance = DistanceFromMonster;
        if (distance > roarHearRange)
            return;
        lookingThroughGlass = false;
        monsterHasRoared = true;
        if (distance < 5)
            DetectionProgress += 2;
        else
        {
            DetectionProgress += 0.25f;
            if (Vector3.Dot(monster.transform.position - transform.position, transform.forward) <= 0)
                transform.Rotate(0, 180, 0);
            StartCoroutine(LookAround());
        }
        CheckDetectionProgress();
    }
    void SetAnimation(Animations animation)
    {
        if (currentAnimation != animation)
        {
            currentAnimation = animation;
            animator.Play(animation.ToString());
        }
    }
    bool dead = false;
    IEnumerator Die()
    {
        agent.isStopped = true;
        SetAnimation(Animations.Dying);
        audioManager.PlaySFX(3);
        expression.ChangeExpression(Expressions.Dead);
        camera.enabled = true;
        dead = true;
        monster.firstPersonCameraInputs.enabled = false;
        yield return new WaitForSeconds(2);
        StartCoroutine(gameStateManager.Win());
    }
}
