using UnityEngine;

[RequireComponent(typeof(SteeringAgent))]
public class LookAtMovementDirection : MonoBehaviour
{
    [SerializeField] private float _min = 0.1f;


    private SteeringAgent _steeringAgent;
    
    private void Awake()
    {
        _steeringAgent = GetComponent<SteeringAgent>();
    }
    
    private void Update()
    {
        Vector3 direction = _steeringAgent.Velocity;
        direction.Normalize();
        if (direction.magnitude > _min)
        {
            transform.forward = direction;
        }
    }
}
