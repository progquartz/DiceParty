using System.Collections.Generic;
using UnityEngine;

public class TargetRandomCharacterDead : BaseTargetOption
{
    public override List<BaseTarget> GetTarget(BaseTarget caller)
    {
        List<BaseCharacter> characters = BattleManager.Instance.GetCharacterList();
        List<BaseCharacter> deadCharacters = new List<BaseCharacter>();
        foreach (BaseCharacter character in characters)
        {
            if (character.stat.isDead)
                deadCharacters.Add(character);
        }
        if (deadCharacters.Count == 0) return new List<BaseTarget>();

        int randIndex = Random.Range(0, deadCharacters.Count);
        return new List<BaseTarget> { deadCharacters[randIndex] };
    }
}
