using UnityEngine;

public abstract class CharacterStat
{
    protected float hp = 100;   //����
    [Range(0.5f, 10f)]
    protected float moveSpeed;  //����
}
