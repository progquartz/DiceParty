using UnityEngine;

public class BattleEvent : RoomEvent
{
    public override void TriggerEvent()
    {
        Logger.Log($"[BattleEvent] - 이벤트 활성화");
        base.CloseMapUI();
        LoadBattleEvent();
    }

    private void LoadBattleEvent()
    {
        BattleManager.Instance.StartBattlePhase(BattleManager.ConvertToBattleType(MapManager.Instance.currentStageNum, 0));
        Debug.Log($"{BattleManager.ConvertToBattleType(MapManager.Instance.currentStageNum, 0).ToString()}로 전투 시작됩니다.");
    }

    private void OnMouseDown()
    {
        MapManager.Instance.RequestPlayerMoveToEvent(this);
    }
}
