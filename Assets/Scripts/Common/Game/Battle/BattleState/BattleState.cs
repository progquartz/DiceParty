using UnityEngine;

public abstract class BattleState : MonoBehaviour
{
    protected BattleManager battleManager;

    public BattleState(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    // ���� ���� �� ȣ��
    public abstract void Enter();

    // ���� ���� �� ȣ��
    public abstract void Exit();

    // ���º� ó���� �ʿ��� �̺�Ʈ��
    public virtual void OnTargetDied(BaseTarget target) { }
    public virtual void OnSkillExecuted(BaseTarget executor, BaseTarget target) { }
    public virtual void OnDiceRolled() { }

    // ���� ������Ʈ
    public virtual void Update() { }

}
