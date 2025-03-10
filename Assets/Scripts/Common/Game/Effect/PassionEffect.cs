using System.Collections.Generic;
using UnityEngine;

public class PassionEffect : BaseEffect
{
    public override void Effect(List<BaseTarget> targets, BaseTarget caller, int strength1, int strength2)
    {
        foreach (var target in targets)
        {
            BaseStat targetStat = target.stat;
            int passionDelta = strength1;
            targetStat.PassionStack += passionDelta;
        }
    }
}
