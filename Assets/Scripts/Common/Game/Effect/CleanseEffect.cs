using System.Collections.Generic;
using UnityEngine;

public class CleanseEffect : BaseEffect
{
    public override void Effect(List<BaseTarget> targets, BaseTarget caller, int strength1, int strength2)
    {
        foreach (var target in targets)
        {
            BaseStat targetStat = target.stat;
            targetStat.Cleanse();
        }
    }
}
