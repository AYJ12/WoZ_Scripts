using UnityEngine;
public enum ZombiePartType
{
    Body,
    Head
}
public class ZombieDamage : MonoBehaviour
{
    private ZombieControl zombieController;
    [SerializeField] private ZombiePartType zombiePartType;

    // Start is called before the first frame update
    void Start()
    {
        zombieController = GetComponentInParent<ZombieControl>();
    }

    public void TakeDamage(int damage)
    {
        if (zombieController != null)
        {
            zombieController.TakeDamage(damage, zombiePartType);
        }
    }
    public ZombiePartType GetZombiePartType()
    {
        return zombiePartType;
    }
}
