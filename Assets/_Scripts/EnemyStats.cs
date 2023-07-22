using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu(menuName = "Scriptable Objects/Enemy Stats")]
public class EnemyStats : ScriptableObject
{
    public int MaxHp;
    
    public float MoveSpeed;
    public float RotationSpeed;
    
    public float Damage;
    public float MeleeAttackInterval;
}
