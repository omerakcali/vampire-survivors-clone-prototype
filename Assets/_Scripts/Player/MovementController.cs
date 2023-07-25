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
    private GameStartManager _gameStartManager;
    private float _lastRotationZ;

    private void Awake()
    {
        _playerInputService = ServiceProvider.Instance.Get<PlayerInputService>();
        _gameStartManager = ServiceProvider.Instance.Get<GameStartManager>();
    }

    private void Update()
    {
        if(!_gameStartManager.GameStarted) return;
        if (_playerInputService.GetTouchCount() <= 0) return;
        if(_playerInputService.Touch0Delta.magnitude == 0f) return;
        var moveVector = new Vector3(_playerInputService.Touch0Delta.x, 0, _playerInputService.Touch0Delta.y);
        moveVector = Vector3.ClampMagnitude(moveVector, 1f);
        transform.Translate(moveVector * (Time.deltaTime * PlayerStats.MoveSpeed));

        var currentRotation = GraphicsRoot.localEulerAngles;
        var targetYRotation = Mathf.Atan2(moveVector.x,moveVector.z)*Mathf.Rad2Deg;
        var YRotation = Mathf.MoveTowardsAngle(currentRotation.y,  targetYRotation , PlayerStats.RotationSpeed*Time.deltaTime);
        currentRotation =
            new Vector3(currentRotation.x, YRotation, currentRotation.z);
        GraphicsRoot.localEulerAngles = currentRotation;
    }
}
