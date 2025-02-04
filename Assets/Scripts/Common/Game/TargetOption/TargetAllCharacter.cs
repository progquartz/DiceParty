using System.Collections.Generic;
using UnityEngine;

public class TargetAllCharacter : BaseTargetOption
{
    public override List<BaseTarget> GetTarget()
    {
        return FindAllEnemy();
    }

    private List<BaseTarget> FindAllEnemy()
    {
        List<BaseCharacter> characters = BattleManager.Instance.GetAllCharacters();
        return new List<BaseTarget>(characters);
    }
}
