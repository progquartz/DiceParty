using System.Collections.Generic;
using UnityEngine;

public class TargetAllCharacter : BaseTargetOption
{
    public override List<BaseTarget> GetTarget(BaseTarget caller)
    {
        List<BaseCharacter> characters = BattleManager.Instance.GetCharacterList();
        List<BaseCharacter> aliveCharacters = new List<BaseCharacter>();
        foreach (BaseCharacter character in characters)
        {
            if (!character.stat.isDead)
            {
                aliveCharacters.Add(character);
            }
        }
        return new List<BaseTarget>(aliveCharacters);
    }
}
