using System.Collections;
using UnityEngine;


public class WeaponSniper : WeaponBase
{
    [Header("Spawn Points")]
    [SerializeField] private Transform casingSpawnPoint;    // ź�� ���� ��ġ
    [SerializeField] Transform bulletSpawnPoint;            // �Ѿ� ���� ��ġ

    [Header("Fire Effects")]
    [SerializeField] private GameObject muzzleFlashEffect;  // �ѱ� �̺�Ʈ(On/ Off)

    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipFire;       // ���� ����
    [SerializeField] private AudioClip audioClipReload; // ������ ����

    [Header("Zoom")]
    [SerializeField] private GameObject zoom;   // �� ȭ��
    private GameObject zoomSVD;
    private GameObject zoomAWM;

    public PlayerLookType _preLookType;
    private float zoomModeFOV = 5;                          // AIM��忡���� ī�޶� FOV

    private CasingMemoryPool casingMemoryPool;  // ź�� ���� �� Ȱ������ ����
    private ImpactMemoryPool impactMemoryPool;  // ���� ȿ�� ���� �� Ȱ�� ���� ����

    private void Awake()
    {
        base.Setup(); // ��� Ŭ������ �ʱ�ȭ�� ���� Setup() ȣ��

        casingMemoryPool = GetComponent<CasingMemoryPool>();
        impactMemoryPool = GetComponent<ImpactMemoryPool>();
    }

    private void OnEnable()
    {
        mainCamera.fieldOfView = defaultModeFOV; // �� ��� ī�޶� off
        mainCamera.cullingMask = mainCamera.cullingMask | (1 << 11); // �� ���̱�

        PlaySound(audioClipTakeOutWeapon);  // ���� ���� ����
        muzzleFlashEffect.SetActive(false); // �ѱ� ȿ�� ������Ʈ ��Ȱ��ȭ

        _preLookType = PlayerLookType.Idle;

        ResetVariables();
    }
    private void Start()
    {
        zoom = Manager.UI.SceneUI.GetObject((int)UI_GameScene.GameObjects.Scope);
        zoomSVD = zoom.transform.GetChild(0).gameObject;
        zoomAWM = zoom.transform.GetChild(1).gameObject;
        zoom.SetActive(false); // Zoom ĵ���� off
        ZoomUI();
    }

    protected override void Update()
    {
        base.Update();
    }

    private void ZoomUI()
    {
        if (weaponData.weaponName == WeaponName.SVD)
        {
            zoomSVD.SetActive(true);
            zoomAWM.SetActive(false);
        }
        else if (weaponData.weaponName == WeaponName.AWM)
        {
            zoomAWM.SetActive(true);
            zoomSVD.SetActive(false);
        }
    }

    public override void StartWeaponAction()
    {
        if (_playerStatHandler.playerState == PlayerActionState.Reload)//isReload == true) // ������ ���� ���� ���� ���� �Ұ���
            return;

        //if (isModeChange == true) // ��� ��ȯ���̸� ���� �׼� �Ұ���
        //    return;

        if ((_playerStatHandler.playerState == PlayerActionState.Attack)) // ���콺 �� Ŭ��(���� ����)
        {
            if (weaponData.Stats.isAutomaticAttack == true) // ���� ����
            {
                StartCoroutine("OnAttackLoop");
            }
            else // �ܹ� ����
            {
                OnAttack();
            }
        }
        else if (_preLookType != Player.Instance.playerStatHandler.playerLookType) // ���콺 ��Ŭ��(��� ��ȯ)
        {
            _preLookType = Player.Instance.playerStatHandler.playerLookType;
            //if (isAttack == true) // ���� ���� ���� ��� ��ȯ �Ұ���
            //    return;

            StartCoroutine("OnModeChange");
        }
    }

    public override void StopWeaponAction()
    {
        if ((_playerStatHandler.playerState == PlayerActionState.Attack))
        {
            //isAttack = (_playerStatHandler.playerState == PlayerActionState.Attack);
            StopCoroutine("OnAttackLoop");
        }
    }

    public override void StartReload()
    {
        //if (_playerStatHandler.playerState == PlayerActionState.Reload) return;
        
        StopWeaponAction(); // ���� �׼� ���� RŰ�� ���� �������� �õ��ϸ� ���� �׼� ���� �� ������
        if (currentAmmo < ammoToReload)
        {
            StartCoroutine("OnReload");
        }
    }

    // ����
    private IEnumerator OnAttackLoop()
    {
        while ((_playerStatHandler.playerState == PlayerActionState.Attack))     // TODO true or ����
        {
            OnAttack();

            yield return null;
        }
    }

