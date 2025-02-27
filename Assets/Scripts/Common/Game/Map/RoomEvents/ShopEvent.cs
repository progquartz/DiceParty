using UnityEngine;

public class ShopEvent : RoomEvent
{
    public override void TriggerEvent()
    {
        Logger.Log($"[ShopEvent] - 이벤트 활성화");
    }

    private void OnMouseDown()
    {
        MapManager.Instance.RequestPlayerMoveToEvent(this);
    }
}
