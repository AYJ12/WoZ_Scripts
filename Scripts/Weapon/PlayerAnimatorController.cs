using UnityEngine;

public class PlayerAnimatorController : MonoBehaviour
{
    private Animator _animator;

    private void Awake()
    {
        _animator = GetComponentInChildren<Animator>(); // 하위 오브젝트
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
    public bool CurrentAnimationIs(string name) // 현재 재생 중인지 확인 & 결과 반환
    {
        return _animator.GetCurrentAnimatorStateInfo(0).IsName(name);
    }
    public void SetFloat(string paramName, float value)
    {
        _animator.SetFloat(paramName, value);
    }
}