    // ����
    public void OnAttack()
    {
        if (Time.time - lastAttackTime > weaponData.Stats.attackRate)
        {
            if (animator.MoveSpeed > 0.5f)   // �ٰ� ���� �� ���� X
            {
                return;
            }

            lastAttackTime = Time.time;     // �����ֱⰡ �Ǿ�� ������ �� �ֵ��� �ϱ� ���� ���� �ð� ����

            if (currentAmmo <= 0) // ź�� 0 ->  ����X
            {
                return;
            }
            currentAmmo--; // ���� �� currentAmmo 1 ����

            string animation = animator.ZoomModeIs == true ? "Rifle_ZoomFire" : "Rifle_Fire"; // ���� �ִϸ��̼� ����(��忡 ���� AimFire or Fire ���)
            animator.Play(animation, -1, 0);

            if (animator.ZoomModeIs == false) StartCoroutine("OnMuzzleFlashEffect"); // �ѱ� ȿ�� ���(default mode �϶��� ���)
            PlaySound(audioClipFire); // ���� ���� ���
            Recoil(); // �ݵ�
            casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right); // ź�� ����
            TwoStepRaycast(); // ���� �߻��� ���ϴ� ��ġ ����

            ZombieControl[] zombies = FindObjectsOfType<ZombieControl>();
            foreach (ZombieControl zombie in zombies)
            {
                zombie.GunshotHeard();
            }
        }
    }

    // �ѱ� ȿ��
    private IEnumerator OnMuzzleFlashEffect()
    {
        muzzleFlashEffect.SetActive(true);
        yield return new WaitForSeconds(weaponData.Stats.attackRate * 0.1f);
        muzzleFlashEffect.SetActive(false);
    }

    private IEnumerator OnReload()
    {
        isReload = true;
        _playerStatHandler.SetState(PlayerActionState.Reload);

        if (animator.ZoomModeIs) // ������ -> �� ��� off, �� ī�޶� off, �� ĵ���� off, �� ���̱�
        {
            animator.ZoomModeIs = false;
            mainCamera.fieldOfView = defaultModeFOV;
            zoom.SetActive(false);
            mainCamera.cullingMask = mainCamera.cullingMask | (1 << 11);
        }
        animator.OnReload(); // ������ �ִϸ��̼�
        PlaySound(audioClipReload); // ������ ���� ���

        while (true)
        {
            // ���� ���X �ִϸ��̼� Movement O�̸�
            if (audioSource.isPlaying == false && animator.CurrentAnimationIs("Movement"))
            {
                isReload = false; // ������ �ִϸ��̼�, ���� ��� ����
                _playerStatHandler.SetState(PlayerActionState.Move);

                int maxAmmoInReload = AmmoManager.instance.GetSniperMaxAmmo();
                maxAmmoInReload = Mathf.Max(0, maxAmmoInReload);
                if (currentAmmo + maxAmmoInReload >= ammoToReload)
                {
                    int decreaseAmmo = ammoToReload - currentAmmo; // ���� ����
                    maxAmmoInReload -= decreaseAmmo; // ������ ź�� maxAmmo���� ��������
                    currentAmmo += decreaseAmmo; // ������ ź�� �� ��ŭ ä���
                }
                else
                {
                    currentAmmo += maxAmmoInReload;
                    maxAmmoInReload = 0;
                }

                AmmoManager.instance.SetSniperMaxAmmo(maxAmmoInReload);

                yield break;
            }
            yield return null;
        }
    }

    private void TwoStepRaycast()
    {
        Ray ray;
        RaycastHit hit;
        Vector3 targetPoint = Vector3.zero;

        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f); // ȭ�� �߾� ��ǥ(aim)

        if (Physics.Raycast(ray, out hit, weaponData.Stats.attackDistance)) // ���� ��Ÿ� �ȿ� �浹�ϴ� ������Ʈ ������ 
        {
            targetPoint = hit.point; // ������ �ε��� ��ġ ����
        }
        else // ���� ��Ÿ� �ȿ� �浹�ϴ� ������Ʈ�� ������ 
        {
            targetPoint = ray.origin + ray.direction * weaponData.Stats.attackDistance; // �ִ� ��Ÿ� ��ġ ����
        }
        Debug.DrawRay(ray.origin, ray.direction * weaponData.Stats.attackDistance, Color.red); // ���� ����


        Vector3 attackDirection = (targetPoint - bulletSpawnPoint.position).normalized; // ���� ����
        if (Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, weaponData.Stats.attackDistance)) // �浹�Ѵٸ�
        {
            impactMemoryPool.SpawnImpact(hit); // Ÿ�� impact ����

            Collider collider = hit.collider;
            ZombieDamage zombieDamage = collider.GetComponent<ZombieDamage>();

            Debug.Log(collider.tag);

            if (zombieDamage != null)
            {
                if (hit.collider.tag == "Zombie")
                {
                    zombieDamage.TakeDamage(weaponData.Stats.damage);
                    Debug.Log("���ε�----------------------------------");
                    Debug.Log(weaponData.Stats.damage);
                }
                if (hit.collider.tag == "ZombieHead")
                {
                    zombieDamage.TakeDamage((int)(weaponData.Stats.damage * 2.5f));
                    Debug.Log("�Ӹ���!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    Debug.Log(weaponData.Stats.damage * 2.5f);
                }
            }
        }
        Debug.DrawRay(bulletSpawnPoint.position, attackDirection * weaponData.Stats.attackDistance, Color.blue); // ���� ����
    }

    private IEnumerator OnModeChange()
    {
        if (animator.ZoomModeIs)
        {
            zoom.SetActive(false);
            mainCamera.cullingMask = mainCamera.cullingMask | (1 << 11);
        }
        else
        {
            zoom.SetActive(true);
            mainCamera.cullingMask = mainCamera.cullingMask & ~(1 << 11);
        }

        float current = 0;
        float percent = 0;
        float time = 0.35f;

        animator.ZoomModeIs = !animator.ZoomModeIs;

        float start = mainCamera.fieldOfView;
        float end = animator.ZoomModeIs == true ? zoomModeFOV : defaultModeFOV;

        _playerStatHandler.playerLookType = animator.ZoomModeIs == true ? PlayerLookType.Zoom : PlayerLookType.Idle;  // �ִϸ��̼� �𵨵��� �ܽ�����Ʈ����

        while (percent < 1)
        {
            current += Time.deltaTime;
            percent = current / time;

            mainCamera.fieldOfView = Mathf.Lerp(start, end, percent);

            yield return null;
        }
    }

    private void ResetVariables()
    {
        isReload = false;
        isAttack = false;
        isModeChange = false;
    }

}
