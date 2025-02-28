using System.Collections.Generic;
using UnityEngine;

public class TargetAllCharacterDead : BaseTargetOption
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
        return new List<BaseTarget>(deadEnemies);
    }
}
