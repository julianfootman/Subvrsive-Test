using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _weaponPos;

    public void Shot()
    {
        _animator.SetTrigger("Shot");
    }
}
