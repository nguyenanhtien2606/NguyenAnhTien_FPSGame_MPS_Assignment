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
    [SerializeField] List<GameObject> zombieModels;
    [SerializeField] int timeToMissTarget = 5;

    float timeDelayToDamage = 0.25f;
    float lastTimeToDamage;
    bool IsDeath;

    Coroutine c_missPlayer;

    PlayerController playerController;

    public bool IsReturnOriginPos
    {
        get;
        set;
    }

    public Vector3 OriginPos
    {
        get;
        set;
    }

    public Vector3 Target
    {
        get;
        set;
    }

    public int EnemyHealth
    {
        get;
        set;
    }

    public int EnemyDamage
    {
        get;
        set;
    }

    public int EnemyPoint
    {
        get;
        set;
    }

    public SpawnEnemy ParentSpawnEnemy
    {
        get;
        set;
    }

    private void Start()
    {
        gameManager = GameManager.GLOBAL;
        playerController = PlayerController.instance;

        //Random model
        zombieModels[Random.Range(0, zombieModels.Count)].SetActive(true);

        navMeshAgent = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();

        SetRandomSpeed();
        FollowTarget();
    }

    private void Update()
    {
        if (!IsDeath && Target != Vector3.zero)
        {
            transform.LookAt(Target);
            transform.eulerAngles = new Vector3(0, transform.eulerAngles.y, 0); //lock x,z axis

            if (navMeshAgent.remainingDistance < 0.2f && IsReturnOriginPos)
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
            //Debug.Log(string.Format("{0} heath reamin: {1}", gameObject.name, EnemyHealth));
            Target = playerController.LookAtPoint.transform.position;
            FollowTarget();
        }
        else
        {
            //die
            ParentSpawnEnemy.SpawnCount -= 1;
            ParentSpawnEnemy.CheckChildLst();
            Destroy(gameObject);
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Debug.Log("I've found you!!!");
            if (c_missPlayer != null)
                StopCoroutine(c_missPlayer);

            IsReturnOriginPos = false;
        }
    }

    private void OnTriggerStay(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Target = playerController.LookAtPoint.transform.position;
            FollowTarget();
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
                playerController.Damaged(EnemyHealth);
                lastTimeToDamage = Time.time;
            }
        }
    }

    IEnumerator C_MissPlayer()
    {
        yield return new WaitForSeconds(timeToMissTarget);

        //Debug.Log("Yeah you escaped!");

        Target = OriginPos;
        IsReturnOriginPos = true;
    }

    public void FollowTarget()
    {
        navMeshAgent.SetDestination(Target);
    }

    public void SetRandomSpeed()
    {
        navMeshAgent.speed = Random.Range(1, 1.5f);
    }
}
