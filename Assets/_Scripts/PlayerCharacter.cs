using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCharacter : MonoBehaviour
{
    public static PlayerCharacter Instance;

    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    }
}
