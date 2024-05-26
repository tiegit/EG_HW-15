using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemyCreator : MonoBehaviour
{
    [SerializeField] private int _mumberOfGreenEnemies = 4; // сделано на скорую руку, для того чтобы не думать об алгоритме генерации
    [SerializeField] private Enemy _greenEnemyPrefab;

    [SerializeField] private int _mumberOfBlueEnemies = 4;
    [SerializeField] private Enemy _blueEnemyPrefab;
    
    [SerializeField, Space(10)] private PatrolManager _patrolManager;
    [SerializeField] private Transform _playerCenter;

    [SerializeField, Space(10)] private float _viewingDistance = 20f;
    [SerializeField] private float _viewingAngle = 50f;
    [SerializeField] private LayerMask _layerMask;


    private List<Transform> _spawnPoints = new();

    private void Awake()
    {
        _spawnPoints = GetComponentsInChildren<Transform>().ToList();
        _spawnPoints.Remove(transform);
    }

    private void Start() // сделано на скорую руку, для того чтобы не думать об алгоритме генерации
    {
        for (int i = 0; i < _mumberOfGreenEnemies; i++)
        {
            Create(_greenEnemyPrefab);
        }
        
        for (int i = 0; i < _mumberOfBlueEnemies; i++)
        {
            Create(_blueEnemyPrefab);
        }
    }
    public void CreateEnemy(EnemyType type)
    {
        switch (type)
        {
            case EnemyType.Green:
                Create(_greenEnemyPrefab);
                break;

            case EnemyType.Blue:
                Create(_blueEnemyPrefab);
                break;

            default:
                throw new ArgumentException(nameof(type));
        }
    }

    private void Create(Enemy enemyPrefab)
    {
        Transform randomPoint = _spawnPoints[Random.Range(0, _spawnPoints.Count)];

        var newEnemy = Instantiate(enemyPrefab, randomPoint.position, randomPoint.rotation).GetComponent<EnemyStateMachine>();

        newEnemy.Initialize(this, _patrolManager, _playerCenter, _viewingDistance, _viewingAngle, _layerMask);
    }
}