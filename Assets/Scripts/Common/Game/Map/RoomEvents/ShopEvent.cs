using UnityEngine;

public class ShopEvent : RoomEvent
{
    public override void TriggerEvent()
    {
        Logger.Log($"[ShopEvent] - �̺�Ʈ Ȱ��ȭ");
    }

    private void OnMouseDown()
    {
        MapManager.Instance.RequestPlayerMoveToEvent(this);
    }
}
