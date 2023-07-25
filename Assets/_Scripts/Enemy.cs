using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class Enemy : MonoBehaviour, IPlayerCollisionReceiver
{
    [SerializeField] private EnemyStats Stats;

    [SerializeField] private Transform GraphicsRoot;

    private EnemyHitEvent _enemyHitEvent;
    private EnemyDiedEvent _enemyDiedEvent;
    
    private PlayerCharacter _playerCharacter;

    private Tween _hitTween;
    private bool _playerHit;
    private float _lastHitTime;
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
        GraphicsRoot.localScale = Vector3.one;
        Hp = Stats.MaxHp;
        _playerHit = false;
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
        _hitTween?.Kill(true);
        _hitTween =GraphicsRoot.DOShakeScale(.12f, .25f, 2);
        Hp -= damage;
        _enemyHitEvent.Fire(new EnemyHitEvent.Data(this,damage));
        if (Hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _hitTween?.Kill();
        GraphicsRoot.DOScale(.1f, .25f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            _enemyDiedEvent.Fire(this);
        });
    }

    private void Update()
    {
        if(_moving)
        {
            var moveVector = _playerCharacter.transform.position - transform.position;
            moveVector = Vector3.ClampMagnitude(moveVector, 1f);
            transform.Translate(moveVector * (Time.deltaTime * Stats.MoveSpeed));
            var currentRotation = GraphicsRoot.localEulerAngles;
            var targetYRotation = Mathf.Atan2(moveVector.x, moveVector.z) * Mathf.Rad2Deg;
            var YRotation =
                Mathf.MoveTowardsAngle(currentRotation.y, targetYRotation, Stats.RotationSpeed * Time.deltaTime);
            currentRotation =
                new Vector3(currentRotation.x, YRotation, currentRotation.z);
            GraphicsRoot.localEulerAngles = currentRotation;
        }

        if (_playerHit)
        {
            if (Time.time > _lastHitTime + Stats.MeleeAttackInterval)
            {
                _lastHitTime = Time.time;
                _playerCharacter.Hit(Stats.Damage);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        var receiver = other.GetComponentInParent<IEnemyTriggerReceiver>();
        if(receiver == null) return;
        receiver.OnEnemyTrigger(this);
    }

    public void OnPlayerCollisionEnter(PlayerCharacter player)
    {
        _playerHit = true;
    }

    public void OnPlayerCollisionExit()
    {
        _playerHit = false;
    }
}

public interface IEnemyTriggerReceiver
{
    public void OnEnemyTrigger(Enemy enemy);
}