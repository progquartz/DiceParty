using UnityEngine;

public class HealEffect : BaseEffect
{
    public override void Effect(BaseTarget[] targets, int strength)
    {
        foreach (var target in targets)
        {
            int totalHpDelta = strength + target.AdditionalDamageStack;
            target.Hp += totalHpDelta;
            if (target.Hp > target.maxHp)
            {
                target.Hp = target.maxHp;
            }
        }
    }
}
