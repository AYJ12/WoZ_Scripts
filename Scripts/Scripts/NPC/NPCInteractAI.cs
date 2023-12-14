using UnityEngine;
using UnityEngine.AI;

public class NpcInteractAI : MonoBehaviour
{
    private Animator _animator;
    private GameObject _player;
    private Transform _playerPosition;
    private NavMeshAgent _navAgent;
    private float _distance;
    public bool isActive = false;

    void Start()
    {
        _animator = GetComponent<Animator>();
        _player = Player.Instance.gameObject;
        _playerPosition = _player.transform;
        _navAgent = GetComponent<NavMeshAgent>();
    }

    void Update()
    {
        _distance = (transform.position - _playerPosition.position).sqrMagnitude;
        ToPlayerMove(CheckActive());    // ��ȣ�ۿ�� ��ȭâ ��� or �ó׸ӽ� ����Ȱ��
    }

    private bool CheckActive()
    {
        return isActive;
    }

    private void ToPlayerMove(bool active)
    {
        _playerPosition = _playerPosition.transform;
        if (active && (_distance > 5.0f * 5.0f))
        {
            //anim ������ �÷���
            _animator.SetBool("isWalk", true);
            _navAgent.SetDestination(_playerPosition.position);
        }
        else
        {
            //anim idle �÷���
            _animator.SetBool("isWalk", false);
            _navAgent.velocity = Vector3.zero;
        }
    }


    //private void OnTriggerEnter(Collider other)
    //{
    //    if (other.gameObject.tag == "Player")
    //    {
    //        Debug.Log("Player Touch");
    //        isActive = true;
    //    }
    //}
}
