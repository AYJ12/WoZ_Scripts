using UnityEngine;

public class Impact : MonoBehaviour
{
    private ParticleSystem _particle;
    private MemoryPool memoryPool;

    private void Awake()
    {
        _particle = GetComponent<ParticleSystem>();
    }
    public void Setup(MemoryPool pool)
    {
        memoryPool = pool;
    }

    private void Update()
    {
        if (_particle.isPlaying == false) // ��ƼŬ ��� ��X -> ����
        {
            memoryPool.DeactivatePoolItem(gameObject);
        }
    }
}
