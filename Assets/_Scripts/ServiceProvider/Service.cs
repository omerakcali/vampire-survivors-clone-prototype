using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class Service : MonoBehaviour
{
    protected ServiceProvider _serviceProvider;
    protected List<Service> _dependencies;

    protected bool _isReady;

    protected void Awake()
    {
        _serviceProvider = ServiceProvider.Instance;
        _serviceProvider.Register(this);
    }

    internal virtual void Init()
    {
    }

    internal virtual void Begin()
    {
    }

    internal virtual void Dispose()
    {
    }

    public virtual void SetReady()
    {
        _isReady = true;
        _serviceProvider.SetDependencyDirty();
    }
    
    public virtual bool IsDependenciesReady()
    {
        if (_dependencies == null)
        {
            return true;
        }
        foreach (var service in _dependencies)
        {
            if (!service.IsReady())
                return false;
        }
        return true;
    }

    public bool IsReady()
    {
        return _isReady;
    }

    public virtual bool HasDependency()
    {
        return _dependencies != null && _dependencies.Count > 0;
    }
    
    protected void OnDestroy()
    {
        if (_serviceProvider != null)
        {
            Dispose();
            _serviceProvider.UnRegister(this);
        }
    }
}

public class Service<T> : Service
{
    
}
