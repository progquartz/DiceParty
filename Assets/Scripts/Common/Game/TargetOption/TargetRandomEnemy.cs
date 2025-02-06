using System.Collections.Generic;
using UnityEngine;

public class TargetRandomEnemy : BaseTargetOption
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
        if (aliveEnemies.Count == 0) return new List<BaseTarget>();

        int randIndex = Random.Range(0, aliveEnemies.Count);
        return new List<BaseTarget> { aliveEnemies[randIndex] };
    }
}
