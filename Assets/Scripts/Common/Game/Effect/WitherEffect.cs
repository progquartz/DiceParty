using System.Collections.Generic;
using UnityEngine;

public class WitherEffect : BaseEffect
{
    public override void Effect(List<BaseTarget> targets, BaseTarget caller, int strength1, int strength2)
    {
        foreach (var target in targets)
        {
            BaseStat targetStat = target.stat;
            if (targetStat.isDead) continue;

            int witherDelta = strength1;
            targetStat.CalcEffectStack(EffectKey.WitherEffect, witherDelta);
        }
    }

}
