using UnityEngine;

public class TreasureEvent : RoomEvent
{
    public override void TriggerEvent()
    {
        Logger.Log($"[TreasureEvent] - 이벤트 활성화");
    }

    private void OnMouseDown()
    {
        MapManager.Instance.RequestPlayerMoveToEvent(this);
    }
}
