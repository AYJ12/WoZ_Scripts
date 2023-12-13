using System.Collections;
using UnityEngine;
using UnityEngine.AI;

public class ZombieTest : MonoBehaviour
{
    public int maxhealth;
    public int curHealth;
    public Transform target;
    public bool isChase;
    public float chaseAngle = 90.0f;
    public float stopDistance = 10.0f;
    public float wanderRadius = 5.0f;
    public float attackRange = 1.0f;
    public int attackDamage = 10;
    public float speed = 3.0f;
    private bool isAttacking = false;
    private bool playerHit = false;

    Material bloodmat;

    Rigidbody rigid;
    CapsuleCollider Capsule;
    NavMeshAgent nav;

    Animator anim;

    Vector3 wanderPoint;

    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        Capsule = GetComponent<CapsuleCollider>();
        //bloodmat = GetComponentInChildren<MeshRenderer>().material;   // �ڽ��� ������ GetComponentInChildren
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        nav.speed = speed;
        Invoke("ChaseStart", 2);
        target = GameObject.FindWithTag("Player").transform;
    }

    void ChaseStart()
    {
        isChase = true;
        anim.SetBool("isWalk", true);
        anim.SetBool("isRun", false);
        anim.SetBool("isWalking", true);
    }

    void Update()
    {
        if (isChase)
        {
            Vector3 targetDirection = target.position - transform.position;
            targetDirection.y = 0;

            Vector3 zombieForward = transform.forward;

            float angle = Vector3.Angle(zombieForward, targetDirection);

            if (angle <= chaseAngle)
            {
                float distanceToTarget = Vector3.Distance(target.position, transform.position);
                if (distanceToTarget <= stopDistance)
                {
                    if (!IsTargetHiddenByObstacles())
                    {
                        nav.isStopped = false;
                        anim.SetBool("isAttack", false);
                        anim.SetBool("isWalk", true);
                        anim.SetBool("isRun", true);
                        nav.SetDestination(target.position);

                        if (distanceToTarget <= attackRange)
                        {
                            anim.SetBool("isAttack", true);
                            nav.isStopped = true;
                            isAttacking = true;
                            //target.GetComponent<PlayerHealth>().TakeDamage(NormalZombieDamage);   //�÷��̾� ��ũ��Ʈ���� ������ �޴� �Լ�
                        }
                    }
                    else
                    {
                        if (!nav.pathPending && nav.remainingDistance < 0.5f)
                        {
                            SetRandomWanderPoint();
                            isAttacking = false;
                        }
                    }
                }
                else
                {
                    if (!nav.pathPending && nav.remainingDistance < 0.5f)
                    {
                        SetRandomWanderPoint();
                        isAttacking = false;
                    }
                }
            }
            else
            {
                if (!nav.pathPending && nav.remainingDistance < 0.5f)
                {
                    SetRandomWanderPoint();
                    isAttacking = false;
                }
            }
        }
    }

    bool IsTargetHiddenByObstacles()
    {
        RaycastHit hit;
        Vector3 direction = (target.position - transform.position).normalized;
        float distance = Vector3.Distance(target.position, transform.position);

        if (Physics.Raycast(transform.position, direction, out hit, distance))
        {
            if (hit.transform != target)
            {
                return true; // ��ֹ��� ��ǥ�� ���̿� �ִ� ���
            }
        }
        return false;
    }

    void SetRandomWanderPoint()
    {
        Vector3 randomDirection = Random.insideUnitSphere * wanderRadius;
        randomDirection += transform.position;
        NavMeshHit hit;
        NavMesh.SamplePosition(randomDirection, out hit, wanderRadius, 1);
        wanderPoint = hit.position;
        nav.SetDestination(wanderPoint);
    }

    void FreezeVelocity()
    {
        if (isChase)
        {
            rigid.velocity = Vector3.zero;
            rigid.angularVelocity = Vector3.zero;
        }
    }
    private void FixedUpdate()
    {
        FreezeVelocity();
    }

    public void TakeDamage(int damage)
    {
        if (isAttacking && !playerHit)
        {
            playerHit = true;
            // �÷��̾ ���� ó��
        }
        curHealth -= damage;
        if (curHealth <= 0)
        {
            curHealth = 0;
            Debug.Log("Die");
        }
    }

    //// ���⺰ ���� Ÿ��
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "bat")
    //    {
    //        //���� script���� �ҷ��� = other.GetComponent<���� ��ũ��Ʈ>();
    //        //curHealth -= ����.dagmage;
    //        //Debug.Log("test����� : " + curHealth);
    //        Vector3 reactVec = transform.position - other.transform.position;
    //        StartCoroutine(OnDamage(reactVec, false));
    //        //if (other.CompareTag("ZombieHead"))
    //        //{
    //        //    curHealth -= �ѹ�.dagmage;
    //        //}
    //    }
    //    else if (other.tag == "bullet")
    //    {
    //        //�Ѿ� script���� �ҷ��� = other.GetComponent<�Ѿ� ��ũ��Ʈ>();
    //        //curHealth -= �Ѿ�.dagmage;
    //        //Debug.Log("test�Ѿ� : " + curHealth);
    //        Vector3 reactVec = transform.position - other.transform.position;
    //        Destroy(other.gameObject);

    //        StartCoroutine(OnDamage(reactVec, false));
    //        //if (other.CompareTag("ZombieHead"))
    //        //{
    //        //    curHealth -= �ѹ�.dagmage;
    //        //}
    //    }
    //}

    public void HitByGrenade(Vector3 explosionPos)
    {
        //curHealth -= ������ ��ġ;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec, true));
    }

    //�������� �ִϸ��̼� �ӽ�
    IEnumerator OnDamage(Vector3 reactVec, bool isGrenade)
    {
        bloodmat.color = Color.red;
        yield return new WaitForSeconds(0.1f);

        if (curHealth > 0)
        {
            bloodmat.color = Color.white;
        }
        else
        {
            bloodmat.color = Color.red;
            gameObject.layer = 21;
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("doDieFront");

            if (isGrenade)
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up * 5;

                rigid.freezeRotation = false;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);
                rigid.AddTorque(reactVec * 15, ForceMode.Impulse);
            }
            else
            {
                reactVec = reactVec.normalized;
                reactVec += Vector3.up;
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);        // or �������� ��� �߰�
            }
            Destroy(gameObject, 5);
        }
    }
}