using System.Collections;
using UnityEngine;

[System.Serializable] public class AmmoEvent : UnityEngine.Events.UnityEvent<int, int> { }

public abstract class WeaponBase : MonoBehaviour
{
    [HideInInspector] public AmmoEvent onAmmoEvent = new AmmoEvent();

    [Header("Ammo")]
    public int ammoToReload;        // �ִ� ���� ������ ��
    public int currentAmmo;         // ���� ź�� ��

    [Header("WeaponBase")]
    public WeaponType weaponType;                            // ���� ����
    public WeaponData weaponData;

    [Header("Recoil")]
    Vector3 currentRotation, targetRotation, targetPosition, currentPosition, initialGunPosition;
    [SerializeField] float recoilX;
    [SerializeField] float recoilY;
    [SerializeField] float recoilZ;
    [SerializeField] float kickBackZ;
    public float snppiness, returnAmount;

    [Header("Audio Clips")]
    [SerializeField] protected AudioClip audioClipTakeOutWeapon;  // �������� ����

    protected float lastAttackTime = 0;                       // ������ �߻�ð� üũ��
    protected bool isReload = false;                          // ������ ������ üũ
    protected bool isAttack = false;                          // ���� ���� üũ��
    protected bool isModeChange = false;                      // ��� ��ȯ ���� üũ��
    protected AudioSource audioSource;                        // ���� ��� ������Ʈ
    protected PlayerAnimatorController animator;              // �ִϸ��̼� ��� ����

    protected PlayerStatHandler _playerStatHandler;
    protected AmmoManager _currentAmmo;

    protected Camera mainCamera;                              // ī�޶�
    protected float defaultModeFOV = 60;                      // �⺻��忡���� ī�޶� FOV

    public PlayerAnimatorController Animator => animator;
    public WeaponName WeaponName => weaponData.weaponName; // Get Property's

    protected void Setup()
    {
        audioSource = GetComponent<AudioSource>();
        animator = GetComponent<PlayerAnimatorController>();
        _playerStatHandler = Player.Instance.playerStatHandler;
        mainCamera = Camera.main;
    }

    protected virtual void Update()
    {
        //공격, 상태 enum으로 처리
        isAttack = (_playerStatHandler.playerState == PlayerActionState.Attack);
        isModeChange = (_playerStatHandler.playerLookType == PlayerLookType.Zoom);

        AmmoManager.instance.UpdateAmmo(weaponType, currentAmmo); // 현재 탄약 수 업데이트
        
        RecoilBack();
    }

    public abstract void StartWeaponAction();
    public abstract void StopWeaponAction();
    public abstract void StartReload();

    protected void PlaySound(AudioClip clip)
    {
        audioSource.Stop();         
        audioSource.clip = clip;    
        audioSource.Play();         
    }

    public void Recoil()
    {
        targetPosition -= new Vector3(0, 0, kickBackZ);
        targetRotation += new Vector3(recoilX, Random.Range(-recoilY, recoilY), Random.Range(-recoilZ, recoilZ));
    }

    private void RecoilBack()
    {
        targetRotation = Vector3.Lerp(targetRotation, Vector3.zero, Time.deltaTime * returnAmount);
        currentRotation = Vector3.Slerp(currentRotation, targetRotation, Time.fixedDeltaTime * snppiness);

        Quaternion targetQuaternion = Quaternion.Euler(currentRotation);
        transform.localRotation = Quaternion.Slerp(transform.localRotation, targetQuaternion, Time.deltaTime * snppiness);
        mainCamera.transform.localRotation = Quaternion.Slerp(mainCamera.transform.localRotation, targetQuaternion, Time.deltaTime * snppiness);

        back(); // kickback
    }

    void back()
    {
        targetPosition = Vector3.Lerp(targetPosition, initialGunPosition, Time.deltaTime * returnAmount);
        currentPosition = Vector3.Lerp(currentPosition, targetPosition, Time.fixedDeltaTime * snppiness);
        transform.localPosition = currentPosition;
    }
}
