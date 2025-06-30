using UnityEngine;

public interface ISteeringBehavior {
    public bool IsActive { get;}
    public float Weight { get;}
    public Vector3 CalculateSteering();
    public void SetTarget(Vector3 target);
}