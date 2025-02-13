using UnityEngine;

public class BattleEvent : RoomEvent
{
    public override void TriggerEvent()
    {
        Logger.Log($"[BattleEvent] - �̺�Ʈ Ȱ��ȭ");
    }

    private void OnMouseDown()
    {
        MapManager.Instance.RequestPlayerMoveToEvent(this);
    }
}
