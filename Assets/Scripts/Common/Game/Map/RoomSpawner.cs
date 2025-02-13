using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    // 새로 만들어질 친구의 입구가 ...
    // 1: 아래, 2: 위, 3: 왼쪽, 4: 오른쪽 (출구 방향)
    public int openingDirection;

    private RoomTemplates templates;
    private int rand;
    public bool spawned = false;
    public float waitTime = 4f;

    public void Init()
    {
        // 일정 시간이 지나면 spawner 오브젝트 자체를 삭제
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
        if (MapManager.Instance.rooms.Count >= MapManager.Instance.maxRooms)
        {
            MapManager.Instance.OnMapRoomCountFull();
            // 만약 최대 방 개수에 도달했을 경우, 입구가 1개만 있어서 더 소환이 되지 않는 방 선택.
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
            // 배열의 1~마지막 인덱스는 2개 이상의 입구가 있는 경우. 방을 늘려야 할 경우 사용.
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

        // MapManager에 새 방의 좌표 등록
        MapManager.Instance.RegisterRoom(spawnPos);

        spawned = true;
    }

    /*
    // 다른 RoomSpawner와 충돌 시 처리 (중복 생성 방지 및 방 간 연결 정보 등록)
    void OnTriggerEnter2D(Collider2D other)
    {
        // RoomSpawner가 부착된 다른 오브젝트와 충돌했을 때 처리합니다.
        Room otherRoom = other.GetComponent<Room>();
        if (otherRoom != null)
        {
            Room currentRoom = GetComponentInParent<Room>();
            if (currentRoom != null && otherRoom != null)
            {
                // 두 방이 실제로 충돌하여 연결되었음을 MapManager에 등록합니다.
                MapManager.Instance.RegisterConnection(currentRoom.gridPos, otherRoom.gridPos);
            }
            spawned = true;
        }
    }
    */
}
