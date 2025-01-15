using System.Collections.Generic;
using UnityEngine;

public class DamageEffect : BaseEffect
{

    public override void Effect(List<BaseTarget> targets, int strength)
    {
        foreach (var target in targets)
        {
            int totalHpDelta = strength + target.AdditionalDamageStack;


            if (target.Armour > 0)
            {
                target.Armour -= totalHpDelta;
                if (target.Armour < 0)
                {
                    totalHpDelta = -target.Armour;
                    target.Hp -= totalHpDelta;
                    if (target.Hp < 0)
                    {
                        target.Hp = 0;
                        BattleManager.Instance.DeathCheck();
                    }
                }
            }
        }
    }
}
