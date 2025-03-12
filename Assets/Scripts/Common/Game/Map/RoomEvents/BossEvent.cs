using UnityEngine;

public class BossEvent : RoomEvent
{
    public override void TriggerEvent()
    {
        Logger.Log($"[BossEvent] - 이벤트 활성화");
        base.CloseMapUI();
        LoadBossBattleEvent();
    }

    private void LoadBossBattleEvent()
    {
        BattleManager.Instance.StartBattlePhase(BattleManager.ConvertToStageType(MapManager.Instance.currentStageNum, 2));
    }

    private void OnMouseDown()
    {
        MapManager.Instance.RequestPlayerMoveToEvent(this);
    }
}
