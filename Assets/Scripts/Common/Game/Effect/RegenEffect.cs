using System.Collections.Generic;
using UnityEngine;

public class RegenEffect : BaseEffect
{
    public override void Effect(List<BaseTarget> targets, BaseTarget caller, int strength1, int strength2)
    {
        foreach (var target in targets)
        {
            BaseStat targetStat = target.stat;
            int regenDelta = strength1;
            targetStat.RegenStack += regenDelta;
        }
    }
}
