using UnityEngine;

public class WeaponGrenadeProjectile : MonoBehaviour
{
    [Header("Explosion Barrel")]
    [SerializeField] private GameObject _explosionPrefab;

    [SerializeField] private float _explosionRadius = 10.0f; // 폭발 이펙트
    [SerializeField] private float _explosionForce = 500.0f; // 폭발 힘
    [SerializeField] private float _throwForce = 100.0f; // 날아가는 힘

    private int _explosionDamage; // 폭발 데미지
    private Rigidbody _rigidbody;
    private GameObject _soundManager;
    private AudioSource _audioSource;  // 사운드 재생 컴포넌트
    
    private WeaponGrenade _weaponGrenade;

    [Header("Sound Name")]
    [SerializeField] private string Grenade_BombSound; // 수류탄 폭발음

    public void Setup(int damage, Vector3 rotation)
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(rotation * _throwForce);
        _explosionDamage = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(_explosionPrefab, transform.position, transform.rotation); // 폭발 이펙트 생성

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius); // 폭발 범위에 있는 모든 오브젝트의 Colider 정보를 받아와 폭발 효과 처리

        foreach (Collider hit in colliders)
        {
            PlayerStatHandler player = hit.GetComponent<PlayerStatHandler>();
            if (player != null)
            {
                // TODO 
                player.Sub(15.0f, 0.0f); // 폭발 범위에 부딪힌 오브젝트가 플레이어일 때 처리
                continue;
            }

            ZombieDamage zombieDamage = hit.GetComponent<ZombieDamage>();
            if (zombieDamage != null)
            {
                ZombiePartType zombiePart = zombieDamage.GetZombiePartType();
                if (zombiePart == ZombiePartType.Body)
                {
                    zombieDamage.TakeDamage(_explosionDamage);//
                }
                else if (zombiePart == ZombiePartType.Head)
                {
                    zombieDamage.TakeDamage((int)(_explosionDamage * 2.5f));
                }
                continue;
            }

            Rigidbody rigidbody = hit.GetComponent<Rigidbody>();
            if (rigidbody != null)
            {
                rigidbody.AddExplosionForce(_explosionForce, transform.position, _explosionRadius);
            }
        }
        //SoundManager.instance.PlaySE(Grenade_BombSound);
        Destroy(gameObject); // 수류탄 오브젝트 삭제
    }
}
