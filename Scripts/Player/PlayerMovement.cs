using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private PlayerEventController _playerEvent;
    private PlayerStatHandler _playerStatHandler;
    private CharacterController _characterController;
    //private WeaponAssaultRifle _weapon;           // ���⸦ �̿��� ���� ����
    private WeaponBase _weapon;                      // ��� ���Ⱑ ��ӹ޴� ��� Ŭ����

    [SerializeField]
    private Transform _cameraTransform;
    public LayerMask groundLayerMask;
    private float _yVelocity = 0;
    private float _gravity = 10.0f;  //�߷�
    private Vector3 _moveDirection = Vector3.zero;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip _audioClipWalk;
    [SerializeField] private AudioClip _audioClipRun;

    private AudioSource _audioSource;


    private void Awake()
    {
        _playerEvent = Player.Instance.playerEvent;
        _playerStatHandler = Player.Instance.playerStatHandler;
        _characterController = GetComponent<CharacterController>();
        _weapon = GetComponentInChildren<WeaponAssaultRifle>();
        _audioSource = GetComponent<AudioSource>();
    }

    private void Start()
    {
        _playerEvent.OnMoveEvent += Move;
        _playerEvent.OnAttackEvent += Attack;
        _playerEvent.OnZoomEvent += Zoom;
        _playerEvent.OnReloadEvent += Reload;
    }

    private void Update()
    {
        if (!Inventory.inventoryActivated)
        {
            ApplyMove(_moveDirection);
            Jump();
            SitDown();

            if (!_characterController.isGrounded)
            {
                FallDamage();
            }
        }
    }


    private void Move(Vector3 direction)
    {
        _moveDirection = direction;
    }

    private void ApplyMove(Vector3 direction)
    {
        //Vector3 direction = new Vector3(x, 0, y);

        direction = _cameraTransform.TransformDirection(direction);
        if ((_playerStatHandler.moveType == MoveType.Run) && _playerStatHandler.currentStat.playerStamina > 0) // �޸���
        {
            direction = direction * _playerStatHandler.currentStat.playerMoveSpeed * 1.5f;

            _playerStatHandler.Sub(0.0f, 10.0f * Time.deltaTime);   //���¹̳�
            _weapon.Animator.MoveSpeed = 1f;

            _audioSource.clip = _audioClipRun;
            StartSoundLoop();
        }
        else
        {
            if (direction.magnitude > 0.01f) // �ȱ�
            {
                direction = direction * _playerStatHandler.currentStat.playerMoveSpeed;
                _weapon.Animator.MoveSpeed = 0.5f;

                _audioSource.clip = _audioClipWalk;
                StartSoundLoop();
            }
            else // Idle
            {
                _weapon.Animator.MoveSpeed = 0f;
                StopSoundLoop();
            }
        }

        _yVelocity -= _gravity * Time.deltaTime;
        direction.y = _yVelocity;

        _characterController.Move(direction * Time.deltaTime);       //rigid��� or character controller��� ����
        transform.rotation = Camera.main.transform.rotation;
    }

    private void Jump()
    {
        if (_characterController.isGrounded)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                _yVelocity = 4.0f; // ����

                Manager.Sound.Play("Jump", Define.Sound.Effect);
            }
        }
    }

    private void SitDown()
    {
        if (_playerStatHandler.moveType == MoveType.Sit)
        {
            _characterController.height = Mathf.Lerp(_characterController.height, 0.3f, Time.deltaTime * 10f);
        }
        else
        {
            _characterController.height = Mathf.Lerp(_characterController.height, 1f, Time.deltaTime * 10f);
        }
    }

    private void FallDamage()
    {
        float fallDistance = Mathf.Abs(_yVelocity) * Time.deltaTime;

        float fallDamageDistance= 7.0f;
        float fallDamage = 5.0f;

        if (fallDistance > fallDamageDistance)
        {
            int damageAmount = Mathf.RoundToInt((fallDistance - fallDamageDistance) * fallDamage);

            _playerStatHandler.Sub(damageAmount, 0.0f);
            Manager.Sound.Play("Damage_Fall", Define.Sound.Effect);
        }
    }

    //private bool IsGround()
    //{
    //    Ray[] rays = new Ray[4]
    //    {
    //        new Ray(transform.position + (transform.forward * 1f) + (Vector3.up * 0.01f) , Vector3.down),
    //        new Ray(transform.position + (-transform.forward * 1f) + (Vector3.up * 0.01f), Vector3.down),
    //        new Ray(transform.position + (transform.right * 1f) + (Vector3.up * 0.01f), Vector3.down),
    //        new Ray(transform.position + (-transform.right * 1f) + (Vector3.up * 0.01f), Vector3.down),
    //    };

    //    for (int i = 0; i < rays.Length; i++)
    //    {
    //        if (Physics.Raycast(rays[i], 0.1f, groundLayerMask))
    //        {
    //            return true;
    //        }
    //    }

    //    return false;
    //}

    private void Attack()
    {
        if (!Inventory.inventoryActivated)
        {
            if (_playerStatHandler.playerState == PlayerActionState.Attack)
            {
                _weapon.StartWeaponAction();
            }
            else
            {
                _weapon.StopWeaponAction();
            }
        }
    }
    private void Zoom()
    {
        _weapon.StartWeaponAction();
        _weapon.StopWeaponAction();
    }

    private void Reload()
    {
        if (_playerStatHandler.playerState == PlayerActionState.Reload)
        {
            _weapon.StartReload();
        }
    }

    public void SwitchingWeapon(WeaponBase newWeapon)
    {
        _weapon = newWeapon;
    }

    private void StartSoundLoop()
    {
        if (_audioSource.isPlaying == false)
        {
            _audioSource.loop = true;
            _audioSource.Play();
        }
    }

    private void StopSoundLoop()
    {
        if (_audioSource.isPlaying == true)
        {
            _audioSource.Stop();
        }
    }
}