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
        //bloodmat = GetComponentInChildren<MeshRenderer>().material;   // 자식이 있으면 GetComponentInChildren
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
                            //target.GetComponent<PlayerHealth>().TakeDamage(NormalZombieDamage);   //플레이어 스크립트에서 데미지 받는 함수
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
                return true; // 장애물이 목표물 사이에 있는 경우
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
            // 플레이어에 대한 처리
        }
        curHealth -= damage;
        if (curHealth <= 0)
        {
            curHealth = 0;
            Debug.Log("Die");
        }
    }

    //// 무기별 좀비 타격
    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.tag == "bat")
    //    {
    //        //무기 script에서 불러옴 = other.GetComponent<무기 스크립트>();
    //        //curHealth -= 무기.dagmage;
    //        //Debug.Log("test방망이 : " + curHealth);
    //        Vector3 reactVec = transform.position - other.transform.position;
    //        StartCoroutine(OnDamage(reactVec, false));
    //        //if (other.CompareTag("ZombieHead"))
    //        //{
    //        //    curHealth -= 한방.dagmage;
    //        //}
    //    }
    //    else if (other.tag == "bullet")
    //    {
    //        //총알 script에서 불러옴 = other.GetComponent<총알 스크립트>();
    //        //curHealth -= 총알.dagmage;
    //        //Debug.Log("test총알 : " + curHealth);
    //        Vector3 reactVec = transform.position - other.transform.position;
    //        Destroy(other.gameObject);

    //        StartCoroutine(OnDamage(reactVec, false));
    //        //if (other.CompareTag("ZombieHead"))
    //        //{
    //        //    curHealth -= 한방.dagmage;
    //        //}
    //    }
    //}

    public void HitByGrenade(Vector3 explosionPos)
    {
        //curHealth -= 데미지 수치;
        Vector3 reactVec = transform.position - explosionPos;
        StartCoroutine(OnDamage(reactVec, true));
    }

    //피터지는 애니메이션 임시
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
                rigid.AddForce(reactVec * 5, ForceMode.Impulse);        // or 쓰러지는 모션 추가
            }
            Destroy(gameObject, 5);
        }
    }
}