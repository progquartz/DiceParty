using UnityEngine;

public class ShopEvent : RoomEvent
{
    public override void TriggerEvent()
    {
        Logger.Log($"[ShopEvent] - 이벤트 활성화");
        UIManager.Instance.OpenUI<ShopUI>(new BaseUIData
        {
            ActionOnShow = () => { Debug.Log("상점 UI 열림."); },
            ActionOnClose = () => { Debug.Log("상점 UI 닫힘."); }
        });
    }

    private void OnMouseDown()
    {
        MapManager.Instance.RequestPlayerMoveToEvent(this);
    }
}
