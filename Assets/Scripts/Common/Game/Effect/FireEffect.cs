using System.Collections.Generic;
using UnityEngine;

public class FireEffect : BaseEffect
{
    public override void Effect(List<BaseTarget> targets, BaseTarget caller, int strength1, int strength2)
    {
        foreach (var target in targets)
        {
            BaseStat targetStat = target.stat;
            if (targetStat.isDead) continue;

            int fireDelta = strength1;
            if(targetStat.HasEffect(EffectKey.ImmuneEffect))
            {
                targetStat.CalcEffectStack(EffectKey.ImmuneEffect, -1);
            }
            else
            {
                targetStat.CalcEffectStack(EffectKey.FireEffect, fireDelta);
            }
        }
    }
}
