using UnityEngine;

public class Enemy : MonoBehaviour
{
    public int MaxHp = 100;
    public int currentHp;

    private void Awake()
    {
        currentHp = MaxHp;
    }
    public void TakeDamage(int damage)
    {
        currentHp -= damage;
        if (currentHp < 0)
        {
            currentHp = 0;
            Debug.Log("Die");
        }

    }
}
