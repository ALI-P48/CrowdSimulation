using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Serialization;

public class CrowdManager : MonoBehaviour
{
    public static CrowdManager Instance;
    
    [SerializeField] private GameObject _npcPrefab;
    [SerializeField] private int _npcCount = 200;
    [SerializeField] private List<Transform> _spawnPoints;

    private List<SteeringAgent> _agents = new List<SteeringAgent>();

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
        }
    }
    
    private void Start()
    {
        SpawnNPCs();
    }

    private void Update()
    {
        for (int i = 0; i < _agents.Count; i++)
        {
            _agents[i].Steer();
        }
    }

    public Vector3 GetRandomSpawnPoint()
    {
        return _spawnPoints[Random.Range(0, _spawnPoints.Count)].position;
    }

    private void SpawnNPCs()
    {
        _agents = new List<SteeringAgent>(_npcCount);
        for (int i = 0; i < _npcCount; i++)
        {
            Vector3 spawnPoint = GetRandomSpawnPoint();
            Vector3 destination = GetRandomSpawnPoint();
            GameObject npc = Instantiate(_npcPrefab, spawnPoint, Quaternion.identity);
            npc.transform.parent = transform;
            SteeringAgent agent = npc.GetComponent<SteeringAgent>();
            agent.SetTarget(destination);
            _agents.Add(agent);
        }
    }
}