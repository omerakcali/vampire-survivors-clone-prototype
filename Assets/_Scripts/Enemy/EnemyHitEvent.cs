using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHitEvent : Event<EnemyHitEvent.Data,EnemyHitEvent>
{
    public class Data
    {
        public Enemy Enemy;
        public int Damage;

        public Data(Enemy enemy, int damage)
        {
            Enemy = enemy;
            Damage = damage;
        }
    }
}

