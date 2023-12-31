using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [Header("Input KeyCodes")]
    [SerializeField] private KeyCode keyCodeRun = KeyCode.LeftShift; // �޸��� Ű
    [SerializeField] private KeyCode keyCodeJump = KeyCode.Space; // ���� Ű
    [SerializeField] private KeyCode keyCodeReload = KeyCode.R; // ź ������ Ű

    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipWalk; // �ȱ� ����
    [SerializeField] private AudioClip audioClipRun; // �޸��� ����

    private RotateToMouse rotateToMouse;            // ���콺 �̵����� ī�޶� ȸ��
    private MovementCharacterController movement;   // Ű���� �Է����� �÷��̾� �̵�, ����
    private Status status;                          // �̵��ӵ� ���� �÷��̾� ����
    private PlayerAnimatorController animator;      // �ִϸ��̼� ��� ���� 
    private AudioSource audioSource;                // ���� ��� ���� 
    private WeaponAssaultRifle weapon;              // ���⸦ �̿��� ���� ����


    private void Awake()
    {
        Cursor.visible = false; // Ŀ�� �����
        Cursor.lockState = CursorLockMode.Locked; // ���� ��ġ�� ����

        rotateToMouse = GetComponent<RotateToMouse>();
        movement = GetComponent<MovementCharacterController>();
        status = GetComponent<Status>();
        animator = GetComponent<PlayerAnimatorController>();
        audioSource = GetComponent<AudioSource>();
        weapon = GetComponentInChildren<WeaponAssaultRifle>();
    }


    private void Update()
    {
        UpdateRotate();
        UpdateMove();
        UpdateJump();
        UpdateWeaponAction();
    }

    private void UpdateRotate()
    {
        float mouseX = Input.GetAxis("Mouse X");
        float mouseY = Input.GetAxis("Mouse Y");

        rotateToMouse.UpdateRotate(mouseX, mouseY);
    }

    // �̵��� �� ��(�ȱ� or �ٱ�)
    private void UpdateMove()
    {
        float x = Input.GetAxisRaw("Horizontal");
        float z = Input.GetAxisRaw("Vertical");

        if (x != 0 || z != 0)
        {
            bool isRun = false;

            // ���̳� �ڷ� �̵��� ���� �޸� �� ����.
            if (z > 0) isRun = Input.GetKey(keyCodeRun);

            movement.MoveSpeed = isRun == true ? status.RunSpeed : status.WalkSpeed;
            animator.MoveSpeed = isRun == true ? 1 : 0.5f;
            audioSource.clip = isRun == true ? audioClipRun : audioClipWalk;

            if (audioSource.isPlaying == false) // ��� ���� ���� �ٽ� ������� �ʵ��� isPlaying ���� üũ�ؼ� ���
            {
                audioSource.loop = true;
                audioSource.Play();
            }
        }
        else // ���ڸ��� �������� ��
        {
            movement.MoveSpeed = 0;
            animator.MoveSpeed = 0; //Idle

            if (audioSource.isPlaying == true) // ������ �� ���尡 ��� ���̸� ����
            {
                audioSource.Stop();
            }
        }

        movement.MoveTo(new Vector3(x, 0, z));
    }

    private void UpdateJump()
    {
        if (Input.GetKeyDown(keyCodeJump))
        {
            movement.Jump();
        }
    }
    private void UpdateWeaponAction()
    {
        if (Input.GetMouseButtonDown(0))
        {
            weapon.StartWeaponAction();
        }
        else if (Input.GetMouseButtonUp(0))
        {
            weapon.StopWeaponAction();
        }
        if (Input.GetKeyDown(keyCodeReload))
        {
            weapon.StartReload();
        }
    }
}
