using Unity.Burst;
using Unity.Collections;
using Unity.Jobs;
using UnityEngine;

[BurstCompile]
struct AvoidanceJob : IJobParallelFor {
    [ReadOnly] public NativeArray<Vector3> positions;
    public Vector3 selfPosition;
    public float radius;
    public NativeArray<Vector3> results;

    public void Execute(int index) {
        Vector3 dir = Vector3.zero;
        Vector3 otherPos = positions[index];
        if (selfPosition != otherPos) {
            float dist = Vector3.Distance(selfPosition, otherPos);
            if (dist < radius) {
                dir += (selfPosition - otherPos).normalized * ((radius - dist) / radius);
            }
        }
        results[index] = dir;
    }
}