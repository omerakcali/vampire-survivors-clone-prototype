using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FrostboltProjectile : MonoBehaviour, IEnemyTriggerReceiver
{
    [SerializeField] private PlayerStats Stats;
    
    private Enemy _target;
    private Vector3 _direction;
    private float _startTime;
    private List<Enemy> _enemiesHit = new();
    private Action<FrostboltProjectile> _onDestroy;

    public void Shoot(Enemy target, Action<FrostboltProjectile> onDestroy = null)
    {
        _onDestroy = onDestroy;
        _startTime = Time.time;
        _target = target;
        _direction = target != null ? target.transform.position - transform.position : Random.onUnitSphere;
        _direction = new Vector3(_direction.x, 0, _direction.z);
        _direction.Normalize();
        _enemiesHit.Clear();
        gameObject.SetActive(true);
    }

    private void Update()
    {
        if(_target == null) return;
        if (Time.time > _startTime + Stats.FrostboltProjectileDuration)
        {
            DestroyProjectile();
            return;
        }
        transform.Translate(_direction * (Time.deltaTime * Stats.FrostboltSpeed));
    }

    private void DestroyProjectile()
    {
        if (_onDestroy == null) Destroy(gameObject);
        else _onDestroy.Invoke(this);
    }

    public void OnEnemyTrigger(Enemy enemy)
    {
        if (_enemiesHit.Contains(enemy)) return;
        _enemiesHit.Add(enemy);
        enemy.Hit(Stats.FrostboltDamage);
    }

    public void ResetToDefault()
    {
        _target = null;
        _onDestroy = null;
    }
}
