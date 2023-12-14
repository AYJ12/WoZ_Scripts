using UnityEngine;


[CreateAssetMenu(fileName = "ZombieData", menuName = "ZombieScriptable/CreateZombieData", order = int.MaxValue)]

public class ZombieData : ScriptableObject
{
    private ZombieControl zombieControl;

    [SerializeField]
    private int hp;
    public int HP { get { return hp; } }
    [SerializeField]
    private string monsterName;
    public string Name { get { return monsterName; } }

    [SerializeField]
    private float speed;
    public float Speed { get { return speed; } }

    [SerializeField]
    private float power;
    public float Power { get { return speed; } }

    public bool GetZombieControlIsAttacking()
    {
        if (zombieControl != null)
        {
            return zombieControl.isAttacking;
        }

        return false;
    }
}
