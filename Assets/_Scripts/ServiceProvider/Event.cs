using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Event<TEventType, TBaseType> : Service<TBaseType>
{
    private UnityEvent<TEventType> _event = new UnityEvent<TEventType>();
    
    public virtual void Fire(TEventType arg)
    {
        _event.Invoke(arg);
    }

    public void Subscribe(UnityAction<TEventType> listener)
    {
        _event.AddListener(listener);
    }

    public void UnSubscribe(UnityAction<TEventType> listener)
    {
        _event.RemoveListener(listener);
    }
}
