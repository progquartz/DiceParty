using UnityEngine;

public class ShopEvent : RoomEvent
{
    public override void TriggerEvent()
    {
        Logger.Log($"[ShopEvent] - �̺�Ʈ Ȱ��ȭ");
        UIManager.Instance.OpenUI<ShopUI>(new BaseUIData
        {
            ActionOnShow = () => { Debug.Log("���� UI ����."); },
            ActionOnClose = () => { Debug.Log("���� UI ����."); }
        });
    }

    private void OnMouseDown()
    {
        MapManager.Instance.RequestPlayerMoveToEvent(this);
    }
}
