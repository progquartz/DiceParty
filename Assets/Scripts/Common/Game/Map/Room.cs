using UnityEngine;
using System.Collections.Generic;
using System;

public class Room : MonoBehaviour
{
    // 방의 그리드 좌표 및 기본 정보
    public Vector2Int gridPos;
    // 0 -> left / 1 -> Right / 2 -> top / 3 -> bottom
    public List<int> connectionDirection;

    // 방 이름 정보 (JSON 저장/로드를 위해 추가)
    public string roomName;

    // 이 방에서 발생할 이벤트 (전투 등이 없으면 null 값)
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



