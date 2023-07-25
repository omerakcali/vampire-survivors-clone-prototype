using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Serialization;
using UnityEngine.UI;

public class GameStartManager : Service<GameStartManager>
{
    [SerializeField] private Transform StartGamePanel;
    [SerializeField] private Button StartGameButton;
    [SerializeField] private Transform RestartGamePanel;
    [SerializeField] private Button RestartGameButton;

    private PlayerDieEvent _playerDieEvent;
    public bool GameStarted { get; private set; }
    internal override void Init()
    {
        _playerDieEvent = _serviceProvider.Get<PlayerDieEvent>();
        
        StartGamePanel.gameObject.SetActive(true);
        GameStarted = false;
        Time.timeScale = 0f; // FIX this logic
    }

    internal override void Begin()
    {
        StartGameButton.onClick.AddListener(StartGame);
        RestartGameButton.onClick.AddListener(RestartGame);
        
        _playerDieEvent.Subscribe(OnPlayerDie);
    }

    private void OnPlayerDie(bool arg0)
    {
        GameStarted = false;
        Time.timeScale = 0f;
        ShowDeathPanel();
    }

    private void StartGame()
    {
        GameStarted = true;
        StartGamePanel.gameObject.SetActive(false);
        Time.timeScale = 1;
    }

    private void ShowDeathPanel()
    {
        RestartGamePanel.gameObject.SetActive(true);
    }

    private void RestartGame()
    {
        SceneManager.LoadScene(0);
    }
}
