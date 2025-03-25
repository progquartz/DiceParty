using System.Collections.Generic;
using UnityEngine;

public class HealEffect : BaseEffect
{
    public override void Effect(List<BaseTarget> targets, BaseTarget caller, int strength1, int strength2)
    {
        foreach (var target in targets)
        {
            BaseStat targetStat = target.stat;
            if (targetStat.isDead) continue;

            int totalHpDelta = strength1;
            targetStat.Hp += totalHpDelta;
            if (targetStat.Hp > targetStat.maxHp)
            {
                targetStat.Hp = targetStat.maxHp;
            }
        }
    }
}
