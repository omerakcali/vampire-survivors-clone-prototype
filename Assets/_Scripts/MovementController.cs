using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MovementController : MonoBehaviour
{
    [SerializeField] 
    private PlayerStats PlayerStats;

    [SerializeField]
    private Transform GraphicsRoot;
    
    private PlayerInputService _playerInputService;

    private void Awake()
    {
        _playerInputService = ServiceProvider.Instance.Get<PlayerInputService>();
    }

    private void Update()
    {
        if (_playerInputService.GetTouchCount() <= 0) return;
        if(_playerInputService.Touch0Delta.magnitude == 0f) return;
        var moveVector = new Vector3(_playerInputService.Touch0Delta.x, 0, _playerInputService.Touch0Delta.y);
        moveVector = Vector3.ClampMagnitude(moveVector, 1f);
        transform.Translate(moveVector*Time.deltaTime*PlayerStats.MoveSpeed);
        GraphicsRoot.forward = moveVector;
    }
}
