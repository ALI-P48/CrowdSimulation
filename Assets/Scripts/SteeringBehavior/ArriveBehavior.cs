using UnityEngine;
using UnityEngine.Serialization;

public class ArriveBehavior : MonoBehaviour, ISteeringBehavior
{
    public bool IsActive => _isActive && _target != null;
    public float Weight => _weight;
    
    [SerializeField] private bool _isActive = true;
    [SerializeField] private float _weight = 1f;
    [SerializeField] private float _arriveRadius = 2f;
    [SerializeField] private float _slowingRadius = 5f;
    [SerializeField] private float _timeToTarget = 0.1f;

    private SteeringAgent _agent;
    private Vector3 _target;

    private void Awake()
    {
        _agent = GetComponent<SteeringAgent>();
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
    }

    public Vector3 CalculateSteering()
    {
        Vector3 toTarget = _target - transform.position;
        float distance = toTarget.magnitude;

        if (distance < _arriveRadius)
        {
            return Vector3.zero;
        }

        float speed = distance > _slowingRadius ? _agent.MaxSpeed : _agent.MaxSpeed * (distance / _slowingRadius);
        Vector3 desiredVelocity = toTarget.normalized * speed;
        return desiredVelocity;
    }
}