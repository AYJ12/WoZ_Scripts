using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "New Player")]
public class PlayerData : ScriptableObject
{
    [Header("Stat")]
    public float hp;
    public float stamina;
    public float moveSpeed;
    public float maxHp;
    public float maxStamina;
    public bool isRunning;
    public bool isAttacking;
    public bool isZoom;
    public bool isSitdown;
    public bool isReloading;

    [Header("StartPoint")]
    public Transform startPoint;    // 맵 추가 되면 리스트로 만들자
}
