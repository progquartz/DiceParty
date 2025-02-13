using UnityEngine;

public class TreasureEvent : RoomEvent
{
    public override void TriggerEvent()
    {
        Logger.Log($"[TreasureEvent] - �̺�Ʈ Ȱ��ȭ");
    }

    private void OnMouseDown()
    {
        MapManager.Instance.RequestPlayerMoveToEvent(this);
    }
}
