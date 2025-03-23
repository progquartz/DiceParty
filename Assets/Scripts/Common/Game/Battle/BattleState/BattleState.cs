using UnityEngine;

public abstract class BattleState : MonoBehaviour
{
    protected BattleManager battleManager;

    public BattleState(BattleManager battleManager)
    {
        this.battleManager = battleManager;
    }

    // 상태 진입 시 호출
    public abstract void Enter();

    // 상태 종료 시 호출
    public abstract void Exit();

    // 상태 업데이트
    public virtual void Update() { }

    // 상태별 처리가 필요한 이벤트들
    public virtual void OnTargetDied(BaseTarget target) { }
    public virtual void OnSkillExecuted(BaseTarget executor, BaseTarget target) { }
    public virtual void OnDiceRolled() { }
}
