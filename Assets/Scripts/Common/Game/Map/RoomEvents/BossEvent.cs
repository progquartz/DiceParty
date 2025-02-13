using UnityEngine;

public class BossEvent : RoomEvent
{
    public override void TriggerEvent()
    {
        throw new System.NotImplementedException();
    }

    private void OnMouseDown()
    {
        MapManager.Instance.RequestPlayerMoveToEvent(this);
    }
}
