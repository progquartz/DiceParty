using UnityEngine;

public class BossEvent : RoomEvent
{
    public override void TriggerEvent()
    {
        Logger.Log($"[BossEvent] - 이벤트 활성화");
    }

    private void OnMouseDown()
    {
        MapManager.Instance.RequestPlayerMoveToEvent(this);
    }
}
