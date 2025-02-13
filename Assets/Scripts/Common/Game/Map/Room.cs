using NUnit.Framework;
using System.Collections.Generic;
using UnityEngine;

public class Room : MonoBehaviour
{
    // 방의 그리드 좌표 등 기본 정보
    public Vector2Int gridPos;
    // 0 -> left / 1 -> Right / 2 -> top / 3 -> bottom
    public List<int> connectionDirection; 

    // 이 방에서 발생할 이벤트 (없을 수도 있으므로 null 허용)
    public RoomEvent roomEvent;

    // 플레이어가 이 방에 들어왔을 때 호출될 메서드 (예: OnPlayerEnter)
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
