using UnityEngine;
using UnityEngine.AI;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private Transform _weaponPos;

    [SerializeField] private State _currentState;
    public enum State
    {
        Movement,
        TargetingToEnemy
    }

    public void Shot()
    {
        _animator.SetTrigger("Shot");
    }

    private void Update()
    {
        if (!_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            SetRandomDestination();
        }

        _animator.SetFloat("Move", _navMeshAgent.velocity.magnitude);
    }

    private void SetRandomDestination()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * 10;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, 30, NavMesh.AllAreas))
        {
            _navMeshAgent.SetDestination(hit.position);
        }
    }
}
