using System.Collections.Generic;
using Unity.Android.Types;
using UnityEngine;

public class StunEffect : BaseEffect
{
    public override void Effect(List<BaseTarget> targets, BaseTarget caller, int strength1, int strength2)
    {
        foreach (var target in targets)
        {
            BaseStat targetStat = target.stat;
            int stunDelta = strength1;
            if(targetStat.HasEffect(EffectKey.ImmuneEffect))
            {
                targetStat.CalcEffectStack(EffectKey.ImmuneEffect, -1);
            }
            else
            {
                targetStat.CalcEffectStack(EffectKey.StunEffect, stunDelta);
            }
        }
    }
}
