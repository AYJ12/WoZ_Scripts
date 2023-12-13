using System.Collections;
using UnityEngine;

public class WeaponGrenade : WeaponBase
{
    [Header("Audio Clips")]
    [SerializeField] private AudioClip AudioClipFire;     // ���� ���� 

    [Header("Grenade Prefab")]
    [SerializeField] private GameObject grenadePrefab; // ����ź ������
    
    [Header("Grenade Setting")]
    [SerializeField] private KeyCode throwKey = KeyCode.Mouse0;
    [SerializeField] private Transform grenadeSpawnPoint; // ����ź ������ġ
    [SerializeField] private Vector3 throwDirection = new Vector3(0, 1, 0);

    [Header("Grenade Force")]
    [SerializeField] private float throwForce = 5f;
    [SerializeField] private float maxForce = 10f;

    [Header("Trajectory Settings")]
    [SerializeField] private LineRenderer trajectoryLine;

    private bool isCharging = false;
    private float chargeTime = 0f;

    private void Awake()
    {
        base.Setup();
    }
    private void OnEnable()
    {
        mainCamera.fieldOfView = defaultModeFOV; 

        PlaySound(audioClipTakeOutWeapon);  
    }

    protected override void Update()
    {
        base.Update();
       
        ThrowControl();
        CheckGrenadeVisibility();
    }

    private void ThrowControl()
    {
        if (Input.GetKeyDown(throwKey))
        {
            StartThrowing();
        }
        if (isCharging) 
        {
            ChargeThrow();
        }
        if (Input.GetKeyUp(throwKey))
        {
            if ( currentAmmo > 0)
            {
                StartCoroutine("OnAttack");
            }
        }
    }

    void StartThrowing()
    {
        isCharging = true;
        chargeTime = 0f;

        trajectoryLine.enabled = true;
    }

    void ChargeThrow()
    {
        chargeTime += Time.deltaTime;

        Vector3 grenadeVelocity = (mainCamera.transform.forward + throwDirection).normalized * Mathf.Min(chargeTime * throwForce, maxForce);
        ShowTrajectory(grenadeSpawnPoint.position + grenadeSpawnPoint.forward, grenadeVelocity);
    }
   
    
    public void SpawnGrenadeProjectile(float force)
    {
        //GameObject grenadeClone = Instantiate(grenadePrefab, grenadeSpawnPoint.position, Random.rotation);
        // grenadeClone.GetComponent<WeaponGrenadeProjectile>().Setup(weaponData.Stats.damage, transform.parent.forward);
        ReleaseThrow();

        currentAmmo--; // ����ź ����
    }
    void ReleaseThrow()
    {
        ThrowGrenade(Mathf.Min(chargeTime * throwForce, maxForce));
        isCharging = false;

        trajectoryLine.enabled = false;
    }

    void ThrowGrenade(float force)
    {
        Vector3 spawanPosition = grenadeSpawnPoint.position + mainCamera.transform.forward;
        GameObject greade = Instantiate(grenadePrefab, spawanPosition, mainCamera.transform.rotation);
        greade.GetComponent<GrenadeProjectile>().Setup(weaponData.Stats.damage);
        Rigidbody rigidbody = greade.GetComponent<Rigidbody>();
        Vector3 finalThrowDirection = (mainCamera.transform.forward + throwDirection).normalized;
        rigidbody.AddForce(finalThrowDirection * force, ForceMode.VelocityChange);
    }

    void ShowTrajectory(Vector3 origin, Vector3 speed)
    {
        Vector3[] points = new Vector3[100];
        trajectoryLine.positionCount = points.Length;
        for(int i = 0; i < points.Length; i++)
        {
            float time = i * 0.1f;
            points[i] = origin + speed * time + 0.5f * Physics.gravity * time * time;
        }
        trajectoryLine.SetPositions(points);
    }

    private IEnumerator OnAttack()
    {
        isAttack = true;
        _playerStatHandler.SetState(PlayerActionState.Attack);

        animator.Play("Grenade_Fire", -1, 0); // ���� �ִϸ��̼� ���
        PlaySound(AudioClipFire);

        yield return new WaitForEndOfFrame();

        while (true)
        {
            if (animator.CurrentAnimationIs("Movement"))
            {
                isAttack = false;
                _playerStatHandler.SetState(PlayerActionState.Move);
                yield break;
            }
            yield return null;
        }
    }
    private void CheckGrenadeVisibility()
    {
        if (currentAmmo == 0)
        {
            mainCamera.cullingMask = mainCamera.cullingMask & ~(1 << 13); // ����ź �� ����
        }
        else
        {
            mainCamera.cullingMask = mainCamera.cullingMask | (1 << 13); // ��Ʃź ����
        }
    }
    
    public override void StartWeaponAction()
    {
        //if ((_playerStatHandler.playerState == PlayerActionState.Attack) && isAttack == false && currentAmmo > 0)
        //{
        //    StartCoroutine("OnAttack");
        //}
    }
    public override void StopWeaponAction()
    {
    }
    public override void StartReload()
    {
    }
  
}
