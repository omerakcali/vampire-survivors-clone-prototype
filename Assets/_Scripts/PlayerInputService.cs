using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputService : Service<PlayerInputService>
{
    public bool IsTouch0StartedOnUI { get; private set; }
    
    public Vector2 Touch0Delta => GetTouch(0).position-_touch0StartPos;

    private Vector2 _touch0StartPos;
    
    public int GetTouchCount()
    {
        return Input.touchCount;
    }

    public Touch GetTouch(int id)
    {
        return Input.GetTouch(id);
    }
    

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            if (GetTouch(0).phase == TouchPhase.Began)
            {
                _touch0StartPos = GetTouch(0).position;
                IsTouch0StartedOnUI = EventSystem.current.IsPointerOverGameObject(GetTouch(0).fingerId);
            }
        }
    }
}
