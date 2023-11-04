using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyAI : MonoBehaviour
{
    private Transform player;

    private PlayerStats playerStats;

    [Header("COMPONENTS REFFERENCES")]

    [SerializeField]
    private NavMeshAgent agent;

    [SerializeField]
    private Animator animator;

    [SerializeField]
    private AudioSource iaContinuousAudioSource;

    [SerializeField]
    private AudioSource iaConditionAudioSource;

    [Header("Stats")]
    [SerializeField]
    private float maxHealth;

    [SerializeField]
    private float currentHealth;

    [SerializeField]
    private float detectionRadius;

    [SerializeField]
    private float attackRadius;

    [SerializeField]
    private float walkSpeed;

    [SerializeField]
    private float chaseSpeed;

    [SerializeField]
    private float attackDelay;

    [SerializeField]
    private float damageDealt;

    [SerializeField]
    private float rotationSpeed;

    [Header("Wandering parameters")]
    [SerializeField]
    private float wanderingWaitTimeMin;

    [SerializeField]
    private float wanderingWaitTimeMax;

    [SerializeField]
    private float wanderingDsitanceMin;

    [SerializeField]
    private float wanderingDsitanceMax;

    private bool isAttacking;
    private bool hasDestination;

    public bool isDead = false;

    void Awake() 
    {
        currentHealth = maxHealth;
        Transform playerTransform = GameObject.FindGameObjectWithTag("Player").transform;
        player = playerTransform;
        playerStats = playerTransform.GetComponent<PlayerStats>();
    }

    void Update()
    {
        if (Vector3.Distance(player.position, transform.position) < detectionRadius && !playerStats.isDead)
        {
            agent.speed = chaseSpeed;

            Quaternion rot = Quaternion.LookRotation(player.position - transform.position);
            transform.rotation = Quaternion.Slerp(transform.rotation, rot, rotationSpeed * Time.deltaTime);

            if (!isAttacking && Vector3.Distance(player.position, transform.position) < attackRadius)
            {
                // coroutine pour ne pas lancer des atttaques en boucle si une attaque est déjà en cours
                StartCoroutine(AttackPlayer());
            }
            else
                agent.SetDestination(player.position);
        }    
        else
        {
            agent.speed = walkSpeed;
            if (agent.remainingDistance < 0.75f && !hasDestination)
            {
                StartCoroutine(GetDestination());
            }
        }

        animator.SetFloat("Speed", agent.velocity.magnitude);
    }

    // Good practice : Affiche la sphere de detection
    private void OnDrawGizmos() 
    {
        Gizmos.color = Color.yellow;
        Gizmos.DrawWireSphere(transform.position, detectionRadius);

        Gizmos.color = Color.red;
        Gizmos.DrawWireSphere(transform.position, attackRadius);
    }

    private IEnumerator GetDestination() 
    {
        hasDestination = true;
        yield return new WaitForSeconds(Random.Range(wanderingWaitTimeMin, wanderingWaitTimeMax));

        Vector3 nextDestination = transform.position;
        nextDestination += Random.Range(wanderingDsitanceMin, wanderingDsitanceMax) * new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
    
        NavMeshHit hit;

        if(NavMesh.SamplePosition(nextDestination, out hit, wanderingDsitanceMax, NavMesh.AllAreas))
        {
            agent.SetDestination(hit.position);
        }

        hasDestination = false;
    }

    private IEnumerator AttackPlayer() 
    {
        isAttacking = true;
        agent.isStopped = true;

        iaConditionAudioSource.Play();
        
        animator.SetTrigger("Attack");
        yield return new WaitForSeconds(attackDelay);
        if (agent.enabled)
        {
            agent.isStopped = false;
            isAttacking = false;
        }
    }

    private void DealtDomage() {
        if (Vector3.Distance(player.position, transform.position) < attackRadius)
            playerStats.TakeDamage(damageDealt);
    }

    public void TakeDamage(float damage) {
        if (isDead) return;

        currentHealth -= damage;
        if (currentHealth <= 0) {
            isDead = true;
            agent.enabled = false;
            animator.SetTrigger("Death");
            iaContinuousAudioSource.enabled = false;
            enabled = false; // script courant 
            
        }
        else
            animator.SetTrigger("GetHit");
    }
}
