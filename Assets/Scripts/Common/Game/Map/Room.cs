using UnityEngine;
using System.Collections.Generic;
using System;

[Serializable]
public class RoomData
{
    public string roomName;        // "B", "EntryRoom", "L" 등
    public Vector2Int gridPosition;  // 방의 그리드 좌표
    public bool connectLeft;
    public bool connectRight;
    public bool connectTop;
    public bool connectBottom;
}

public class Room : MonoBehaviour
{
    // 방의 그리드 좌표 등 기본 정보
    public Vector2Int gridPos;
    // 0 -> left / 1 -> Right / 2 -> top / 3 -> bottom
    public List<int> connectionDirection;

    // 방 이름 데이터 (JSON 저장/로드를 위해 추가)
    public string roomName;

    // 이 방에서 발생할 이벤트 (없을 수도 있으므로 null 허용)
    public RoomEvent roomEvent;

    public void ActivateRoom()
    {
        if (roomEvent != null)
        {
            roomEvent.TriggerEvent();
        }
        else
        {
            Debug.Log("이 방에는 특별한 이벤트가 없습니다.");
        }
    }

    public void DeactivateRoomEvent()
    {
        RoomEvent objectEvent = GetComponent<RoomEvent>();
        Destroy(objectEvent);
        roomEvent = null;
    }
}



