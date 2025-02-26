using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.Rendering;

public class MapManager : SingletonBehaviour<MapManager>
{
    public int currentStageNum = 1;


    [SerializeField] private RoomTemplates _roomTemplate;
    private MapLoader mapLoader;
    [SerializeField] private Transform roomParent;
    [SerializeField] private GameObject entryRoomPrefab; // EntryRoom 프리팹 (Inspector에 할당)
    


    // 각 방의 그리드 좌표를 기록 (Vector2Int 사용)
    [SerializeField]private Dictionary<Vector2Int, GameObject> mapGenRooms = new Dictionary<Vector2Int, GameObject>();
    [SerializeField] private Dictionary<Vector2Int, GameObject> mapGenVisuals = new Dictionary<Vector2Int, GameObject>();

    // 키: 해당 방의 gridPos, 값: 연결된 방의 gridPos 목록
    private Dictionary<Vector2Int, List<Vector2Int>> roomGraph = new Dictionary<Vector2Int, List<Vector2Int>>();

    public int RoomCount { get { return mapGenRooms.Count; } }
    public GameObject startRoom;

    // 최대 방 개수 (원하는 값으로 설정)
    public int maxRooms = 10;
    private bool isRoomReachedMax;
    public float waitTime;
    private bool spawnedBoss;
    private int currentRoomGenerating = 0;

    // 플레이어의 현재 위치 (시작방은 보통 (0,0)으로 설정)
    public Vector2Int currentPlayerPos = Vector2Int.zero;
    [SerializeField] private GameObject player;
    public float playerMoveSpeed = 13f;

    [SerializeField] private int battleRoomCount;
    [SerializeField] private int bossRoomCount;
    [SerializeField] private int treasureRoomCount;
    [SerializeField] private int shopRoomCount;

    private bool isEventPlaced = false;
    private bool isPlayerMoving = false;

    public float debugEventPlacingTime = 0.0f;
    private void Awake()
    {
        // 씬 전환 시 삭제되지 않도록 (원하는 경우)
        IsDestroyOnLoad = true;
        Init();
        
    }

    void Update()
    {
        // 0,0에서 가장 먼 좌표에 보스 방 생성.

        // 랜덤한 방에 대애충 다른 방들 넣기.
        // 추가: 모든 방 생성이 완료되면 데이터를 저장하거나 다른 로직 처리 가능
        //음...
        if(!isEventPlaced)
        {
            debugEventPlacingTime += Time.deltaTime;
        }
        if(!isEventPlaced && isRoomReachedMax)
        {
            bool isObjectDataValid = true;
            for(int i = 0 ; i < mapGenRooms.Values.Count; i++)
            {
                if (mapGenRooms.Values.ToList()[i] == null)
                {
                    Debug.LogError($"{i}번째 데이터에 오류 있음.");
                    isObjectDataValid = false;
                }
            }
            if(isObjectDataValid)
            {
                PlaceRandomEvents();
                isEventPlaced = true;
                Debug.Log($"이벤트를 모두 설치하기까지 {debugEventPlacingTime}이 걸립니다.");
                debugEventPlacingTime = 0f;
            }
        }
        
    }

    protected override void Init()
    {
        base.Init();
        InitializeData();
    }

    private void InitializeData()
    {
        // 필요한 초기화 작업 구현 (예: 기존 방 데이터 초기화)
        mapLoader = gameObject.AddComponent<MapLoader>();
        mapLoader.Init(this);
    }

    public Transform RoomParent { get { return roomParent; } }

    public RoomTemplates RoomTemplate
    {
        get
        {
            if (_roomTemplate == null)
            {
                _roomTemplate = gameObject.GetComponent<RoomTemplates>();
            }
            return _roomTemplate;
        }
    }

    // 주어진 좌표에 방이 이미 존재하는지 확인
    public bool IsRoomExistAt(Vector2Int pos)
    {
        return mapGenRooms.ContainsKey(pos);
    }

