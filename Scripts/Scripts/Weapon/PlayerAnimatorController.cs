using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>(); // ���� ������Ʈ
    }

    public float MoveSpeed
    {
        set => _animator.SetFloat("movementSpeed", value);
        get => _animator.GetFloat("movementSpeed");
    }

    public void Play(string stateName, int layer, float normalizedTime)
    {
        _animator.Play(stateName, layer, normalizedTime);
    }

    public void OnReload()
    {
        _animator.SetTrigger("OnReload");
    }
    public bool ZoomModeIs
    {
        set => _animator.SetBool("isZoomMode", value);
        get => _animator.GetBool("isZoomMode");
    }
    public bool CurrentAnimationIs(string name) // ���� ��� ������ Ȯ�� & ��� ��ȯ
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
    public void SetFloat(string paramName, float value)
    {
        _animator.SetFloat(paramName, value);
    }
}
