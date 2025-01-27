using UnityEngine;

public class Weapon : MonoBehaviour
{
    [SerializeField] private float _speed;
    [SerializeField] private float _range;

    private void OnDrawGizmos()
    {
        Debug.DrawLine(transform.position, transform.forward * _range);
    }
}
