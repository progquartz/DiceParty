using System;
using System.Collections.Generic;
using UnityEngine;

public class TargetAllEnemy : BaseTargetOption
{
    public override List<BaseTarget> GetTarget(BaseTarget caller)
    {
        return FindAllEnemy();
    }

    private List<BaseTarget> FindAllEnemy()
    {
        List<BaseEnemy> enemies =  BattleManager.Instance.GetAllEnemys();
        return new List<BaseTarget>(enemies);
    }
}
