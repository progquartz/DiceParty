using System.Collections.Generic;
using UnityEngine;

public class FireEffect : BaseEffect
{
    public override void Effect(List<BaseTarget> targets, BaseTarget caller, int strength1, int strength2)
    {
        foreach (var target in targets)
        {
            BaseStat targetStat = target.stat;
            int fireDelta = strength1;
            if (targetStat.ImmuneStack > 0) // ��ȭ �� ����� ����.
            {
                targetStat.ImmuneStack--;
            }
            else
            {
                targetStat.FireStack += fireDelta;
            }
        }
    }
}
