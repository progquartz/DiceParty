using System.Collections.Generic;
using UnityEngine;

public class ConfuseEffect : BaseEffect
{
    public override void Effect(List<BaseTarget> targets, BaseTarget caller, int strength1, int strength2)
    {
        foreach (var target in targets)
        {
            BaseStat targetStat = target.stat;
            
            if(targetStat.ImmuneStack > 0) // 정화 시 디버프 삭제.
            {
                targetStat.ImmuneStack--;
            }
            else
            {
                int ConfuseDelta = strength1;
                targetStat.ConfuseStack += ConfuseDelta;
            }
        }
    }
}
