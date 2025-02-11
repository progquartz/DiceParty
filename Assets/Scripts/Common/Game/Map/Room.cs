using UnityEngine;

public class Room : MonoBehaviour
{
    // ���� �׸��� ��ǥ �� �⺻ ����
    public Vector2Int gridPos;

    // �� �濡�� �߻��� �̺�Ʈ (���� ���� �����Ƿ� null ���)
    public RoomEvent roomEvent;

    // �÷��̾ �� �濡 ������ �� ȣ��� �޼��� (��: OnPlayerEnter)
    public void ActivateRoom()
    {
        if (roomEvent != null)
        {
            roomEvent.TriggerEvent();
        }
        else
        {
            Debug.Log("�� �濡�� Ư���� �̺�Ʈ�� �����ϴ�.");
        }
    }
}
