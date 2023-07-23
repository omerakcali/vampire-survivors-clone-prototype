using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    [SerializeField] private EnemyStats Stats;

    [SerializeField] private Transform GraphicsRoot;

    private EnemyHitEvent _enemyHitEvent;
    private EnemyDiedEvent _enemyDiedEvent;
    
    private PlayerCharacter _playerCharacter;

    public int Hp { get; private set; }

    private bool _moving;

    private void Awake()
    {
        _enemyHitEvent = ServiceProvider.Instance.Get<EnemyHitEvent>();
        _enemyDiedEvent = ServiceProvider.Instance.Get<EnemyDiedEvent>();

        _playerCharacter = PlayerCharacter.Instance;
    }

    public void ResetToDefault()
    {
        SetMove(false);
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

    public void Hit(int damage)
    {
        Hp -= damage;
        _enemyHitEvent.Fire(new EnemyHitEvent.Data(this,damage));
        if (Hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _enemyDiedEvent.Fire(this);
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

    private void OnTriggerEnter(Collider other)
    {
        var receiver = other.GetComponentInParent<IEnemyTriggerReceiver>();
        if(receiver == null) return;
        receiver.OnEnemyTrigger(this);
    }
}

public interface IEnemyTriggerReceiver
{
    public void OnEnemyTrigger(Enemy enemy);
}