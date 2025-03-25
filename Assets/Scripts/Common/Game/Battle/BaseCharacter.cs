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
    /// �� �� ���� 
    /// </summary>
    private void OnPlayerTurnStart()
    {
        // �� ���� / �� �� ȿ�� ���.
        base.EffectCalcOnTurnStart();
    }

    /// <summary>
    /// �� �� ��
    /// </summary>
    private void OnPlayerTurnEnd()
    {
        base.EffectCalcOnTurnEnd();
    }
}
