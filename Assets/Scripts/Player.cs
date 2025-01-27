using UnityEngine;
using UnityEngine.AI;
using UnityEngine.Animations.Rigging;
using UnityEngine.UI;

public class Player : MonoBehaviour
{
    [Header("References")]
    [SerializeField] private Animator _animator;
    [SerializeField] private NavMeshAgent _navMeshAgent;
    [SerializeField] private RigBuilder _rigBuilder;
    [SerializeField] private Transform _weaponPos;
    [SerializeField] private TwoBoneIKConstraint _leftBoneIK;
    [SerializeField] private TwoBoneIKConstraint _rightBoneIK;
    [SerializeField] private Transform _firstPersonCameraFollow;
    [SerializeField] private Transform _thirdPersonCameraFollow;
    [SerializeField] private Renderer _renderer;
    [SerializeField] private GameObject _boneParent;
    [Header("Enemy Detect")]
    [SerializeField] private float _sightDistance = 20f;
    [SerializeField] private float _moveDetectDistance = 30f;
    [SerializeField] private float _maxDistance = 100f;
    [SerializeField] private float _sightAngle = 60f;
    [Header("UI")]
    [SerializeField] private Slider _healthUI;

    private float _currentHealth;
    private Weapon _equippedWeapon;
    private Transform _target;
    private bool _isDead;

    private const float _maxHealth = 100f;

    public Transform FirstPersonCameraFollow => _firstPersonCameraFollow;
    public Transform ThirdPersonCameraFollow => _thirdPersonCameraFollow;
    public bool IsDead => _isDead;

    private void Start()
    {
        _healthUI.maxValue = _maxHealth;
        _healthUI.minValue = 0;
        _currentHealth = _maxHealth;

        for (int i = 0; i < 5; i++)
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
        _equippedWeapon = weapon;
    }

    public void Shot()
    {
        _animator.SetTrigger("Shot");
    }

    public void TakeDamage(float damage)
    {
        _currentHealth = Mathf.Clamp(_currentHealth - damage, 0, _maxHealth);
        if (_currentHealth == 0)
        {
            EnterDeathState();
        }
    }

    private void EnterDeathState()
    {
        _isDead = true;
        _animator.SetBool("Dead", true);
        _rigBuilder.enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        _healthUI.gameObject.SetActive(false);
        _equippedWeapon.gameObject.SetActive(false);
    }

    public void LaunchProjectile()
    {
        if (_isDead == false && _target != null)
        {
            _equippedWeapon.LaunchProjectile(_target.transform.position - transform.position);
        }
    }

    private void Update()
    {
        if (_isDead)
        {
            return;
        }

        if (_target == null && !_navMeshAgent.pathPending && _navMeshAgent.remainingDistance <= _navMeshAgent.stoppingDistance)
        {
            SetRandomDestination();
        }

        _animator.SetFloat("Move", _navMeshAgent.velocity.magnitude);
        CheckForTarget();
        _healthUI.value = _currentHealth;
    }

    private void SetRandomDestination()
    {
        Vector3 randomPoint = transform.position + Random.insideUnitSphere * _moveDetectDistance;
        NavMeshHit hit;

        if (NavMesh.SamplePosition(randomPoint, out hit, _maxDistance, NavMesh.AllAreas))
        {
            _navMeshAgent.SetDestination(hit.position);
        }
    }

    private void CheckForTarget()
    {
        if (_target == null)
        {
            TryFindTarget();
        }
        else
        {
            UpdateTarget();
        }
    }

    private void TryFindTarget()
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, _sightDistance);

        foreach (var hitCollider in hitColliders)
        {
            if (IsValidTarget(hitCollider))
            {
                _target = hitCollider.transform;
                break;
            }
        }
    }

    private bool IsValidTarget(Collider collider)
    {
        if (!collider.CompareTag("Player"))
        {
            return false;
        }

        Vector3 directionToTarget = collider.transform.position - transform.position;
        float angleToTarget = Vector3.Angle(transform.forward, directionToTarget);

        if (angleToTarget > _sightAngle / 2)
            return false;

        RaycastHit hit;
        if (!Physics.Raycast(transform.position, directionToTarget.normalized, out hit, _sightDistance))
            return false;

        return hit.collider.CompareTag("Player") 
            && hit.collider.TryGetComponent(out Player otherPlayer) 
            && otherPlayer.IsDead == false;
    }

    private void UpdateTarget()
    {
        if (_target.TryGetComponent(out Player otherPlayer) && otherPlayer.IsDead == true)
        {
            _target = null;
            return;
        }
        transform.LookAt(_target);

        float distanceToTarget = Vector3.Distance(transform.position, _target.position);
        if (distanceToTarget > _equippedWeapon.Range)
        {
            _navMeshAgent.SetDestination(_target.position);
            _navMeshAgent.stoppingDistance = _equippedWeapon.Range;
        }
        else
        {
            Shot();
        }
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.green;
        Gizmos.color = new Color(0, 1, 0, 0.2f);
        Vector3 leftBoundary = Quaternion.Euler(0, -_sightAngle / 2, 0) * transform.forward;
        Vector3 rightBoundary = Quaternion.Euler(0, _sightAngle / 2, 0) * transform.forward;
        Gizmos.DrawLine(transform.position, transform.position + leftBoundary * _sightDistance);
        Gizmos.DrawLine(transform.position, transform.position + rightBoundary * _sightDistance);
    }
}
