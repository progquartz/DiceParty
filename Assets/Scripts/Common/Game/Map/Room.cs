using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class RoomData
{
    public string roomName;        // "B", "EntryRoom", "L" ��
    public Vector2Int gridPosition;  // ���� �׸��� ��ǥ
    public bool connectLeft;
    public bool connectRight;
    public bool connectTop;
    public bool connectBottom;
}

public class Room : MonoBehaviour
{
    // ���� �׸��� ��ǥ �� �⺻ ����
    public Vector2Int gridPos;
    // 0 -> left / 1 -> Right / 2 -> top / 3 -> bottom
    public List<int> connectionDirection;

    // �� �̸� ������ (JSON ����/�ε带 ���� �߰�)
    public string roomName;

    // �� �濡�� �߻��� �̺�Ʈ (���� ���� �����Ƿ� null ���)
    public RoomEvent roomEvent;

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



