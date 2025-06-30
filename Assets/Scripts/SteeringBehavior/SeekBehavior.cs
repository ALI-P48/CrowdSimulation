using UnityEngine;
using UnityEngine.Serialization;

public class SeekBehavior : MonoBehaviour, ISteeringBehavior
{
    public bool IsActive => _isActive && _target != null;
    public float Weight => _weight;
    
    [SerializeField] private bool _isActive = true;
    [SerializeField] private float _weight = 1f;

    private SteeringAgent _agent;
    private Vector3 _target;

    void Awake()
    {
        _agent = GetComponent<SteeringAgent>();
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
    }

    public Vector3 CalculateSteering()
    {
        Vector3 desiredVelocity = (_target - transform.position).normalized * _agent.MaxSpeed;
        return desiredVelocity;
    }
}
