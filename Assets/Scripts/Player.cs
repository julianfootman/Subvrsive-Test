using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;

public class Player : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private RigBuilder _rigBuilder;
    [SerializeField] private Transform _weaponPos;
    [SerializeField] private TwoBoneIKConstraint _leftBoneIK;
    [SerializeField] private TwoBoneIKConstraint _rightBoneIK;
    [SerializeField] private State _currentState;
    [SerializeField] private Transform _firstPersonCameraFollow;
    [SerializeField] private Transform _thirdPersonCameraFollow;
    [Header("Visual")]
    [SerializeField] private Renderer _renderer;

    public Transform FirstPersonCameraFollow => _firstPersonCameraFollow;
    public Transform ThirdPersonCameraFollow => _thirdPersonCameraFollow;

    public enum State
    {
        Movement,
        TargetingToEnemy
    }

    private void Start()
    {
        for (int i = 0; i < 5; i ++)
        {
            UpdateColor(i);
        }
    }

    private void UpdateColor(int yPixel)
    {
        Texture2D originalTexture = (Texture2D)_renderer.material.mainTexture;
        Texture2D textureInstance = Instantiate(originalTexture);
        textureInstance.SetPixel(0, yPixel, Random.ColorHSV());
        textureInstance.Apply();
        _renderer.material.mainTexture = textureInstance;
    }

    public void EquipWeapon(Weapon weaponPrefab)
    {
        Weapon weapon = Instantiate(weaponPrefab, _weaponPos);
        _leftBoneIK.data.target = weapon.LeftRig.transform;
        _rightBoneIK.data.target = weapon.RightRig.transform;
        _rigBuilder.Build();
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
