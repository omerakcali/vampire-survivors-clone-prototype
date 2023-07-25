using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using Random = UnityEngine.Random;

public class EnemySpawner : Service<EnemySpawner>
{
    [SerializeField] private float SpawnInterval;
    [SerializeField] private int MaxSpawnedEnemies;
    [SerializeField] private Vector2 SpawnDistanceRange;
    
    private EnemyPoolService _enemyPoolService;

    private EnemyDiedEvent _enemyDiedEvent;

    private List<Enemy> _availableEnemies = new();
    private float _lastSpawnTime;
    private bool _spawning;
    
    internal override void Init()
    {
        _enemyPoolService = _serviceProvider.Get<EnemyPoolService>();
        _dependencies = new()
        {
            _enemyPoolService
        };

        _enemyDiedEvent = _serviceProvider.Get<EnemyDiedEvent>();
    }

    internal override void Begin()
    {
        _enemyDiedEvent.Subscribe(OnEnemyDied);
        StartSpawning();
        
        SetReady();
    }

    private void OnEnemyDied(Enemy enemy)
    {
        enemy.ResetToDefault();
        _availableEnemies.Remove(enemy);
        _enemyPoolService.Return(enemy);
        if(_availableEnemies.Count <=MaxSpawnedEnemies) StartSpawning();
    }

    private void StartSpawning()
    {
        _spawning = true;
    }

    private void StopSpawning()
    {
        _spawning = false;
    }

    private void SpawnEnemy()
    {
        _lastSpawnTime = Time.time;
        var enemy = _enemyPoolService.Request();
        _availableEnemies.Add(enemy);
        if(_availableEnemies.Count >MaxSpawnedEnemies) StopSpawning();
        var direction = new Vector3(Random.Range(-1f, 1f), 0, Random.Range(-1f, 1f)).normalized;
        var position = PlayerCharacter.Instance.transform.position +
                       direction * Random.Range(SpawnDistanceRange.x, SpawnDistanceRange.y);
        enemy.Initialize(position,transform);
    }

    public Enemy GetNearestEnemy(Vector3 origin)
    {
        if (_availableEnemies.Count == 0) return null;
        float minDistance = float.MaxValue;
        Enemy nearest = _availableEnemies[0];
        foreach (var enemy in _availableEnemies)
        {
            var distance = Vector3.Distance(enemy.transform.position, origin);
            if ( distance< minDistance)
            {
                minDistance = distance;
                nearest = enemy;
            }
        }

        return nearest;
    }
    

    private void Update()
    {
        if(!_spawning) return;
        if (Time.time > _lastSpawnTime + SpawnInterval)
            SpawnEnemy();
    }
}
