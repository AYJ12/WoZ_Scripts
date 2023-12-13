using System.Collections;
using UnityEngine;

public class WeaponMeleeColider : MonoBehaviour
{
    [SerializeField] private ImpactMemoryPool _impactMemoryPool;
    [SerializeField] private Transform _meleeTransform;
    private WeaponMelee _weaponMelee;

    private Collider _collider;
    private int _damage;

    private void Awake()
    {
        _weaponMelee = GetComponent<WeaponMelee>();
        _collider = GetComponent<Collider>();
        _collider.enabled = false;
    }

    public void StartCollider(int damage)
    {
        this._damage = damage;
        _collider.enabled = true;

        StartCoroutine("DisablebyTime", 0.1f);
    }

    private IEnumerator DisablebyTime(float time)
    {
        yield return new WaitForSeconds(time);
        _collider.enabled = false;
    }

    private void OnTriggerEnter(Collider other)
    {
        _impactMemoryPool.SpawnImpact(other, _meleeTransform);

        ZombieDamage zombieDamage = other.GetComponent<ZombieDamage>();
        if (zombieDamage == null)
            return;
        ZombiePartType zombiePart = zombieDamage.GetZombiePartType();

        if (zombiePart == ZombiePartType.Body)
        {
            zombieDamage.TakeDamage(_damage);
        }
        else if (zombiePart == ZombiePartType.Head)
        {
            zombieDamage.TakeDamage((int)(_damage * 2.5f));
        }
    }
}
