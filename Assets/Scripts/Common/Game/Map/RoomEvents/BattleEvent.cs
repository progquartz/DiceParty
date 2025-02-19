using UnityEngine;

public class BattleEvent : RoomEvent
{
    public override void TriggerEvent()
    {
        Logger.Log($"[BattleEvent] - �̺�Ʈ Ȱ��ȭ");
        LoadBattleEvent();
    }

    private void LoadBattleEvent()
    {
        BattleManager.Instance.StartBattlePhase(BattleManager.ConvertToStageType(MapManager.Instance.currentStageNum, 0));
        Debug.Log($"{BattleManager.ConvertToStageType(MapManager.Instance.currentStageNum, 0).ToString()}�� ���� �����ɴϴ�.");
    }

    private void OnMouseDown()
    {
        MapManager.Instance.RequestPlayerMoveToEvent(this);
    }
}
