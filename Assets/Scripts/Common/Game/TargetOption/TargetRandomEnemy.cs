using System.Collections.Generic;
using UnityEngine;

public class TargetRandomEnemy : BaseTargetOption
{
    public override List<BaseTarget> GetTarget()
    {
        BaseEnemy[] enemies = GameObject.FindObjectsOfType<BaseEnemy>();
        if (enemies.Length == 0) return new List<BaseTarget>();

        int randIndex = Random.Range(0, enemies.Length);
        return new List<BaseTarget> { enemies[randIndex] };
    }
}
