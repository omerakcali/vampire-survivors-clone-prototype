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

    private List<Service> _dependencyWaitingServices = new List<Service>();
    private Dictionary<string, Service> _newServices = new Dictionary<string, Service>();
    
    private bool _dependencyDirty;
    private bool _checkingDependencies;
    private void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        Resolve();
    }

    public void Register(Service service)
    {
        string name = service.GetType().Name;
        _services.Add(name, service);
        _newServices.Add(name, service);
    }

    public void UnRegister(Service service)
    {
        _services.Remove(name);
        if (_newServices.ContainsKey(name))
            _newServices.Remove(name);
        if (_dependencyWaitingServices.Contains(service))
            _dependencyWaitingServices.Remove(service);
    }
    
    public void SetDependencyDirty()
    {
        if (_checkingDependencies)
        {
            _dependencyDirty = true;
        }
        else
        {
            CheckDependencies();
        }
    }

    private void Resolve()
    {
        foreach (var pair in _newServices)
        {
            pair.Value.Init();
        }
        foreach (var pair in _newServices)
        {
            if (pair.Value.HasDependency())
            {
                _dependencyWaitingServices.Add(pair.Value);
            }
            else
            {
                pair.Value.Begin();
            }
        }
        CheckDependencies();
        _newServices.Clear();
    }
    
    public void CheckDependencies()
    {
        _checkingDependencies = true;
        for (int i = _dependencyWaitingServices.Count - 1; i >= 0; i--)
        {
            var service = _dependencyWaitingServices[i];
            if (service.IsDependenciesReady())
            {
                _dependencyWaitingServices.Remove(service);
                service.Begin();
            }
        }
        if (_dependencyDirty)
        {
            _dependencyDirty = false;
            CheckDependencies();
        }
        _checkingDependencies = false;
    }

    public T Get<T>() where T:Service
    {
        var type = typeof(T);
        return (T)_services[type.Name];
    }
}
