using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHitEvent : Event<PlayerHitEvent.Data,EnemyHitEvent>
{
    public class Data
    {
        public int Damage;

        public Data(int damage)
        {
            Damage = damage;
        }
    }
}
