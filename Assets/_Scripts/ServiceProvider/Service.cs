using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[DefaultExecutionOrder(-10000)]
public class Service : MonoBehaviour
{
    protected ServiceProvider _serviceProvider;

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

    protected void OnDestroy()
    {
        Dispose();
        _serviceProvider.UnRegister(this);
    }
}

public class Service<T> : Service
{
    
}
