using System.Collections.Generic;
using UnityEngine;

public class TargetRandomEnemy : BaseTargetOption
{
    public override List<BaseTarget> GetTarget(BaseTarget caller)
    {
        List<BaseEnemy> enemies = BattleManager.Instance.GetAllEnemys();
        if (enemies.Count == 0) return new List<BaseTarget>();

        int randIndex = Random.Range(0, enemies.Count);
        return new List<BaseTarget> { enemies[randIndex] };
    }
}
