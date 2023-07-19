using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerInputService : Service<PlayerInputService>
{
    public bool IsTouch0StartedOnUI { get; private set; }
    
    public Vector2 Touch0Delta => GetTouchPosition(0) - _touch0StartPos;

    private Vector2 _touch0StartPos;
    
    public int GetTouchCount()
    {
        if (Input.touchCount > 0) return Input.touchCount;
        return Input.touchCount + (Input.GetMouseButton(0) || Input.GetMouseButtonDown(0) ? 1 : 0);
    }

    private Touch GetTouch(int id)
    {
        return Input.GetTouch(id);
    }

    public Vector2 GetTouchPosition(int id)
    {
        return Input.touchCount > 0 ? GetTouch(0).position : Input.mousePosition;
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
        else
        {
            if (Input.GetMouseButtonDown(0))
            {
                _touch0StartPos = Input.mousePosition;
                IsTouch0StartedOnUI = EventSystem.current.IsPointerOverGameObject(0);
            }
        }
    }
}
