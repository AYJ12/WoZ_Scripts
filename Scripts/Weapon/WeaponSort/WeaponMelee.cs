using System.Collections;
using UnityEngine;

public class WeaponMelee : WeaponBase
{
    private PlayerEventController _playerEvent;
    [SerializeField] private WeaponMeleeColider weaponMeleeColider;

    [Header("Audio Clips")]
    [SerializeField] private AudioClip audioClipFire1;       // ���� ����
    [SerializeField] private AudioClip audioClipFire2;       // ���� ����

    private void Awake()
    {
        base.Setup();
        _playerStatHandler = Player.Instance.playerStatHandler;
    }
    private void OnEnable()
    {
        mainCamera.fieldOfView = defaultModeFOV; // ���� ���� �� �� ��� ī�޶� off
        PlaySound(audioClipTakeOutWeapon);  // ���� ���� ����

        isAttack = false;
        _playerStatHandler.SetState(PlayerActionState.Move);
    }

    protected override void Update()
    {
        base.Update();
    }

    public override void StartWeaponAction()
    {
        //if (_playerStatHandler.playerState == PlayerActionState.Attack)
        //    return;
        if (_playerStatHandler != null && _playerStatHandler.currentStat.playerStamina > 0)
        {
            float staminaCost = 15.0f;
            _playerStatHandler.Sub(0.0f, staminaCost);
            StartCoroutine("OnAttack", (_playerStatHandler.playerState == PlayerActionState.Attack));
        }
    }

    public override void StopWeaponAction()
    {
        //isAttack = false;
        _playerStatHandler.SetState(PlayerActionState.Move);
        StopCoroutine("OnAttackLoop");
    }

    public override void StartReload()
    {

    }

    private IEnumerator OnAttackLoop()
    {
        while (true)
        {
            yield return StartCoroutine("OnAttack", (_playerStatHandler.playerState == PlayerActionState.Attack));
        }
    }

    private IEnumerator OnAttack()
    {
        isAttack = true;

        animator.SetFloat("attackType", (_playerStatHandler.playerState == PlayerActionState.Attack) ? 0.0f : 1.0f); // ���� ��� ����(0,1)
        animator.Play("Fire", -1, 0); // ���� �ִϸ��̼� ���

        AudioClip currentAudioClip = (_playerStatHandler.playerState == PlayerActionState.Attack) ? audioClipFire1 : audioClipFire2; // ���� ���� ��ǿ� ���� ������ ���带 ����
        PlaySound(currentAudioClip);

        yield return new WaitForEndOfFrame();

        while (true)
        {
            if (animator.CurrentAnimationIs("Movement"))
            {
                //isAttack = false;
                _playerStatHandler.SetState(PlayerActionState.Move);
                yield break;
            }
            yield return null;
        }
    }

    public void StartWeaponMeleeCollider()
    {
        weaponMeleeColider.StartCollider(weaponData.Stats.damage);
    }
}
