using UnityEngine;

public class BattleManager : SingletonBehaviour<BattleManager>
{
    
    public void AmplifyEffect(BaseTarget[] targets, EffectBase effect)
    {
        effect.Effect(targets);
    }


}
