using System.Collections;
using UnityEngine;


public class WeaponSniper : WeaponBase
{
    [Header("Spawn Points")]
    [SerializeField] private Transform casingSpawnPoint;    // 탄피 생성 위치
    [SerializeField] Transform bulletSpawnPoint;            // 총알 생성 위치

    [Header("Fire Effects")]
    [SerializeField] private GameObject muzzleFlashEffect;  // 총구 이벤트(On/ Off)

    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipFire;       // 공격 사운드
    [SerializeField] private AudioClip audioClipReload; // 재장전 사운드

    [Header("Zoom")]
    [SerializeField] private GameObject zoom;   // 줌 화면
    private GameObject zoomSVD;
    private GameObject zoomAWM;

    public PlayerLookType _preLookType;
    private float zoomModeFOV = 5;                          // AIM모드에서의 카메라 FOV

    private CasingMemoryPool casingMemoryPool;  // 탄피 생성 후 활성여부 관리
    private ImpactMemoryPool impactMemoryPool;  // 공격 효과 생성 후 활성 여부 관리

    private void Awake()
    {
        base.Setup(); // 기반 클래스의 초기화를 위한 Setup() 호출

        casingMemoryPool = GetComponent<CasingMemoryPool>();
        impactMemoryPool = GetComponent<ImpactMemoryPool>();
    }

