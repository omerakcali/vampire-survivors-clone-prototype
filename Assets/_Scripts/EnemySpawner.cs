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
    }

    internal override void Begin()
    {
        StartSpawning();
        SetReady();
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

    private void Update()
    {
        if(!_spawning) return;
        if (Time.time > _lastSpawnTime + SpawnInterval)
            SpawnEnemy();
    }
}
