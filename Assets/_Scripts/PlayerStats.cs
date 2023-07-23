using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "Scriptable Objects/Player Stats")]
public class PlayerStats : ScriptableObject
{
    public int MaxHp;
    public float MoveSpeed;
    public float RotationSpeed;

    public int FrostboltDamage;
    public float FrostboltInterval;
    public float FrostboltSpeed;
    public float FrostboltProjectileDuration;
}
