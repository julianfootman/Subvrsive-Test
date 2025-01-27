using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _range;
    [SerializeField] private Transform _muzzle;
    [SerializeField] private Transform _rigLeft;
    [SerializeField] private Transform _rigRight;
    [SerializeField] private Bullet _bullet;

    public Transform LeftRig => _rigLeft;
    public Transform RightRig => _rigRight;
    public float Range => _range;

    public void LaunchProjectile(Vector3 direction)
    {
        Bullet bullet = Instantiate(_bullet);
        bullet.transform.position = _muzzle.transform.position;
        bullet.Launch(direction);
    }

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.forward * _range + transform.position, Color.red);
    }
}
