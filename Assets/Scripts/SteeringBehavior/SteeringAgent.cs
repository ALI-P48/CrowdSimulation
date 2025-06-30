using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class SteeringAgent : MonoBehaviour
{
    public Vector3 Velocity
    {
        get;
        private set;
    }

    public float MaxSpeed => _maxSpeed;
    
    [SerializeField] private float _maxSpeed = 5f;

    private List<ISteeringBehavior> behaviors;

    private void Awake()
    {
        Velocity = Vector3.zero;
        behaviors = new List<ISteeringBehavior>(GetComponents<ISteeringBehavior>());
    }

    public void Steer()
    {
        Vector3 velocity = Vector3.zero;
        foreach (var behavior in behaviors)
        {
            if (behavior.IsActive)
            {
                velocity += behavior.CalculateSteering() * behavior.Weight;
            }
        }
        velocity.y = 0f;
        Velocity = Vector3.ClampMagnitude(velocity, _maxSpeed);
        transform.position += Velocity * Time.deltaTime;
    }

    public void SetTarget(Vector3 target)
    {
        foreach (var behavior in behaviors)
        {
            behavior.SetTarget(target);
        }
    }
}
