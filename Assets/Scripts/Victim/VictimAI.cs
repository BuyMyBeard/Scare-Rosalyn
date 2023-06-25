using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations;
using UnityEngine.UI;


public abstract class Objective : MonoBehaviour
{
    public Transform objectivePosition;
    public bool found = false;
    [SerializeField] int collectibleId;
    [SerializeField] CheckboxList checkboxList;
    public abstract void InteractionOver();
    protected void Check()
    {
        checkboxList.Check(collectibleId);
    }
}
enum Animations { Walking, LookingAround, Fleeing, Spooked, Interact };

[RequireComponent(typeof(NavMeshAgent))]
[RequireComponent(typeof(Animator))]
[RequireComponent(typeof(Expression))]
public class VictimAI : MonoBehaviour
{
    [Header("Detection")]
    [SerializeField] Transform goal;
    [SerializeField] float detectionCoefficient = 1;
    [SerializeField] float detectionDecayRate = 0.1f;
    [SerializeField] float detectionLingerLength = 2;
    [SerializeField] Slider detectionMeter;
    [SerializeField] LayerMask obstructionLayer;

    [Header("Spooked")]

    [SerializeField] float escapeDistance = 20;
    [SerializeField] float escapeSpeed = 0;
    [SerializeField] float frozenLength = 1;
    [SerializeField] Slider fearMeter;

    [Header("Objective")]
    [SerializeField] Objective[] objectives;
    [SerializeField] Objective finalObjective;
    [SerializeField] float walkSpeed = 0;
    [SerializeField] float minDistanceFromTarget = 0.2f;
    [SerializeField] LayerMask groundLayer;
    [SerializeField] float raycastStartHeight;
    [SerializeField] float raycastLength;
    [SerializeField] float openDoorAnimationLength;


    [Header("Debug")]
    public bool monsterInLOS = false, spooked = false, idle = false;
    float detectionProgress = 0;
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
    bool ReachedDestination
    {
        get => agent.remainingDistance < minDistanceFromTarget;
    }

    NavMeshAgent agent;
    Animator animator;
    Expression expression;
    IEnumerator detectionDecayCoroutine, lookAroundCoroutine;
    void Awake()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();
       expression = GetComponent<Expression>();
    }
    private void OnEnable()
    {
        agent.destination = FindNewObjective().transform.position;
        agent.speed = walkSpeed;
        lookAroundCoroutine = LookAround();
        StartCoroutine(lookAroundCoroutine);
    }

    // Update is called once per frame
    void Update()
    {

        if (!agent.isStopped)
        {
            Vector3 lookDirection = new Vector3(agent.desiredVelocity.x, 0, agent.desiredVelocity.z);
            transform.rotation = Quaternion.LookRotation(lookDirection == Vector3.zero ? Vector3.forward : lookDirection, Vector3.up);
        }
        else
        {
            if (ReachedDestination && !idle)
            {
                animator.Play("Idle");
                idle = true;
            }
        }
    }
    IEnumerator LookAround()
    {
        while (true)
        {
            animator.Play(Animations.Walking.ToString());
            float nextStop = Random.Range(10, 30);
            yield return new WaitForSeconds(nextStop);
            animator.Play(Animations.LookingAround.ToString());
            agent.isStopped = true;
            yield return new WaitForSeconds(3.75f);
            agent.isStopped = false;
        }
    }
    public void DetectMonster(Collider other)
    {
        if (spooked)
            return;
        Vector3 direction = other.transform.position - transform.position;
        Physics.Raycast(transform.position, direction, out RaycastHit obstructionHit, 50, obstructionLayer);
        if (obstructionHit.collider != null)
        {
            if (monsterInLOS)
                StopDetectingMonster();
            return;
        }
        if (!monsterInLOS)
        {
            monsterInLOS = true;
            if (detectionDecayCoroutine != null)
            {
                StopCoroutine(detectionDecayCoroutine);
                detectionDecayCoroutine = null;
            }
        }

        DetectionProgress += detectionCoefficient / direction.magnitude;
        if (DetectionProgress >= 1)
        {
            DetectionProgress = 1;
            StopAllCoroutines();
            StartCoroutine(RunAway(other.transform.position));
        }
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
    IEnumerator RunAway(Vector3 monsterLocation)
    {
        spooked = true;
        agent.isStopped = true;
        Vector3 lookDirection = monsterLocation - transform.position;
        lookDirection.y = 0;
        transform.rotation = Quaternion.LookRotation(lookDirection, Vector3.up);
        animator.Play(Animations.Spooked.ToString());
        expression.ChangeExpression(Expressions.Scared);
        yield return new WaitForSeconds(frozenLength);

        agent.isStopped = false;
        agent.speed = escapeSpeed;
        animator.Play(Animations.Fleeing.ToString());
        agent.destination = FindEscapeLocation();
        
        yield return new WaitUntil(() => ReachedDestination);
        DetectionProgress = 0;
        agent.speed = walkSpeed;
        agent.destination = FindNewObjective().transform.position;
        spooked = false;
        animator.Play(Animations.Walking.ToString());
        expression.ChangeExpression(Expressions.Neutral);
    }

    Vector3 FindEscapeLocation()
    {
        const int MAX_ITERATIONS = 50;
        int i = 0;
        while (true)
        {
            i++;
            if (i >= MAX_ITERATIONS)
                return agent.destination;

            Vector2 direction = Random.insideUnitCircle.normalized * escapeDistance;
            Vector3 rayOrigin = transform.position + new Vector3(direction.x, raycastStartHeight, direction.y);
            if (Physics.Raycast(rayOrigin, Vector3.down, out var hit, raycastLength, groundLayer))
                return hit.point;
        }
    }

    public void OpenDoor()
    {
        StartCoroutine(OpenDoorAnimation());
    }
    IEnumerator OpenDoorAnimation()
    {
        agent.isStopped = true;
        StopCoroutine(lookAroundCoroutine);
        animator.Play(Animations.Interact.ToString());
        yield return new WaitForSeconds(openDoorAnimationLength);
        animator.Play(Animations.Walking.ToString());
        agent.isStopped = false;
        lookAroundCoroutine = LookAround();
        StartCoroutine(lookAroundCoroutine);
    }
    public void InteractWithObjective(Objective objective)
    {
        if (spooked)
            return;
        StartCoroutine(InteractWithObjectiveAnimation(objective));
    }
    IEnumerator InteractWithObjectiveAnimation(Objective objective)
    {
        agent.isStopped = true;
        StopCoroutine(lookAroundCoroutine);
        animator.Play(Animations.Interact.ToString());
        yield return new WaitForSeconds(0.5f);
        objective.InteractionOver();
        yield return new WaitForSeconds(1);
        objective.found = true;
        agent.destination = FindNewObjective().objectivePosition.position;
        animator.Play(Animations.Walking.ToString());
        agent.isStopped = false;
        lookAroundCoroutine = LookAround();
        StartCoroutine(lookAroundCoroutine);
    }

    Objective FindNewObjective()
    {
        if (objectives.All(o => o.found))
            return finalObjective;
        while (true)
        {
            Objective objective = objectives[Random.Range(0, objectives.Length)];
            if (!objective.found)
                return objective;
        }
    }

}