    private void OnEnable()
    {
        mainCamera.fieldOfView = defaultModeFOV; // 줌 모드 카메라 off
        mainCamera.cullingMask = mainCamera.cullingMask | (1 << 11); // 총 보이기

        PlaySound(audioClipTakeOutWeapon);  // 무기 장착 사운드
        muzzleFlashEffect.SetActive(false); // 총구 효과 오브젝트 비활성화

        _preLookType = PlayerLookType.Idle;

        ResetVariables();
    }
    private void Start()
    {
        zoom = Manager.UI.SceneUI.GetObject((int)UI_GameScene.GameObjects.Scope);
        zoomSVD = zoom.transform.GetChild(0).gameObject;
        zoomAWM = zoom.transform.GetChild(1).gameObject;
        zoom.SetActive(false); // Zoom 캔버스 off
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
        if (_playerStatHandler.playerState == PlayerActionState.Reload)//isReload == true) // 재장전 중일 때는 무기 동작 불가능
            return;

        //if (isModeChange == true) // 모드 전환중이면 무기 액션 불가능
        //    return;

        if ((_playerStatHandler.playerState == PlayerActionState.Attack)) // 마우스 좌 클릭(공격 시작)
        {
            if (weaponData.Stats.isAutomaticAttack == true) // 연속 공격
            {
                StartCoroutine("OnAttackLoop");
            }
            else // 단발 공격
            {
                OnAttack();
            }
        }
        else if (_preLookType != Player.Instance.playerStatHandler.playerLookType) // 마우스 우클릭(모드 전환)
        {
            _preLookType = Player.Instance.playerStatHandler.playerLookType;
            //if (isAttack == true) // 공격 중일 때는 모드 전환 불가능
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
        
        StopWeaponAction(); // 무기 액션 도중 R키를 눌러 재장전을 시도하면 무기 액션 종료 후 재장전
        if (currentAmmo < ammoToReload)
        {
            StartCoroutine("OnReload");
        }
    }

    // 연사
    private IEnumerator OnAttackLoop()
    {
        while ((_playerStatHandler.playerState == PlayerActionState.Attack))     // TODO true or 조건
        {
            OnAttack();

            yield return null;
        }
    }

    // 공격
    public void OnAttack()
    {
        if (Time.time - lastAttackTime > weaponData.Stats.attackRate)
        {
            if (animator.MoveSpeed > 0.5f)   // 뛰고 있을 때 공격 X
            {
                return;
            }

            lastAttackTime = Time.time;     // 공격주기가 되어야 공격할 수 있도록 하기 위해 현재 시간 저장

            if (currentAmmo <= 0) // 탄수 0 ->  공격X
            {
                return;
            }
            currentAmmo--; // 공격 후 currentAmmo 1 감소

            string animation = animator.ZoomModeIs == true ? "Rifle_ZoomFire" : "Rifle_Fire"; // 무기 애니메이션 저장(모드에 따라 AimFire or Fire 재생)
            animator.Play(animation, -1, 0);

            if (animator.ZoomModeIs == false) StartCoroutine("OnMuzzleFlashEffect"); // 총구 효과 재생(default mode 일때만 재생)
            PlaySound(audioClipFire); // 공격 사운드 재생
            Recoil(); // 반동
            casingMemoryPool.SpawnCasing(casingSpawnPoint.position, transform.right); // 탄피 생성
            TwoStepRaycast(); // 광성 발사해 원하는 위치 공격

            ZombieControl[] zombies = FindObjectsOfType<ZombieControl>();
            foreach (ZombieControl zombie in zombies)
            {
                zombie.GunshotHeard();
            }
        }
    }

    // 총구 효과
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

        if (animator.ZoomModeIs) // 재장전 -> 줌 모드 off, 줌 카메라 off, 줌 캔버스 off, 총 보이기
        {
            animator.ZoomModeIs = false;
            mainCamera.fieldOfView = defaultModeFOV;
            zoom.SetActive(false);
            mainCamera.cullingMask = mainCamera.cullingMask | (1 << 11);
        }
        animator.OnReload(); // 재장전 애니메이션
        PlaySound(audioClipReload); // 재장전 사운드 재생

        while (true)
        {
            // 사운드 재생X 애니메이션 Movement O이면
            if (audioSource.isPlaying == false && animator.CurrentAnimationIs("Movement"))
            {
                isReload = false; // 재장전 애니메이션, 사운드 재생 종료
                _playerStatHandler.SetState(PlayerActionState.Move);

                int maxAmmoInReload = AmmoManager.instance.GetSniperMaxAmmo();
                maxAmmoInReload = Mathf.Max(0, maxAmmoInReload);
                if (currentAmmo + maxAmmoInReload >= ammoToReload)
                {
                    int decreaseAmmo = ammoToReload - currentAmmo; // 탼약 감소
                    maxAmmoInReload -= decreaseAmmo; // 감소한 탄약 maxAmmo에서 가져오기
                    currentAmmo += decreaseAmmo; // 감소한 탄약 수 만큼 채우기
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

        ray = mainCamera.ViewportPointToRay(Vector2.one * 0.5f); // 화면 중아 좌표(aim)

        if (Physics.Raycast(ray, out hit, weaponData.Stats.attackDistance)) // 공격 사거리 안에 충돌하는 오브젝트 있으면 
        {
            targetPoint = hit.point; // 광선에 부딪힌 위치 저장
        }
        else // 공격 사거리 안에 충돌하는 오브젝트가 없으면 
        {
            targetPoint = ray.origin + ray.direction * weaponData.Stats.attackDistance; // 최대 사거리 위치 저장
        }
        Debug.DrawRay(ray.origin, ray.direction * weaponData.Stats.attackDistance, Color.red); // 광선 정보


        Vector3 attackDirection = (targetPoint - bulletSpawnPoint.position).normalized; // 공격 방향
        if (Physics.Raycast(bulletSpawnPoint.position, attackDirection, out hit, weaponData.Stats.attackDistance)) // 충돌한다면
        {
            impactMemoryPool.SpawnImpact(hit); // 타격 impact 생성

            Collider collider = hit.collider;
            ZombieDamage zombieDamage = collider.GetComponent<ZombieDamage>();

            Debug.Log(collider.tag);

            if (zombieDamage != null)
            {
                if (hit.collider.tag == "Zombie")
                {
                    zombieDamage.TakeDamage(weaponData.Stats.damage);
                    Debug.Log("몸인뎅----------------------------------");
                    Debug.Log(weaponData.Stats.damage);
                }
                if (hit.collider.tag == "ZombieHead")
                {
                    zombieDamage.TakeDamage((int)(weaponData.Stats.damage * 2.5f));
                    Debug.Log("머리당!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!!");
                    Debug.Log(weaponData.Stats.damage * 2.5f);
                }
            }
        }
        Debug.DrawRay(bulletSpawnPoint.position, attackDirection * weaponData.Stats.attackDistance, Color.blue); // 광선 정보
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

        _playerStatHandler.playerLookType = animator.ZoomModeIs == true ? PlayerLookType.Zoom : PlayerLookType.Idle;  // 애니메이션 모델따라 줌스테이트변경

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