    // 맵 리셋 기능: 현재 생성된 방들을 삭제하고 내부 데이터를 초기화한 후 시작방(EntryRoom) 생성
    public void ResetMap()
    {
        // roomParent 하위의 모든 방(GameObject) 삭제
        foreach (Transform child in roomParent)
        {
            Destroy(child.gameObject);
        }
        mapGenRooms.Clear();
        mapGenVisuals.Clear();
        roomGraph.Clear();
        isEventPlaced = false;
        isRoomReachedMax = false;

        // (필요한 다른 데이터들도 초기화)
        currentPlayerPos = Vector2Int.zero;

        // EntryRoom 프리팹으로 시작방 생성
        if (entryRoomPrefab != null)
        {
            Vector3 startWorldPos = GetWorldPositionForGrid(Vector2Int.zero);

            GameObject newEntryRoom = Instantiate(entryRoomPrefab, roomParent, false);
            newEntryRoom.transform.localPosition = startWorldPos;
            startRoom = newEntryRoom;
            Room roomComp = newEntryRoom.GetComponent<Room>();
            if (roomComp == null)
            {
                roomComp = newEntryRoom.AddComponent<Room>();
            }
            roomComp.gridPos = Vector2Int.zero;
            roomComp.roomName = "EntryRoom"; // 방 이름 지정 (저장/로드를 위해)
            // 시작방 설정
            currentPlayerPos = Vector2Int.zero;
        }
        else
        {
            Logger.LogError("EntryRoom 프리팹이 할당되어 있지 않습니다.");
        }
    }

    /// <summary>
    /// 외부에서 MapLoader가 내부 방 데이터를 접근할 수 있도록 Dictionary를 반환합니다.
    /// </summary>
    public Dictionary<Vector2Int, GameObject> GetMapRooms()
    {
        return mapGenRooms;
    }

    /// <summary>
    /// 그리드 좌표를 기준으로 월드 좌표를 계산 (방 크기에 맞게 수정)
    /// </summary>
    public Vector3 GetWorldPositionForGrid(Vector2Int gridPos)
    {
        float roomWidth = 10f;  // 예시 값
        float roomHeight = 10f; // 예시 값
        return new Vector3(gridPos.x * roomWidth, gridPos.y * roomHeight, 0f);
    }

    /// <summary>
    /// 저장된 방 이름에 따라 해당 프리팹을 반환합니다.
    /// (여기서는 RoomTemplates에 있는 배열을 활용하는 예시입니다.)
    /// </summary>
    public GameObject GetPrefabForRoomName(string roomName)
    {
        // 예시 매핑 (실제 프로젝트 상황에 맞게 수정)
        switch (roomName)
        {
            case "B":
                return _roomTemplate.BottomRooms.Length > 0 ? _roomTemplate.BottomRooms[0] : null;
            case "BaseRoom":
                return _roomTemplate.ClosedRoom;
            case "EntryRoom":
                return entryRoomPrefab;
            case "L":
                return _roomTemplate.LeftRooms.Length > 0 ? _roomTemplate.LeftRooms[0] : null;
            case "LB":
                return _roomTemplate.LeftRooms.Length > 1 ? _roomTemplate.LeftRooms[1] : null;
            case "LR":
                return _roomTemplate.LeftRooms.Length > 2 ? _roomTemplate.LeftRooms[2] : null;
            case "R":
                return _roomTemplate.RightRooms.Length > 0 ? _roomTemplate.RightRooms[0] : null;
            case "RB":
                return _roomTemplate.RightRooms.Length > 1 ? _roomTemplate.RightRooms[1] : null;
            case "T":
                return _roomTemplate.TopRooms.Length > 0 ? _roomTemplate.TopRooms[0] : null;
            case "TB":
                return _roomTemplate.TopRooms.Length > 1 ? _roomTemplate.TopRooms[1] : null;
            case "TL":
                return _roomTemplate.TopRooms.Length > 2 ? _roomTemplate.TopRooms[2] : null;
            case "TR":
                return _roomTemplate.TopRooms.Length > 3 ? _roomTemplate.TopRooms[3] : null;
            default:
                Debug.LogWarning("알 수 없는 방 이름: " + roomName);
                return null;
        }
    }

    // 방 좌표를 등록 (이미 등록되어 있으면 경고)
    public void RegisterRoom(Vector2Int pos, GameObject room)
    {
        if (!mapGenRooms.ContainsKey(pos))
        {
            mapGenRooms.Add(pos, room);
            CheckConnection(pos);
        }
        else
        {
            Debug.LogWarning($"좌표 {pos}에는 이미 방이 등록되어 있습니다.");
        }
    }

    public void OnMapRoomCountFull()
    {
        isRoomReachedMax = true;
    }

    /// <summary>
    /// bfs 기반 Pathfinding
    /// </summary>
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        // 이미 등록된 방 간 연결 정보 (roomGraph)를 사용
        if (!roomGraph.ContainsKey(start) || !roomGraph.ContainsKey(target))
        {
            Logger.LogWarning("pathfinding중 시작 또는 목표 방의 연결 정보가 없습니다.");
            return null;
        }

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        queue.Enqueue(start);
        cameFrom[start] = start;  // 시작점을 표시

