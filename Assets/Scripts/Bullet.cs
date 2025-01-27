using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] private float _speed = 10f;
    [SerializeField] private float _damage = 15f;
    [SerializeField] private Rigidbody _rigidbody;

    private Vector3 _startPosition;
    private float _maxRange;

    private void Start()
    {
        _startPosition = transform.position;
    }

    public void Launch(Vector3 direction, float maxRange)
    {
        transform.forward = direction;
        _rigidbody.AddForce(transform.forward * _speed);
        _maxRange = maxRange;
    }

    private void Update()
    {
        if (Vector3.Distance(_startPosition, transform.position) >= _maxRange)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Player")
        {
            collision.gameObject.GetComponent<Player>().TakeDamage(_damage);
        }
    }
}
