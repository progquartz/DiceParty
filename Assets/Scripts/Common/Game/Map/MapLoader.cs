using System.Collections.Generic;
using UnityEngine;


#region JSON ����ȭ Ŭ����
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
    // MapManager �ν��Ͻ� (Inspector���� �Ҵ��ϰų� �̱��� �ν��Ͻ��� ���)
    public MapManager mapManager;


    public void Init(MapManager manager)
    {
        mapManager = manager;
    }

    /// <summary>
    /// ���� MapManager�� �� �����͸� JSON ���ڿ��� �����մϴ�.
    /// </summary>
    public string SaveMapDataToJson()
    {
        MapData data = new MapData();
        data.rooms = new List<RoomData>();

        // MapManager ������ �� �����͸� ��ȸ
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
        Debug.Log("����� �� ������: " + json);
        return json;
    }

    /// <summary>
    /// JSON ���ڿ��� ����� �� �����͸� �ҷ��ͼ� ���� ������մϴ�.
    /// ���� ���� MapManager�� ResetMap()�� ȣ���� �ʱ�ȭ�մϴ�.
    /// </summary>
    public void LoadMapDataFromJson(string json)
    {
        // ���� ���� ���� �����մϴ�.
        mapManager.ResetMap();

        MapData data = JsonUtility.FromJson<MapData>(json);
        if (data == null || data.rooms == null)
        {
            Debug.LogError("�� �����͸� �ε��ϴ� �� �����߽��ϴ�. JSON ������ �ùٸ��� �ʽ��ϴ�.");
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
                Debug.LogWarning("�� �� ���� �� �̸��Դϴ�: " + rd.roomName);
            }
        }
    }
}
