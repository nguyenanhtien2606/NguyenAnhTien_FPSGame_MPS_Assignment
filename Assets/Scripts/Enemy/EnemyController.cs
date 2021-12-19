using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class EnemyController : MonoBehaviour
{
    GameManager gameManager;

    NavMeshAgent navMeshAgent;
    Animator anim;

    [Header("Enemy Settings")]
    [SerializeField] int enemyHealth = 100;
    [SerializeField] int enemyDamage = 10;
    [SerializeField] int timeToMissTarget = 3;

    [Space]
    [SerializeField] int enemyPoint = 10;

    float timeDelayToDamage = 0.25f;
    float lastTimeToDamage;
    bool IsDeath;

    Coroutine c_missTarget;

    PlayerController playerController;

    public GameObject Target
    {
        get;
        private set;
    }

    public int EnemyHealth
    {
        get { return enemyHealth; }
        private set { enemyHealth = value; }
    }

    private void Start()
    {
        gameManager = GameManager.GLOBAL;

        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
    }

    private void Update()
    {
        if (Target != null)
        {
            transform.LookAt(Target.transform.position);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0); //lock x,z axis
            FollowTarget();
        }

        anim.SetBool("IsWaking", Target != null);
    }

    public void Damaged(int damage)
    {
        EnemyHealth -= damage;
        if (EnemyHealth > 0)
        {
            Debug.Log(string.Format("{0} heath reamin: {1}", gameObject.name, enemyHealth));
        }
        else
        {
            //die
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("I've found you!!!");
            playerController = other.gameObject.GetComponent<PlayerController>();
            Target = playerController.LookAtPoint;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (c_missTarget != null)
                StopCoroutine(c_missTarget);

            c_missTarget = StartCoroutine(C_MissTarget());
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if ((lastTimeToDamage + timeDelayToDamage) < Time.time)
            {
                if (playerController == null)
                    playerController = collision.gameObject.GetComponent<PlayerController>();
                
                playerController.Damaged(enemyDamage);
                lastTimeToDamage = Time.time;
            }
        }
    }

    IEnumerator C_MissTarget()
    {
        yield return new WaitForSeconds(timeToMissTarget);
        Target = null;
        
        navMeshAgent.isStopped = true;
        Debug.Log("Yeah you escaped!");
    }

    void FollowTarget()
    {
        if (!IsDeath && Target != null)
        {
            navMeshAgent.isStopped = false;
            navMeshAgent.SetDestination(Target.transform.position);
        }
    }
}
