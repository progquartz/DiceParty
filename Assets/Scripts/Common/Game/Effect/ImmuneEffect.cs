using System.Collections.Generic;
using UnityEngine;

public class ImmuneEffect : BaseEffect
{
    public override void Effect(List<BaseTarget> targets, BaseTarget caller, int strength1, int strength2)
    {
        foreach (var target in targets)
        {
            BaseStat targetStat = target.stat;
            int immuneDelta = strength1;
            targetStat.CalcEffectStack(EffectKey.ImmuneEffect, immuneDelta);
        }
    }
}
