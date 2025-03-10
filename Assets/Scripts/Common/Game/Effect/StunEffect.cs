using System.Collections.Generic;
using UnityEngine;

public class StunEffect : BaseEffect
{
    public override void Effect(List<BaseTarget> targets, BaseTarget caller, int strength1, int strength2)
    {
        foreach (var target in targets)
        {
            BaseStat targetStat = target.stat;
            int stunDelta = strength1;
            if(targetStat.ImmuneStack > 0)
            {
                targetStat.ImmuneStack--;
            }
            else
            {
                targetStat.StunnedStack += stunDelta;
            }
        }
    }
}
