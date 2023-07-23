using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostboltSkill : MonoBehaviour , IPlayerSkill
{
    [SerializeField] private Transform SpawnPoint;
    [SerializeField] private PlayerStats PlayerStats;
    
    private bool _active;
    private float _lastInvokeTime;

    private EnemySpawner _enemySpawner;
    private FrostboltPoolService _pool;
    private void Awake()
    {
        _enemySpawner = ServiceProvider.Instance.Get<EnemySpawner>();
        _pool = ServiceProvider.Instance.Get<FrostboltPoolService>();
    }

    private void Start()
    {
        Activate();
    }

    public void Activate()
    {
        _active = true;
        _lastInvokeTime = Time.time;
    }

    public void Deactivate()
    {
        _active = false;
    }

    public void Upgrade()
    {
        throw new System.NotImplementedException();
    }

    private void InvokeSkill()
    {
        _lastInvokeTime = Time.time;
        var enemy = _enemySpawner.GetNearestEnemy(transform.position);
        var projectile = _pool.Request();
        projectile.transform.position = SpawnPoint.position;
        projectile.Shoot(enemy,OnProjectileDestroy);
    }

    private void OnProjectileDestroy(FrostboltProjectile projectile)
    {
        _pool.Return(projectile);
    }
    
    private void Update()
    {
        if(!_active) return;
        if(Time.time > _lastInvokeTime + PlayerStats.FrostboltInterval) InvokeSkill();
    }
}

public interface IPlayerSkill
{
    public void Activate();
    public void Deactivate();
    public void Upgrade();
}
