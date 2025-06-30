using System.Collections.Generic;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

public class AvoidanceBehavior : MonoBehaviour, ISteeringBehavior
{
    public static List<Transform> AllAgents = new List<Transform>();
    
    public bool IsActive => _isActive;
    public float Weight => _weight;
    
    [SerializeField] private bool _isActive = true;
    [SerializeField] private float _weight = 1f;
    [SerializeField] private float _radius = 1.5f;
    [SerializeField] private float _strength = 3f;
    
    private Vector3 avoidanceDir;
    private NativeArray<Vector3> _positions;
    private NativeArray<Vector3> _results;
    private int _maxAgentCount = 200;

    private void OnEnable()
    {
        AllAgents.Add(transform);
    }

    private void OnDisable()
    {
        AllAgents.Remove(transform);
    }

    private void Awake()
    {
        _positions = new NativeArray<Vector3>(_maxAgentCount, Allocator.Persistent);
        _results = new NativeArray<Vector3>(_maxAgentCount, Allocator.Persistent);
    }

    private void OnDestroy()
    {
        if (_positions.IsCreated) _positions.Dispose();
        if (_results.IsCreated) _results.Dispose();
    }

    public void SetTarget(Vector3 target)
    {
        
    }

    public Vector3 CalculateSteering()
    {
        List<Vector3> gg = new List<Vector3>();
        for (int i = 0; i < AllAgents.Count; i++) {
            if (Vector3.Distance(transform.position, AllAgents[i].position) < _radius)
            {
                gg.Add(AllAgents[i].position);
            }
        }
        
        int count = gg.Count;

        for (int i = 0; i < count; i++) {
            _positions[i] = gg[i];
        }

        AvoidanceJob job = new AvoidanceJob {
            positions = _positions,
            selfPosition = transform.position,
            radius = _radius,
            results = _results
        };

        JobHandle handle = job.Schedule(count, 32);
        handle.Complete();

        avoidanceDir = Vector3.zero;
        for (int i = 0; i < count; i++) {
            avoidanceDir += _results[i];
        }

        return avoidanceDir * _strength;
    }
}
