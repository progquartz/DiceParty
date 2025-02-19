using UnityEngine;

public class BattleEvent : RoomEvent
{
    public override void TriggerEvent()
    {
        Logger.Log($"[BattleEvent] - 이벤트 활성화");
        LoadBattleEvent();
    }

    private void LoadBattleEvent()
    {
        BattleManager.Instance.StartBattlePhase(BattleManager.ConvertToStageType(MapManager.Instance.currentStageNum, 0));
        Debug.Log($"{BattleManager.ConvertToStageType(MapManager.Instance.currentStageNum, 0).ToString()}의 전투 가져옵니다.");
    }

    private void OnMouseDown()
    {
        MapManager.Instance.RequestPlayerMoveToEvent(this);
    }
}
