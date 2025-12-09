using UnityEngine;
using UnityEngine.AI;
using System.Collections;
using System.Diagnostics.CodeAnalysis;

public class CrowdAI : MonoBehaviour
{
    private NavMeshAgent agent;
    private Animator animator;

    [Header("Hype System")]
    public HypeSystem HypeSystem;

    [Header("NPC Times")]
    public float drinkDuration = 7.0f;   // seconds spent "drinking"
    public float idleMinTime = 20f;       // min interval before next trip
    public float idleMaxTime = 65f;    // max interval before next trip


    [Header("NPC Places")]
    [SerializeField] private Transform danceFloorMin;
    [SerializeField] private Transform danceFloorMax;
    [SerializeField] private Transform DJ;
    [SerializeField] private Transform barMin;
    [SerializeField] private Transform barMax;

    private enum NPCStates
    {
        DanceFloor,
        Walking,
        Drinking
    }
    [SerializeField]
    private NPCStates currentState;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        animator = GetComponent<Animator>();

        StartCoroutine(CrowdRoutine());
        SetDancing(false);
    }

    // Update is called once per frame
    void Update()
    {
        switch (currentState)
        {
            case NPCStates.DanceFloor:
                //Debug.Log(gameObject.name + " is now on the Dance Floor");
                break;
            case NPCStates.Walking:
               // Debug.Log(gameObject.name + " is now Walking");
                break;
        }
    }

    void StartDancing()
    {
        //agent.isStopped = true;
        currentState = NPCStates.DanceFloor;
        animator.SetInteger("danceInt", Random.Range(0, 4));
        animator.SetBool("isDancing", true);
        //gameObject.transform.LookAt(DJ);
    }

    Vector3 GetRandomPos(Vector3 minBounds, Vector3 maxBounds)
    {
        float x = Random.Range(minBounds.x, maxBounds.x);
        float z = Random.Range(minBounds.z, maxBounds.z);
        //Debug.Log(gameObject.name + " traveling to X: " + x + " and Z: " + z);
        return new Vector3(x, 0, z);

    }



    IEnumerator CrowdRoutine()
    {
        while (true)
        {
            // Wait for random interval before walking to the bar
            yield return new WaitForSeconds(Random.Range(idleMinTime, idleMaxTime));

            currentState = NPCStates.Walking;

            // Go to bar
            animator.SetBool("isWalking", true);
            agent.SetDestination(GetRandomPos(barMin.position, barMax.position));
            yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < 0.5f);
            //agent.isStopped = true;

            currentState = NPCStates.Drinking;
            animator.SetBool("isWalking", false);
            //animator.SetTrigger("grabDrink");    // play "grab/drink" animation

            yield return new WaitForSeconds(drinkDuration); // simulate time spent at the bar

            // Return to dance floor
            currentState = NPCStates.Walking;
            animator.SetBool("isWalking", true);
            agent.SetDestination(GetRandomPos(danceFloorMin.position, danceFloorMax.position));
            yield return new WaitUntil(() => !agent.pathPending && agent.remainingDistance < 0.5f);

            gameObject.transform.LookAt(DJ);
            currentState = NPCStates.DanceFloor;
            animator.SetBool("isWalking", false);
        }

    }


    public void SetDancing(bool dancing)
    {
        if (dancing)
        {
            StartDancing();
        }

        if (!dancing && HypeSystem.hype > 0)
        {
            animator.SetBool("isDancing", false);
            animator.SetBool("musicPlaying", true);
        }
        else if (!dancing && HypeSystem.hype == 0)
        {
            animator.SetBool("isDancing", false);
            animator.SetBool("musicPlaying", false);
        }
    }
}

