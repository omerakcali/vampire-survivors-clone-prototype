using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStats Stats;

    [SerializeField] private Transform GraphicsRoot;


    private PlayerCharacter _playerCharacter;
    
    public int Hp { get; private set; }

    private bool _moving;

    private void Awake()
    {
        _playerCharacter = PlayerCharacter.Instance;
    }

    public void ResetToDefault()
    {
        Hp = Stats.MaxHp;
    }

    public void Initialize(Vector3 position, Transform parent)
    {
        ResetToDefault();
        transform.SetParent(parent);
        transform.position = position;
        GraphicsRoot.LookAt(PlayerCharacter.Instance.transform);
        gameObject.SetActive(true);
        SetMove(true);
    }

    private void SetMove(bool status)
    {
        _moving = status;
    }

    private void Update()
    {
        if(!_moving) return;
        var moveVector = _playerCharacter.transform.position - transform.position;
        moveVector = Vector3.ClampMagnitude(moveVector, 1f);
        transform.Translate(moveVector * (Time.deltaTime * Stats.MoveSpeed));
        var currentRotation = GraphicsRoot.localEulerAngles;
        var targetYRotation = Mathf.Atan2(moveVector.x,moveVector.z)*Mathf.Rad2Deg;
        var YRotation = Mathf.MoveTowardsAngle(currentRotation.y,  targetYRotation , Stats.RotationSpeed*Time.deltaTime);
        currentRotation =
            new Vector3(currentRotation.x, YRotation, currentRotation.z);
        GraphicsRoot.localEulerAngles = currentRotation;
    }
}
