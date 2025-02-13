using UnityEngine;

public class ShopEvent : RoomEvent
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
