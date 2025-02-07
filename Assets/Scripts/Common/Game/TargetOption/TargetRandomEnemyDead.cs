using System.Collections.Generic;
using UnityEngine;

public class TargetRandomEnemyDead : BaseTargetOption
{
    public override List<BaseTarget> GetTarget(BaseTarget caller)
    {
        List<BaseEnemy> enemies = BattleManager.Instance.GetAllEnemys();
        List<BaseEnemy> deadEnemies = new List<BaseEnemy>();
        foreach (BaseEnemy enemy in enemies)
        {
            if (enemy.stat.isDead)
            {
                deadEnemies.Add(enemy);
            }
        }
        if (deadEnemies.Count == 0) return new List<BaseTarget>();

        int randIndex = Random.Range(0, deadEnemies.Count);
        return new List<BaseTarget> { deadEnemies[randIndex] };
    }
}
