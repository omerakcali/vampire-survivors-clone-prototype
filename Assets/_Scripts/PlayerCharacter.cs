using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    [SerializeField] private PlayerStats Stats;
    [SerializeField] private Transform GraphicsRoot;
    
    public static PlayerCharacter Instance;
    public int Hp { get; private set; }

    private PlayerHitEvent _playerHitEvent;
    private PlayerDieEvent _playerDieEvent;
    
    private Tween _hitTween;
    private bool _dead;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
        _dead = false;
        Hp = Stats.MaxHp;
        
        _playerHitEvent = ServiceProvider.Instance.Get<PlayerHitEvent>();
        _playerDieEvent = ServiceProvider.Instance.Get<PlayerDieEvent>();
    }

    public void Hit(int damage)
    {
        if(_dead) return;
        _hitTween?.Kill(true);
        _hitTween =GraphicsRoot.DOShakeScale(.12f, .25f, 2);
        Hp -= damage;
        _playerHitEvent.Fire(new PlayerHitEvent.Data(damage));
        
        if (Hp <= 0)
        {
            Die();
        }
    }

    private void Die()
    {
        _dead = true;
        _hitTween?.Kill();
        GraphicsRoot.DOScale(.1f, .25f).SetEase(Ease.InCubic).OnComplete(() =>
        {
            _playerDieEvent.Fire(true);
        });
    }

    
    private void OnCollisionEnter(Collision other)
    {
        var receiver = other.gameObject.GetComponentInParent<IPlayerCollisionReceiver>();
        if(receiver != null)
            receiver.OnPlayerCollisionEnter(this);
    }

    private void OnCollisionExit(Collision other)
    { 
        var receiver = other.gameObject.GetComponentInParent<IPlayerCollisionReceiver>();
        if(receiver != null)
            receiver.OnPlayerCollisionExit();
    }
}

public interface IPlayerCollisionReceiver
{
    public void OnPlayerCollisionEnter(PlayerCharacter player);

    public void OnPlayerCollisionExit();
}
