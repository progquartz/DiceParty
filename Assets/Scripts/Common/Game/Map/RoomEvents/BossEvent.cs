using UnityEngine;

public class BossEvent : RoomEvent
{
    public override void TriggerEvent()
    {
        Logger.Log($"[BossEvent] - �̺�Ʈ Ȱ��ȭ");
    }

    private void OnMouseDown()
    {
        MapManager.Instance.RequestPlayerMoveToEvent(this);
    }
}
