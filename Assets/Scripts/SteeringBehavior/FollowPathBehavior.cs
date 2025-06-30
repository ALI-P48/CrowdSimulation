using UnityEngine;
using System.Collections.Generic;
using UnityEngine.AI;
using UnityEngine.Serialization;

public class FollowPathBehavior : MonoBehaviour, ISteeringBehavior
{
    public bool IsActive => _isActive && _path != null && _path.Length > 0;
    public float Weight => _weight;
    
    [SerializeField] private bool _isActive = true;
    [SerializeField] private float _weight = 1f;
    [SerializeField] private float _waypointRadius = 1f;
    [SerializeField] private float _arriveRadius = 2f;

    private SteeringAgent _agent;
    private Vector3 _target;
    private Vector3[] _path;
    private int _currentWaypointIndex = 0;

    private void Awake()
    {
        _agent = GetComponent<SteeringAgent>();
    }

    private void Start()
    {
    }

    public void SetTarget(Vector3 target)
    {
        _target = target;
        CalculatePath();
    }
    
    public Vector3 CalculateSteering()
    {
        Vector3 targetPos = _path[_currentWaypointIndex];
        Vector3 toTarget = targetPos - transform.position;
        float distance = toTarget.magnitude;
        
        Vector3 toNext = _currentWaypointIndex < _path.Length - 1
            ? _path[_currentWaypointIndex + 1] - transform.position
            : toTarget;
        bool isMovingForward = Vector3.Dot(_agent.Velocity.normalized, toNext.normalized) > -0.5f;

        if (distance < _waypointRadius && isMovingForward)
        {
            if (_currentWaypointIndex + 1 < _path.Length)
            {
                _currentWaypointIndex++;
            }
            else
            {
                SetTarget(CrowdManager.Instance.GetRandomSpawnPoint());
                return Vector3.zero;
            }
            targetPos = _path[_currentWaypointIndex];
            toTarget = targetPos - transform.position;
        }
        if (distance < _arriveRadius)
        {
            return Vector3.zero;
        }
        Vector3 desiredVelocity = toTarget.normalized * _agent.MaxSpeed;
        return desiredVelocity;
    }
    
    private void CalculatePath()
    {
        _currentWaypointIndex = 0;
        NavMeshPath path = new NavMeshPath();
        if (NavMesh.CalculatePath(transform.position, _target, NavMesh.AllAreas, path)) {
            _path = path.corners;
        }
    }
    
    private void OnDrawGizmos()
    {
#if UNITY_EDITOR
        if (!IsActive)
            return;
        
        Transform selected = UnityEditor.Selection.activeTransform;
        if (selected != null && (selected == transform || selected.IsChildOf(transform)))
        {
            for (int i = 0; i < _path.Length - 1; i++)
            {
                Debug.DrawLine(_path[i], _path[i + 1], Color.cyan, 0.0f, false);
            }
        }
#endif
    }
}