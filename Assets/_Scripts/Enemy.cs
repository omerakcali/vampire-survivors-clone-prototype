using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStats Stats;
    
    public int Hp { get; private set; }

    private EnemySpawner _enemySpawner;

    private void Awake()
    {
        _enemySpawner = ServiceProvider.Instance.Get<EnemySpawner>();
    }

    public void ResetToDefault()
    {
        Hp = Stats.MaxHp;
    }

    public void Initialize(Vector3 position, Transform parent)
    {
        ResetToDefault();
        transform.SetParent(parent);
        transform.position = position;
        transform.LookAt(PlayerCharacter.Instance.transform);
        gameObject.SetActive(true);
    }
}