        while (queue.Count > 0)
        {
            Vector2Int current = queue.Dequeue();
            if (current == target)
                break;

            if (roomGraph.ContainsKey(current))
            {
                foreach (Vector2Int neighbor in roomGraph[current])
                {
                    if (!cameFrom.ContainsKey(neighbor))
                    {
                        if (neighbor == target ||  mapGenRooms[neighbor].GetComponent<Room>().roomEvent == null)
                        {
                            queue.Enqueue(neighbor);
                            cameFrom[neighbor] = current;
                        }
                    }
                }
            }
        }

        // 경로가 없으면 null 리턴
        if (!cameFrom.ContainsKey(target))
        {
            Debug.LogWarning("목표 이벤트까지의 경로를 찾을 수 없습니다.");
            return null;
        }

        // 경로 재구성 (뒤에서부터 따라가서 역순으로 만든 후 reverse)
        List<Vector2Int> path = new List<Vector2Int>();
        Vector2Int currPos = target;
        while (currPos != start)
        {
            path.Add(currPos);
            currPos = cameFrom[currPos];
        }
        path.Add(start);
        path.Reverse();

        return path;
    }

    public void RequestPlayerMoveToEvent(RoomEvent clickedEvent)
    {
        if(isPlayerMoving)
        {
            Debug.LogError("현재 플레이어가 움직이고 있어 호출을 무시합니다.");
            return;
        }
        // 클릭된 이벤트가 부착된 방의 좌표를 가져옴
        Room targetRoom = clickedEvent.Room;
        if (targetRoom == null)
        {
            Debug.LogError("클릭된 이벤트에 Room 컴포넌트가 없습니다.");
            return;
        }
        Vector2Int targetPos = targetRoom.gridPos;

        // 플레이어 현재 방 (MapManager.currentPlayerRoom)에서 목표까지의 경로 찾기
        List<Vector2Int> path = FindPath(currentPlayerPos, targetPos);
        if (path == null)
        {
            Debug.LogWarning("경로를 찾지 못했습니다.");
            return;
        }
        else
        {
            string pathString = "";
            for(int i = 0; i < path.Count; i++)
            {
                pathString += (path[i].ToString() + " ");
            }
            Debug.Log($"생각한 경로가 {pathString}입니다.");
        }

        // 경로의 중간에 다른 이벤트가 있는지 확인 (시작방과 목표방은 제외)
        // 만약 중간에 roomEvent가 있다면, 이동을 막고 로그 출력
        for (int i = 1; i < path.Count - 1; i++)
        {
            if (mapGenRooms.ContainsKey(path[i]))
            {
                Room room = mapGenRooms[path[i]].GetComponent<Room>();
                if (room != null && room.roomEvent != null)
                {
                    Debug.Log("경로에 다른 이벤트가 있어 이동할 수 없습니다.");
                    return;
                }
            }
        }

        // 경로가 모두 깨끗하면, 코루틴을 시작해서 플레이어 이동
        StartCoroutine(MovePlayerAlongPath(path));
    }

    private IEnumerator MovePlayerAlongPath(List<Vector2Int> path)
    {
        isPlayerMoving = true;
        // path의 각 방 좌표를 순서대로 이동합니다.
        foreach (Vector2Int roomPos in path)
        {
            Vector3 targetWorldPos = GetWorldPositionInPos(roomPos);
            // (예시) 타겟 좌표까지 선형 보간으로 이동
            while (Vector3.Distance(player.transform.position, targetWorldPos) > 0.01f)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, targetWorldPos, playerMoveSpeed * Time.deltaTime);
                yield return null;
            }
            // 이동이 끝나면 현재 플레이어 방 좌표 업데이트
            currentPlayerPos = roomPos;
            yield return null;
        }
        Debug.Log($"플레이어가 {currentPlayerPos}로 이동 완료. ");
        ActivateRoomInPos(currentPlayerPos);
        isPlayerMoving = false;
    }

    private void ActivateRoomInPos(Vector2Int pos)
    {
        Room eventRoom = GetRoomInPos(pos).GetComponent<Room>();
        if (eventRoom != null)
        {
            eventRoom.ActivateRoom();
            RemoveVisualInPos(pos);
            eventRoom.DeactivateRoomEvent();
        }

    }
    

    private void CheckConnection(Vector2Int pos)
    {
        int dx = pos.x;
        int dy = pos.y;
        int[] deltaX = { 1, -1, 0, 0 };
        int[] deltaY = { 0, 0, -1, 1 };

        for(int i = 0; i < deltaX.Length; i++) 
        { 
            Vector2Int deltaPos = new Vector2Int(dx + deltaX[i], dy + deltaY[i]);
            if(mapGenRooms.ContainsKey(deltaPos))
            {
                Room deltaPosRoom = GetRoomInPos(deltaPos);
                if(deltaPosRoom != null)
                {
                    foreach(int dir in deltaPosRoom.connectionDirection)
                    {
                        if(dir == i)
                        {
                            //Debug.Log($"연결을 확인해 {pos}와 {deltaPos}사이의 연결을 만듭니다.");
                            RegisterConnection(pos, deltaPos);
                            
                        }
                    }
                }
            }

        }

    }

    private Room GetRoomInPos(Vector2Int pos)
    {
        if(mapGenRooms.ContainsKey(pos) == false)
        {
            Logger.LogWarning($"[MapManager] - 현재 없는 좌표인 {pos}의 Room에 접근하고 있습니다.");
            return null;
        }

        return mapGenRooms[pos].GetComponent<Room>();
    }

    public Vector3 GetWorldPositionInPos(Vector2Int pos)
    {
        if (mapGenRooms.ContainsKey(pos)  == false)
        {
            Debug.LogError($"[MapManager] - 현재 존재하지 않는 맵의 좌표인 {pos}에 접근하려 합니다.");
            return Vector3.zero;
        }

        return mapGenRooms[pos].transform.position;
    }
    public void CalcRoomGenCount(int amount)
    {
        currentRoomGenerating += amount;
        //Logger.Log($"현재 {currentRoomGenerating} 개의 방이 생성 중입니다.");
        if (currentRoomGenerating <= 0)
        {
            Logger.LogError("방 생성 개수가 0 이하입니다.");
        }
    }

    // 두 방이 연결되었음을 등록하는 함수 (양방향 연결)
    public void RegisterConnection(Vector2Int posA, Vector2Int posB)
    {
        if (!roomGraph.ContainsKey(posA))
        {
            roomGraph[posA] = new List<Vector2Int>();
        }
        if (!roomGraph[posA].Contains(posB))
        {
            roomGraph[posA].Add(posB);
        }

        if (!roomGraph.ContainsKey(posB))
        {
            roomGraph[posB] = new List<Vector2Int>();
        }
        if (!roomGraph[posB].Contains(posA))
        {
            roomGraph[posB].Add(posA);
        }
    }

    // 현재 플레이어의 방에서 이동 가능한 (연결된) 방들의 좌표 목록을 리턴
    public List<Vector2Int> GetAvailableMoves()
    {
        if (roomGraph.ContainsKey(currentPlayerPos))
        {
            return roomGraph[currentPlayerPos];
        }
        return new List<Vector2Int>();
    }

    public void DebugCheckCurrentAvailableMoves()
    {
        List<Vector2Int> currentAvailableMove = GetAvailableMoves();
        Debug.Log($"현재 {currentPlayerPos}에서 이동가능한 방의 개수 = {currentAvailableMove.Count} ///");
        foreach (var move in currentAvailableMove) 
        {
            Debug.Log(move);
        }
    }


    // 플레이어 이동 처리 함수 이동 가능한 방인지 확인하고, 이동
    public bool MovePlayerTo(Vector2Int targetRoom)
    {
        List<Vector2Int> availableMoves = GetAvailableMoves();
        if (availableMoves.Contains(targetRoom))
        {
            currentPlayerPos = targetRoom;
            // 추가: 플레이어 오브젝트의 실제 위치 이동 처리 등
            return true;
        }
        Debug.LogWarning("목표 방으로 이동할 수 없습니다: " + targetRoom);
        return false;
    }

    // 예시: 모든 방 생성이 완료된 후, 로직에 따라 각 방에 이벤트를 할당한다.
    private void PlaceRandomEvents()
    {
        //Logger.Log("방의 개수 = " + mapGenRooms.Count);
        if(battleRoomCount + bossRoomCount + shopRoomCount + treasureRoomCount - 1 > mapGenRooms.Count)
        {
            // 현재 있는 방보다 생성해야 할 방이 많아야 할 경우..?
            Logger.LogError($"생성해야 할 방이 {battleRoomCount + bossRoomCount + shopRoomCount + treasureRoomCount} 개지만 현재 있는 방의 개수가 {mapGenRooms.Count}로 더 많습니다.");
            return;
        }

        BossEvent bossEvent = new BossEvent();
        PlaceEventInFarest(bossEvent);

        BattleEvent battleEvent = new BattleEvent();
        PlaceRandomEvents(battleRoomCount, battleEvent);

        TreasureEvent treasureEvent = new TreasureEvent();
        PlaceRandomEvents(treasureRoomCount, treasureEvent);

        ShopEvent shopEvent = new ShopEvent();
        PlaceRandomEvents(shopRoomCount, shopEvent);
    }

    private void PlaceRandomEvents(int count, RoomEvent eventType)
    {
        for(int i = 0; i < count; i++)
        {
            bool isPlacedRight = false;
            while(!isPlacedRight)
            {
                
                List<GameObject> currentRooms = mapGenRooms.Values.ToList<GameObject>();
                int index = Random.Range(0, currentRooms.Count);

                Room room = currentRooms[index].GetComponent<Room>();

                if (room == null || room.gridPos == Vector2Int.zero) continue;
                if(room.roomEvent == null)
                {
                    PlaceEventInVisual(currentRooms[index], room.gridPos, eventType);
                    room.roomEvent = currentRooms[index].AddComponent(eventType.GetType()) as RoomEvent;
                    room.roomEvent.Room = room;
                    //Logger.Log($"[MapManager] - {room.gridPos} 에 {eventType.GetType().ToString()} 을 배치합니다.");
                    isPlacedRight = true;
                    break;
                }
            }
        }
    }

    private void PlaceEventInFarest(RoomEvent eventType)
    {
        bool isPlacedRight = false;
        float farestDistance = float.MinValue;

        List<GameObject> currentRooms = mapGenRooms.Values.ToList<GameObject>();
        Debug.Log($"이벤트 설치 중, 현재 있는 방의 개수가 {currentRooms.Count}개임");
        for(int i = 0; i <  currentRooms.Count; i++) 
        {
            Debug.Log($"{i}번째.");
            Debug.Log(currentRooms[i].name + "임");
        }

        GameObject farestRoomObject = null;
        foreach(GameObject room in  currentRooms)
        {
            float distance = room.GetComponent<Room>().gridPos.sqrMagnitude;
            // 거리가 가장 멀고, 이벤트가 할당되어있지 않으며, 0,0의 위치가 아닌 경우.
            if(distance >= farestDistance && room.GetComponent<Room>().roomEvent == null && room.GetComponent<Room>().gridPos != Vector2Int.zero)
            {
                farestDistance = distance;
                farestRoomObject = room;
            }
        }

        if(farestRoomObject != null)
        {
            Room room = farestRoomObject.GetComponent<Room>();

            PlaceEventInVisual(farestRoomObject, room.gridPos, eventType);
            room.roomEvent = farestRoomObject.AddComponent(eventType.GetType()) as RoomEvent;
            room.roomEvent.Room = room;
            //Logger.Log($"[MapManager] - {room.gridPos} 에 {eventType.GetType().ToString()} 을 배치합니다.");
            isPlacedRight = true;
        }

        if (!isPlacedRight)
        {
            Logger.LogError($"[MapManager] - 가장 먼 방에 배치하는 방 배정이 비정상적으로 작동되었습니다.");
        }
    }

    private void PlaceEventInVisual(GameObject worldPos, Vector2Int pos, RoomEvent eventType)
    {
        if (eventType.GetType() == typeof(BattleEvent))
        {
            PlaceVisualInPos(worldPos, pos, RoomTemplate.BattleVisual);
        }
        else if (eventType.GetType() == typeof(BossEvent))
        {
            PlaceVisualInPos(worldPos, pos, RoomTemplate.BossVisual);
        }
        else if(eventType.GetType() == typeof(ShopEvent))
        {
            PlaceVisualInPos(worldPos, pos, RoomTemplate.TreatureVisual);
        }
        else if(eventType.GetType() == typeof(TreasureEvent))
        {
            PlaceVisualInPos(worldPos, pos, RoomTemplate.ShopVisual);
        }
        
    }

    private void PlaceVisualInPos(GameObject worldPos, Vector2Int IntPos, GameObject visual)
    {
        if(!mapGenVisuals.ContainsKey(IntPos))
        {
            GameObject instanceVisual = Instantiate(visual, worldPos.transform);
            mapGenVisuals.Add(IntPos, instanceVisual);
        }
        else
        {
            Logger.LogWarning("[MapManager] - 맵의 비주얼상 이미 있는 이벤트 위치에 배치하려고 합니다.");
        }
        
        
    }

    private void RemoveVisualInPos(Vector2Int intPos)
    {
        GameObject instantiatedVisual = mapGenVisuals[intPos];
        if (instantiatedVisual != null)
        {
            Destroy(instantiatedVisual);
        }
        mapGenVisuals.Remove(intPos);
        Logger.Log($"[MapManager] - {intPos}에 위치한 이벤트를 비주얼상 제거했습니다.");
    }

}


