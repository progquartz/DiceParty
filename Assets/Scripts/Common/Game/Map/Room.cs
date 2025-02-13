using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // ���� �׸��� ��ǥ �� �⺻ ����
    public Vector2Int gridPos;
    // 0 -> left / 1 -> Right / 2 -> top / 3 -> bottom
    public List<int> connectionDirection; 

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

    public void DeactivateRoomEvent()
    {
        RoomEvent objectEvent = GetComponent<RoomEvent>();
        Destroy(objectEvent);
        roomEvent = null;
    }
}
