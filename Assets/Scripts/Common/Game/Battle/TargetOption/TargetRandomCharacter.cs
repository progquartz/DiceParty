using System.Collections.Generic;
using UnityEngine;

public class TargetRandomCharacter : BaseTargetOption
{
    public override List<BaseTarget> GetTarget(BaseTarget caller)
    {
        List<BaseCharacter> characters = BattleManager.Instance.GetAllCharacters();
        List<BaseCharacter> aliveCharacters = new List<BaseCharacter>();
        foreach (BaseCharacter character in characters)
        {
            if(!character.stat.isDead)
                aliveCharacters.Add(character);
        }
        if(aliveCharacters.Count == 0 ) return new List<BaseTarget>();

        int randIndex = Random.Range( 0, aliveCharacters.Count );
        return new List<BaseTarget> { aliveCharacters[randIndex] };
    }
}
