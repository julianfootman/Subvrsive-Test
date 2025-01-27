using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _range;
    [SerializeField] private Transform _rigLeft;
    [SerializeField] private Transform _rigRight;
    public Transform LeftRig => _rigLeft;
    public Transform RightRig => _rigRight;

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.forward * _range + transform.position, Color.red);
    }
}
