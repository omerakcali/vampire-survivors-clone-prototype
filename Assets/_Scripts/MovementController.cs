using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    private PlayerInputService _playerInputService;

    private void Awake()
    {
        _playerInputService = ServiceProvider.Instance.Get<PlayerInputService>();
    }

    private void Update()
    {
        if(_playerInputService.GetTouchCount()>0 && !_playerInputService.IsTouch0StartedOnUI)
        {
            Debug.Log(_playerInputService.Touch0Delta.normalized);
        }
    }
}
