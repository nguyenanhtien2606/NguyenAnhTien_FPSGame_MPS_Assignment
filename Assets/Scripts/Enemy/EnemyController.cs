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

    Coroutine c_missPlayer;

    PlayerController playerController;

    Vector3 originPos;
    bool isReturnOriginPos;

    public Vector3 Target
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

        originPos = gameObject.transform.position;
    }

    private void Update()
    {
        if (Target != Vector3.zero)
        {
            transform.LookAt(Target);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0); //lock x,z axis
            FollowTarget();

            if (navMeshAgent.remainingDistance < 0.2f && isReturnOriginPos)
            {
                anim.SetBool("IsWaking", false);
            }
            else
            {
                anim.SetBool("IsWaking", true);
            }
        } 
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
            if (c_missPlayer != null)
                StopCoroutine(c_missPlayer);

            playerController = other.gameObject.GetComponent<PlayerController>();
            isReturnOriginPos = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (playerController == null)
                playerController = other.gameObject.GetComponent<PlayerController>();

            Target = playerController.LookAtPoint.transform.position;
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (c_missPlayer != null)
                StopCoroutine(c_missPlayer);

            c_missPlayer = StartCoroutine(C_MissPlayer());
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

    IEnumerator C_MissPlayer()
    {
        yield return new WaitForSeconds(timeToMissTarget);

        Debug.Log("Yeah you escaped!");

        Target = originPos;
        isReturnOriginPos = true;
    }

    void FollowTarget()
    {
        if (!IsDeath && Target != null)
        {
            navMeshAgent.SetDestination(Target);
        }
    }
}
