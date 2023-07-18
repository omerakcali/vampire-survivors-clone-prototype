using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.Design.Serialization;
using UnityEngine;
[DefaultExecutionOrder(-100000)]

public class ServiceProvider : MonoBehaviour
{
    public static  ServiceProvider Instance;
    
    private Dictionary<string, Service> _services = new Dictionary<string, Service>();

    private bool _dependencyDirty;
    private bool _checkingDependencies;
    private void Awake()
    {
        Instance = this;
    }

    public void Register(Service service)
    {
        _services.Add(service.name, service);
    }

    public void UnRegister(Service service)
    {
        _services.Remove(service.name);
    }

    public T Get<T>() where T:Service
    {
        var type = typeof(T);
        return (T)_services[type.Name];
    }
}
