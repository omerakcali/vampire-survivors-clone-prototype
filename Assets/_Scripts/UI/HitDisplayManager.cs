using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.FullSerializer;
using UnityEngine;

public class HitDisplayManager : Service<HitDisplayManager>
{
    [SerializeField] private HitDisplayItem TemplateItem;
    
    private EnemyHitEvent _enemyHitEvent;

    private List<HitDisplayItem> _reservedItems = new();
    internal override void Init()
    {
        _enemyHitEvent = _serviceProvider.Get<EnemyHitEvent>();
    }

    internal override void Begin()
    {
        PopulatePool();
        _enemyHitEvent.Subscribe(OnEnemyHit);
        SetReady();
    }

    private void OnEnemyHit(EnemyHitEvent.Data data)
    {
        var item = GetItem();
        item.Display(Camera.main.WorldToScreenPoint(data.Enemy.transform.position),data.Damage);
    }
    

    private HitDisplayItem GetItem()
    {
        if (_reservedItems.Count > 0)
        {
            var instance = _reservedItems[^1];
            _reservedItems.RemoveAt(_reservedItems.Count-1);
            return instance;
        }
        else
        {
            var instance = Instantiate(TemplateItem);
            instance.Init(this);
            return instance;
        }
    }

    private void PopulatePool()
    {
        for (int i = 0; i < 15; i++)
        {
            var item = Instantiate(TemplateItem,transform);
            item.Init(this);
            item.gameObject.SetActive(false);
            _reservedItems.Add(item);
        }
    }
    public void Return(HitDisplayItem item)
    {
        _reservedItems.Add(item);
        item.gameObject.SetActive(false);
    }
}
