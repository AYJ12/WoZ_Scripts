using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GrenadeProjectile : MonoBehaviour
{
    [Header("Explosion Prefab")]
    [SerializeField] private GameObject explosionEffectPrefab;
    [SerializeField] private GameObject audioSourcePrefab;

    [Header("Explosion Settings")]
    [SerializeField] private float explosionDelay = 2f;
    [SerializeField] private float explosionForce = 500f;
    [SerializeField] private float explosionRadius = 5f;
    private int _explosionDamage; // 폭발 데미지

    [Header("Audio Effects")]
    [SerializeField] private AudioClip explosionSound;
    [SerializeField] private AudioClip impactSound;

    private AudioSource audioSource;

    private float countdown;
    private bool hasExploded = false;
    
    private void Start()
    {
        countdown = explosionDelay;
        audioSource = GetComponent<AudioSource>();
    }
    private void Update()
    {
        if (!hasExploded)
        {
            countdown -= Time.deltaTime;
            if(countdown <-0f)
            {
                Explode();
                hasExploded = true;
            }
        }
    }

    public void Setup(int damage)
    {
        _explosionDamage = damage;
    }

    void Explode()
    {
        //Destroy(gameObject);
    }

    void PlaySoundAtPosition(AudioClip clip)
    {
        GameObject audioSourceObject = Instantiate(audioSourcePrefab, transform.position, Quaternion.identity);
        AudioSource instantiateAudioSource = audioSourceObject.GetComponent<AudioSource>();
        instantiateAudioSource.clip = clip;
        instantiateAudioSource.spatialBlend = 1;
        instantiateAudioSource.Play();

        Destroy(audioSourceObject, instantiateAudioSource.clip.length);
    }

    private void OnCollisionEnter(Collision collision)
    {
        Instantiate(explosionEffectPrefab, transform.position, transform.rotation);
        PlaySoundAtPosition(explosionSound);

        Collider[] colliders = Physics.OverlapSphere(transform.position, explosionRadius);

        foreach(Collider hit in colliders)
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
                    Debug.Log("Damage");
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
                rigidbody.AddExplosionForce(explosionForce, transform.position, explosionRadius);
            }
        }
        Destroy(gameObject);
    }
}
