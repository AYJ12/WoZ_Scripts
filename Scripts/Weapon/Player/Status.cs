using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("Walk, Run Speed")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    // 외부에서 값 확인 용도
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;

}
