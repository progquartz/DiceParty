using UnityEngine;

public enum CharacterType
{
    Knight = 0,
    Rogue = 1,
    Magician = 2,
    Priest = 3,
    Neutral = 4,
}

public class BaseCharacter :BaseTarget
{
    public CharacterType CharacterType;

    private void Awake()
    {
        Init();
    }

    public void Init()
    {
        base.Init();
        RegisterEvents();
    }

    private void RegisterEvents()
    {
        BattleManager.Instance.OnPlayerTurnStart += OnPlayerTurnStart;
        BattleManager.Instance.OnPlayerTurnEnd += OnPlayerTurnEnd;
        BattleManager.Instance.OnBattleEnd += stat.OnBattleEnd;
    }

    private void ReleaseEvents()
    {
        BattleManager.Instance.OnPlayerTurnStart -= OnPlayerTurnStart;
        BattleManager.Instance.OnPlayerTurnEnd -= OnPlayerTurnEnd;
        BattleManager.Instance.OnBattleEnd -= stat.OnBattleEnd;
    }

    /// <summary>
    /// 적 턴 시작 
    /// </summary>
    private void OnPlayerTurnStart()
    {
        // 턴 시작 / 턴 별 효과 계산.
        base.EffectCalcOnTurnStart();
    }

    /// <summary>
    /// 적 턴 끝
    /// </summary>
    private void OnPlayerTurnEnd()
    {
        base.EffectCalcOnTurnEnd();
    }
}
