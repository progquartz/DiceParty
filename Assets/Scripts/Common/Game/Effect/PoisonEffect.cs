using System.Collections.Generic;
using UnityEngine;

public class PoisonEffect : BaseEffect
{
    public override void Effect(List<BaseTarget> targets, int strength1, int strength2)
    {
        foreach (var target in targets)
        {
            BaseStat targetStat = target.stat;
            int totalHpDelta = strength1 + targetStat.AdditionalDamageStack;
            targetStat.Hp += totalHpDelta;
            if (targetStat.Hp > targetStat.maxHp)
            {
                targetStat.Hp = targetStat.maxHp;
            }
        }
    }
}
