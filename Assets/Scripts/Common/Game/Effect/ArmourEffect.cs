using System.Collections.Generic;
using UnityEngine;

public class ArmourEffect : BaseEffect
{
    public override void Effect(List<BaseTarget> targets, int strength1, int strength2)
    {
        foreach (var target in targets)
        {
            BaseStat targetStat = target.stat;
            int ArmourDelta = strength1;
            targetStat.ArmourStack += ArmourDelta;
        }
    }
}
