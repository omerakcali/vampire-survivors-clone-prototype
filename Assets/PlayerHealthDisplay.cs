using System;
using System.Collections;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthDisplay : MonoBehaviour
{
    [SerializeField] private Slider Slider;
    [SerializeField] private PlayerStats PlayerStats;
    
    private PlayerHitEvent _playerHitEvent;

    private float _sliderVal;
    private Tween _sliderTween;
    private void Awake()
    {
        _playerHitEvent = ServiceProvider.Instance.Get<PlayerHitEvent>();
    }

    private void Start()
    {
        _playerHitEvent.Subscribe(OnPlayerHit);
        Slider.value = (float)PlayerCharacter.Instance.Hp / PlayerStats.MaxHp;
    }

    private void OnPlayerHit(PlayerHitEvent.Data data)
    {
        _sliderTween?.Kill();
        _sliderTween = Slider.DOValue((float)PlayerCharacter.Instance.Hp / PlayerStats.MaxHp, .25f);
    }
}
