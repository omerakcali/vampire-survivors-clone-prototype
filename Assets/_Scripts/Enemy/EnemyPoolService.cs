using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyPoolService : Service<EnemyPoolService>
{
    [SerializeField] 
    private Enemy EnemyPrefab;

    [SerializeField] 
    private Transform PoolRoot;
    
    [SerializeField] 
    private int PoolInitSize = 20;

    private List<Enemy> _reservedItems = new();
    
    internal override void Begin()
    {
        PopulatePool();
        SetReady();
    }

    private void PopulatePool()
    {
        for (int i = 0; i < PoolInitSize; i++)
        {
            var instance = Instantiate(EnemyPrefab,PoolRoot);
            instance.ResetToDefault();
            instance.gameObject.SetActive(false);
            _reservedItems.Add(instance);
        }
    }

    public Enemy Request()
    {
        if (_reservedItems.Count > 0)
        {
            var instance = _reservedItems[^1];
            _reservedItems.RemoveAt(_reservedItems.Count-1);
            instance.ResetToDefault();
            return instance;
        }
        else
        {
            var instance = Instantiate(EnemyPrefab);
            instance.ResetToDefault();
            return instance;
        }
    }

    public void Return(Enemy enemy)
    {
        _reservedItems.Add(enemy);
        enemy.gameObject.SetActive(false);
        enemy.transform.SetParent(PoolRoot);
    }
}
