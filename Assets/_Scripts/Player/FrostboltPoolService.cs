using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class FrostboltPoolService : Service<FrostboltPoolService>
{
    [SerializeField] 
    private FrostboltProjectile Prefab;

    [SerializeField] 
    private Transform PoolRoot;
    
    [SerializeField] 
    private int PoolInitSize = 20;

    private List<FrostboltProjectile> _reservedItems = new();
    
    internal override void Begin()
    {
        PopulatePool();
        SetReady();
    }

    private void PopulatePool()
    {
        for (int i = 0; i < PoolInitSize; i++)
        {
            var instance = Instantiate(Prefab,PoolRoot);
            //instance.ResetToDefault();
            instance.gameObject.SetActive(false);
            _reservedItems.Add(instance);
        }
    }

    public FrostboltProjectile Request()
    {
        if (_reservedItems.Count > 0)
        {
            var instance = _reservedItems[^1];
            _reservedItems.RemoveAt(_reservedItems.Count-1);
            instance.transform.SetParent(null);
            instance.ResetToDefault();
            return instance;
        }
        else
        {
            var instance = Instantiate(Prefab);
            instance.ResetToDefault();
            return instance;
        }
    }

    public void Return(FrostboltProjectile projectile)
    {
        _reservedItems.Add(projectile);
        projectile.gameObject.SetActive(false);
        projectile.transform.SetParent(PoolRoot);
    }
}
