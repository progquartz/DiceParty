using System.Collections.Generic;
using UnityEngine;


#region JSON 직렬화 클래스
[System.Serializable]
public class RoomData
{
    public int x;
    public int y;
    public string roomName;
}

[System.Serializable]
public class MapData
{
    public List<RoomData> rooms;
}
#endregion


public class MapLoader : MonoBehaviour
{
    // MapManager 인스턴스 (Inspector에서 할당하거나 싱글톤 인스턴스를 사용)
    public MapManager mapManager;


    public void Init(MapManager manager)
    {
        mapManager = manager;
    }

    /// <summary>
    /// 현재 MapManager의 맵 데이터를 JSON 문자열로 저장합니다.
    /// </summary>
    public string SaveMapDataToJson()
    {
        MapData data = new MapData();
        data.rooms = new List<RoomData>();

        // MapManager 내부의 맵 데이터를 순회
        foreach (var kvp in mapManager.GetMapRooms())
        {
            Vector2Int pos = kvp.Key;
            GameObject roomObj = kvp.Value;
            Room roomComp = roomObj.GetComponent<Room>();
            if (roomComp != null)
            {
                RoomData rd = new RoomData();
                rd.x = pos.x;
                rd.y = pos.y;
                rd.roomName = roomComp.roomName;
                data.rooms.Add(rd);
            }
        }
        string json = JsonUtility.ToJson(data, true);
        Debug.Log("저장된 맵 데이터: " + json);
        return json;
    }

    /// <summary>
    /// JSON 문자열로 저장된 맵 데이터를 불러와서 맵을 재구성합니다.
    /// 먼저 현재 MapManager의 ResetMap()을 호출해 초기화합니다.
    /// </summary>
    public void LoadMapDataFromJson(string json)
    {
        // 현재 맵을 먼저 초기화합니다.
        mapManager.ResetMap();

        MapData data = JsonUtility.FromJson<MapData>(json);
        if (data == null || data.rooms == null)
        {
            Debug.LogError("맵 데이터를 로드하는 중 실패했습니다. JSON 형식이 올바르지 않습니다.");
            return;
        }

        foreach (RoomData rd in data.rooms)
        {
            Vector2Int pos = new Vector2Int(rd.x, rd.y);
            GameObject prefab = mapManager.GetPrefabForRoomName(rd.roomName);
            if (prefab != null)
            {
                GameObject newRoom = Instantiate(prefab, mapManager.GetWorldPositionForGrid(pos), Quaternion.identity, mapManager.RoomParent);
                Room roomComp = newRoom.GetComponent<Room>();
                if (roomComp == null)
                {
                    roomComp = newRoom.AddComponent<Room>();
                }
                roomComp.gridPos = pos;
                roomComp.roomName = rd.roomName;
                mapManager.RegisterRoom(pos, newRoom);
            }
            else
            {
                Debug.LogWarning("알 수 없는 방 이름입니다: " + rd.roomName);
            }
        }
    }
}
