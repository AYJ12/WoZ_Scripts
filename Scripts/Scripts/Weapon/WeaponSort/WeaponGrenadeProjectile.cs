using UnityEngine;

public class WeaponGrenadeProjectile : MonoBehaviour
{
    [Header("Explosion Barrel")]
    [SerializeField] private GameObject _explosionPrefab;

    [SerializeField] private float _explosionRadius = 10.0f; // ���� ����Ʈ
    [SerializeField] private float _explosionForce = 500.0f; // ���� ��
    [SerializeField] private float _throwForce = 100.0f; // ���ư��� ��

    private int _explosionDamage; // ���� ������
    private Rigidbody _rigidbody;
    private GameObject _soundManager;
    private AudioSource _audioSource;  // ���� ��� ������Ʈ
    
    private WeaponGrenade _weaponGrenade;

    [Header("Sound Name")]
    [SerializeField] private string Grenade_BombSound; // ����ź ������

    public void Setup(int damage, Vector3 rotation)
    {
        _rigidbody = GetComponent<Rigidbody>();
        _rigidbody.AddForce(rotation * _throwForce);
        _explosionDamage = damage;
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(_explosionPrefab, transform.position, transform.rotation); // ���� ����Ʈ ����

        Collider[] colliders = Physics.OverlapSphere(transform.position, _explosionRadius); // ���� ������ �ִ� ��� ������Ʈ�� Colider ������ �޾ƿ� ���� ȿ�� ó��

        foreach (Collider hit in colliders)
        {
            PlayerStatHandler player = hit.GetComponent<PlayerStatHandler>();
            if (player != null)
            {
                // TODO 
                player.Sub(15.0f, 0.0f); // ���� ������ �ε��� ������Ʈ�� �÷��̾��� �� ó��
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
        Destroy(gameObject); // ����ź ������Ʈ ����
    }
}
