using System.Collections.Generic;
using UnityEngine;

public class AdditionalDiceEffect : BaseEffect
{
    public override void Effect(List<BaseTarget> targets, BaseTarget caller, int strength1, int strength2)
    {
        Logger.LogWarning("아직 개발되지 않은 AdditionalDice에 접근해 이를 사용했습니다.");
    }
}
