using System.Collections;
using UnityEngine;

public class Casing : MonoBehaviour
{
    [SerializeField] private float _deactivateTime = 5.0f; // 탄피 생성 후 비활성화 되는 시간
    [SerializeField] private float _casingSpin = 1.0f; // 탄피 회전 속력 개수
    [SerializeField] private AudioClip[] _audioClips; // 탄피가 충돌 시 재생 사운드

    private Rigidbody _rigidbody3D;
    private AudioSource _audioSource;
    private MemoryPool _memoryPool;

    public void Setup(MemoryPool pool, Vector3 direction)
    {
        _rigidbody3D = GetComponent<Rigidbody>();
        _audioSource = GetComponent<AudioSource>();
        _memoryPool = pool;

        // 탄피의 이동 속력, 회전 속력 설정
        _rigidbody3D.velocity = new Vector3(direction.x, 1.0f, direction.z);
        _rigidbody3D.angularVelocity = new Vector3(Random.Range(-_casingSpin, _casingSpin), Random.Range(-_casingSpin, _casingSpin), Random.Range(-_casingSpin, _casingSpin));

        // 탄피 자동 비활성화를 위한 코루틴 실행
        StartCoroutine("DeactivateAfterTime");
    }

    private void OnCollisionEnter(Collision collision)
    {
        int index = Random.Range(0, _audioClips.Length); // 사운드 랜덤
        _audioSource.clip = _audioClips[index];
        _audioSource.Play();
    }
    private IEnumerator DeactivateAfterTime()
    {
        yield return new WaitForSeconds(_deactivateTime);
        _memoryPool.DeactivatePoolItem(this.gameObject); // 탄피 오브젝트 비활성화
    }
}
