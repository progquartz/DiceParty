using System.Collections.Generic;
using UnityEngine;

public class TargetRandomCharacter : BaseTargetOption
{
    public override List<BaseTarget> GetTarget(BaseTarget caller)
    {
        List<BaseCharacter> characters = BattleManager.Instance.GetAllCharacters();
        if(characters.Count == 0 ) return new List<BaseTarget>();

        int randIndex = Random.Range( 0, characters.Count );
        return new List<BaseTarget> { characters[randIndex] };
    }
}
