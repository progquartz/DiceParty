using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    // 방의 출입구가 친구를 찾기를 ...
    // 1: 아래, 2: 위, 3: 오른쪽, 4: 왼쪽 (출구 방향)
    public int openingDirection;

    private RoomTemplates templates;
    private int rand;
    public bool spawned = false;
    public float waitTime = 4f;

    public void Init()
    {
        // 대기 시간이 지나면 spawner 오브젝트 자체를 제거
        Destroy(gameObject, waitTime);
        templates = MapManager.Instance.RoomTemplate;
        Spawn();
    }

    void Spawn()
    {
        if (spawned) return;

        // 부모 방의 Room 컴포넌트에서 현재 좌표를 가져옵니다.
        Room parentRoom = GetComponentInParent<Room>();
        if (parentRoom == null)
        {
            Debug.LogError("RoomSpawner의 부모에 Room 컴포넌트가 없습니다!");
            return;
        }
        Vector2Int parentPos = parentRoom.gridPos;

        // 출구 방향에 따른 좌표 오프셋 계산
        Vector2Int offset = Vector2Int.zero;
        switch (openingDirection)
        {
            case 1: // 위
                offset = new Vector2Int(0, 1);
                break;
            case 2: // 아래
                offset = new Vector2Int(0, -1);
                break;
            case 3: // 오른쪽
                offset = new Vector2Int(1, 0);
                break;
            case 4: // 왼쪽
                offset = new Vector2Int(-1, 0);
                break;
            default:
                Debug.LogError("올바르지 않은 openingDirection 값!");
                break;
        }
        Vector2Int spawnPos = parentPos + offset;

        GameObject roomPrefab = null;

        // 최대 방 개수 제한 체크
        if (MapManager.Instance.RoomCount >= MapManager.Instance.maxRooms)
        {
            MapManager.Instance.OnMapRoomCountFull();
            // 만약 최대 방 개수에 도달했을 경우, 출구가 1개만 있어 더 생성되지 않는 방 생성.
            if (openingDirection == 1)
            {
                roomPrefab = templates.BottomRooms[0];
            }
            else if (openingDirection == 2)
            {
                roomPrefab = templates.TopRooms[0];
            }
            else if (openingDirection == 3)
            {
                roomPrefab = templates.LeftRooms[0];
            }
            else if (openingDirection == 4)
            {
                roomPrefab = templates.RightRooms[0];
            }
        }
        else
        {
            // 배열의 1~끝까지 인덱스는 2개 이상의 출구가 있는 방임. 맵을 플레이 할 수도 있음.
            if (openingDirection == 1)
            {
                rand = Random.Range(1, templates.BottomRooms.Length);
                roomPrefab = templates.BottomRooms[rand];
            }
            else if (openingDirection == 2)
            {
                rand = Random.Range(1, templates.TopRooms.Length);
                roomPrefab = templates.TopRooms[rand];
            }
            else if (openingDirection == 3)
            {
                rand = Random.Range(1, templates.LeftRooms.Length);
                roomPrefab = templates.LeftRooms[rand];
            }
            else if (openingDirection == 4)
            {
                rand = Random.Range(1, templates.RightRooms.Length);
                roomPrefab = templates.RightRooms[rand];
            }
        }

        // 이미 해당 좌표에 방이 생성되었는지 확인
        if (MapManager.Instance.IsRoomExistAt(spawnPos))
        {
            spawned = true;
            return;
        }

        // 새 방 생성
        GameObject newRoom = Instantiate(roomPrefab, transform.position, roomPrefab.transform.rotation, MapManager.Instance.RoomParent);

        // 새 방에 Room 컴포넌트가 없다면 추가하고, 그리드 좌표 설정
        Room newRoomComp = newRoom.GetComponent<Room>();
        if (newRoomComp == null)
        {
            newRoomComp = newRoom.AddComponent<Room>();
        }
        newRoomComp.gridPos = spawnPos;

        // MapManager에 새 방과 좌표 등록
        MapManager.Instance.RegisterRoom(spawnPos, newRoom);

        spawned = true;
    }
}
