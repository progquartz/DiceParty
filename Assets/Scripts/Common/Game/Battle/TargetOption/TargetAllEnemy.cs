using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetAllEnemy : BaseTargetOption
{
    public override List<BaseTarget> GetTarget(BaseTarget caller)
    {
        List<BaseEnemy> enemies = BattleManager.Instance.GetAllEnemys();
        List<BaseEnemy> aliveEnemies = new List<BaseEnemy>();
        foreach (BaseEnemy enemy in enemies)
        {
            if (!enemy.stat.isDead)
            {
                aliveEnemies.Add(enemy);
            }
        }
        return new List<BaseTarget>(aliveEnemies);
    }
}
