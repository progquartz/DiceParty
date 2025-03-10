using System.Collections.Generic;
using UnityEngine;

public class CleanseEffect : BaseEffect
{
    public override void Effect(List<BaseTarget> targets, BaseTarget caller, int strength1, int strength2)
    {
        foreach (var target in targets)
        {
            BaseStat targetStat = target.stat;
            targetStat.ConfuseStack = 0;
            targetStat.FireStack = 0;
            targetStat.PoisonStack = 0;
            targetStat.StunnedStack = 0;
            targetStat.TauntStack = 0;
            targetStat.WeakenStack = 0;
            targetStat.WitherStack = 0;
        }
    }
}
