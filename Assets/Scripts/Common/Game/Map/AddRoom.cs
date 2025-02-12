using System;
using UnityEngine;

public class AddRoom : MonoBehaviour
{
    [SerializeField] private RoomSpawner[] roomSpawners;

    void Start()
    {
        // 방에 Room 컴포넌트가 없다면 추가
        Room room = GetComponent<Room>();
        if (room == null)
        {
            room = gameObject.AddComponent<Room>();
            // 시작방이라면 (0,0) 등 기본 좌표를 설정할 수 있음.
            // 예시로 시작방이면 gridPos를 (0,0)으로 설정합니다.
            room.gridPos = Vector2Int.zero;
        }

        // 방 리스트에 추가
        MapManager.Instance.rooms.Add(gameObject);

        // 좌표가 사전에 등록되어 있지 않다면 등록 (이미 등록되어 있다면 경고)
        if (!MapManager.Instance.IsRoomExistAt(room.gridPos))
        {
            MapManager.Instance.RegisterRoom(room.gridPos);
        }

        // 시작방이면 플레이어의 현재 방 좌표로 등록
        if (room.gridPos == Vector2Int.zero)
        {
            MapManager.Instance.currentPlayerRoom = room.gridPos;
        }

        foreach(var roomSpawner in roomSpawners)
        {
            roomSpawner.Init();
        }

    }
}
