using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Audio;

public enum ZombieType
{
    Walker,
    Runner,
    Tanker
}

public enum ZombieState
{
    Dead,
    Alive,
}
public class ZombieControl : MonoBehaviour
{
    public ZombieData zombieData;
    public ZombieType zombieType;
    public ZombieState zombieState;

    private int maxhealth;
    private int curHealth;
    public Transform target;
    public bool isChase;
    public float chaseAngle = 90.0f;
    public float stopDistance = 10.0f;
    public float wanderRadius = 5.0f;
    public float attackRange = 1.0f;
    public float attackDamage = 10.0f;
    private float speed;
    private bool playerHit = false;
    public float hearingDistance = 20.0f;

    private ZombieSoundManager zombieSoundManager;
    private ZombieSoundManager.ZombieSoundData soundData;
    private AudioSource zombieAudioSource;
    private AudioClip zombieChaseSound;
    private AudioClip zombieAttackSound;
    public float soundPlayInterval = 5.0f;
    private float nextSoundTime = 0.0f;
    private float nextAttackSoundTime = 0.0f;
    [SerializeField]
    private LayerMask groundLayer;
    public bool isAttacking { get; private set; }
    public ZombieControl ZombieControlScriptInstance { get; private set; }

    Rigidbody rigid;
    NavMeshAgent nav;

    Animator anim;

    Vector3 wanderPoint;

    private void Start()
    {
        maxhealth = zombieData.HP;
        curHealth = maxhealth;
        target = Player.Instance.transform;
        zombieState = ZombieState.Alive;
        Invoke("ChaseStart", 2);
    }
    public void SetIsAttacking(bool value)
    {
        isAttacking = value;
    }
    void Awake()
    {
        rigid = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponentInChildren<Animator>();
        nav.enabled = false;
        nav.speed = zombieData.Speed;
        zombieAudioSource = gameObject.AddComponent<AudioSource>();
        AudioMixerGroup zombieAudioMixer = Manager.Setting.masterAudioMixerGroup.audioMixer.FindMatchingGroups("Effect")[0];
        zombieAudioSource.outputAudioMixerGroup = zombieAudioMixer;
        zombieAudioSource.loop = false;
        zombieAudioSource.playOnAwake = false;
        zombieAudioSource.volume = 0.2f;
        zombieSoundManager = FindObjectOfType<ZombieSoundManager>();
        if (zombieSoundManager != null)
        {
            soundData = zombieSoundManager.GetZombieSoundData(zombieType);
            if (soundData != null)
            {
                zombieChaseSound = soundData.chaseSound;
                zombieAttackSound = soundData.attackSound;
            }
        }
        zombieChaseSound = soundData != null ? soundData.chaseSound : null;
        zombieAttackSound = soundData != null ? soundData.attackSound : null;
    }

    void ChaseStart()
    {
        isChase = true;
        SetAnimation("isWalk", true);
        SetAnimation("isRun", false);
        SetAnimation("isWalking", true);
    }
    void FixedUpdate()
    {
        if (nav.enabled == false)
            return;
        if (isChase && zombieState == ZombieState.Alive)
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
                        if (zombieType == ZombieType.Runner)
                        {
                            ChaseLogic();
                            SetAnimation("isRun", true);
                            nav.speed = 3.0f;
                        }
                        else
                        {
                            ChaseLogic();
                            SetAnimation("isWalk", true);
                        }
                        if (distanceToTarget <= attackRange)
                        {
                            SetAnimation("isAttack", true);
                            nav.isStopped = true;
                            isAttacking = true;
                            PlayAttackSound();
                        }
                    }
                    else
                    {
                        if (!nav.pathPending && nav.remainingDistance < 0.5f)
                        {
                            WanderLogic();
                        }
                    }
                }
                else
                {
                    if (!nav.pathPending && nav.remainingDistance < 0.5f)
                    {
                        WanderLogic();
                    }
                }
            }
            else
            {
                if (!nav.pathPending && nav.remainingDistance < 0.5f)
                {
                    WanderLogic();
                }
            }
            FreezeVelocity();
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (nav.enabled == true)
            return;

        if (IsInLayerMask(collision.gameObject.layer, groundLayer))
        {
            nav.enabled = true;
        }

    }

    private bool IsInLayerMask(int layer, LayerMask layermask)
    {
        return layermask == (layermask | (1 << layer));
    }

    void SetAnimation(string name, bool value)
    {
        anim.SetBool(name, value);
    }

    void ChaseLogic()
    {
        if (nav.enabled)
        {
            nav.isStopped = false;
            SetAnimation("isAttack", false);
            nav.SetDestination(target.position);
            PlayChaseSound();
        }
    }
    void WanderLogic()
    {
        SetRandomWanderPoint();
        SetAnimation("isRun", false);
        isAttacking = false;
    }
    void PlayAttackSound()
    {
        if (CanPlayAttackSound())
        {
            if (zombieAttackSound != null)
            {
                zombieAudioSource.clip = zombieAttackSound;
                zombieAudioSource.Play();
                nextAttackSoundTime = Time.time + 2.0f;
            }
        }
    }

    void PlayChaseSound()
    {
        if (CanPlayChaseSound())
        {
            if (zombieChaseSound != null)
            {
                zombieAudioSource.clip = zombieChaseSound;
                zombieAudioSource.Play();
                nextSoundTime = Time.time + soundPlayInterval;
            }
        }
    }
    bool CanPlayAttackSound()
    {
        return Time.time >= nextAttackSoundTime && zombieAttackSound != null;
    }

    bool CanPlayChaseSound()
    {
        return Time.time >= nextSoundTime && zombieChaseSound != null;
    }

    public void GunshotHeard()
    {
        float distanceToPlayer = Vector3.Distance(target.position, transform.position);
        if (distanceToPlayer <= hearingDistance)
        {
            if (gameObject.CompareTag("runner"))
            {
                ChaseLogic();
                SetAnimation("isRun", true);
                nav.speed = 3.0f;
            }
            else
            {
                ChaseLogic();
                SetAnimation("isWalk", true);
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
                return true;
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

    public void TakeDamage(int damage, ZombiePartType zombiePartType)
    {
        curHealth -= damage;

        if (isAttacking && !playerHit)
        {
            playerHit = true;
            nav.isStopped = true;
            // 플레이어에 대한 처리
        }
        if (curHealth <= 0)
        {
            curHealth = 0;
            isChase = false;
            nav.enabled = false;
            anim.SetTrigger("doDieFront");
            rigid.isKinematic = true;
            zombieState = ZombieState.Dead;
            Invoke("DestroyEnemy", 5f);
        }
        else
        {
            anim.SetTrigger("getHit");
            nav.isStopped = true;
            rigid.isKinematic = false;
        }
    }

    private void DestroyEnemy()
    {
        GetComponentInParent<Spawner>().ObjectDestroyedSpawn();
        Manager.Resource.Destroy(gameObject);
    }
}