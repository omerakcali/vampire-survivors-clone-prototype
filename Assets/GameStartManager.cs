using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class GameStartManager : Service<GameStartManager>
{
    [SerializeField] private Canvas Canvas;
    [SerializeField] private Button Button;

    public bool GameStarted { get; private set; }
    internal override void Init()
    {
        Canvas.gameObject.SetActive(true);
        GameStarted = false;
        Time.timeScale = 0f; // FIX this logic
    }

    internal override void Begin()
    {
        Button.onClick.AddListener(StartGame);
    }

    private void StartGame()
    {
        GameStarted = true;
        Canvas.gameObject.SetActive(false);
        Time.timeScale = 1;
    }
}
