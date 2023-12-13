using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class MovementCharacterController : MonoBehaviour
{
    [SerializeField] private float moveSpeed;   // �̵� �ӵ�
    private Vector3 moveForce;                  // �̵� ��(x,z�� y���� ������ ����� ���� �̵��� ����)

    [SerializeField] private float jumpForce; // ���� ��
    [SerializeField] private float gravity; // �߷� ���

    public float MoveSpeed
    {
        set => moveSpeed = Mathf.Max(0, value); // �̵� �ӵ� ���� ���� X
        get => moveSpeed;
    }

    private CharacterController characterController;    // �÷��̾� �̵� ��� ���� ������Ʈ

    private void Awake()
    {
        characterController = GetComponent<CharacterController>();
    }

    private void Update()
    {
        // ����� �������� �߷¸�ŭ y�� �̵��ӵ� ����
        if (!characterController.isGrounded)
        {
            moveForce.y += gravity * Time.deltaTime;
        }

        // 1�ʴ� moveForce �ӷ����� �̵�
        characterController.Move(moveForce * Time.deltaTime);
    }

    public void MoveTo(Vector3 direction)
    {
        // �̵� ���� = ĳ������ ȸ�� �� * ���� ��
        direction = transform.rotation * new Vector3(direction.x, 0, direction.z);

        // �̵� �� = �̵����� * �ӵ�
        moveForce = new Vector3(direction.x * moveSpeed, moveForce.y, direction.z * moveSpeed);
    }

    public void Jump()
    {
        // �÷��̾ �ٴڿ� ���� ���� ���� ����
        if (characterController.isGrounded)
        {
            moveForce.y = jumpForce;
        }
    }
}

