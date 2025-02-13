using UnityEngine;

public abstract class RoomEvent : MonoBehaviour
{
    public abstract void TriggerEvent();

    public Room Room { get; set; }

    private void OnMouseDown()
    {
        MapManager.Instance.RequestPlayerMoveToEvent(this);
    }
}
