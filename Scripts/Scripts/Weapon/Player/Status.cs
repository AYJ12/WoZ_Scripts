using UnityEngine;

public class Status : MonoBehaviour
{
    [Header("Walk, Run Speed")]
    [SerializeField] private float walkSpeed;
    [SerializeField] private float runSpeed;

    // �ܺο��� �� Ȯ�� �뵵
    public float WalkSpeed => walkSpeed;
    public float RunSpeed => runSpeed;

}
