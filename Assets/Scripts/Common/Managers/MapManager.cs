using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Rendering;

public class MapManager : SingletonBehaviour<MapManager>
{
    public int currentStageNum = 1;
    [SerializeField] private MapUI mapUI;
    [SerializeField] private RoomTemplates _roomTemplate;
    private MapLoader mapLoader;
    public event Action OnMoveRoom;
    [SerializeField] private Transform roomParent;
    [SerializeField] private Transform entryRoomParent;
    [SerializeField] private GameObject entryRoomPrefab; // EntryRoom 프리팹 (Inspector에 할당)

    // 맵 생성 그리드 좌표와 방들 (Vector2Int 기준)
    [SerializeField]private Dictionary<Vector2Int, GameObject> mapGenRooms = new Dictionary<Vector2Int, GameObject>();
    [SerializeField] private Dictionary<Vector2Int, GameObject> mapGenVisuals = new Dictionary<Vector2Int, GameObject>();

    // 키: 해당 방의 gridPos, 값: 연결된 방의 gridPos 목록
    private Dictionary<Vector2Int, List<Vector2Int>> roomGraph = new Dictionary<Vector2Int, List<Vector2Int>>();

    public int RoomCount { get { return mapGenRooms.Count; } }
    public GameObject startRoom;

    // 최대 방 개수 (원하는 만큼만 생성)
    public int maxRooms = 10;
    private bool isRoomReachedMax;
    public float waitTime;
    private int currentRoomGenerating = 0;

    // 플레이어의 현재 위치 (시작방은 항상 (0,0)에서 시작)
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
        IsDestroyOnLoad = true;
        Init();
    }

    void Update()
    {
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
                    Debug.LogError($"{i}번째 데이터에 방이 없음.");
                    isObjectDataValid = false;
                    ResetMap(); // 맵 생성을 다시한번 시도함.
                }
            }

            // 모든 방 생성이 완료 된 경우, 랜덤 이벤트 배치.
            if(isObjectDataValid)
            {
                PlaceRandomEvents();
                isEventPlaced = true;
                Debug.Log($"이벤트를 모두 배치하기까지 {debugEventPlacingTime}초 걸렸습니다.");
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

    public void GoToNextStage()
    {
        if(currentStageNum >= 3)
        {
            Logger.LogWarning("마지막 스테이지까지 클리어되었습니다.");
            return;
        }
        currentStageNum++;
        
        // 스테이지 팝업 호출.
        UIManager.Instance.OpenUI<FullScreenPopup>(new BaseUIData { });
        FullScreenPopup nextStagePopup = UIManager.Instance.GetActiveUI<FullScreenPopup>().GetComponent<FullScreenPopup>();
        nextStagePopup.StartFade($"- Stage {currentStageNum} -", 0.5f, 1.0f, 1.0f);


        ResetMap();
    }

    /// <summary>
    /// 맵 생성 및 관련 정보 초기화 (스테이지 변화)
    /// </summary>
    public void ResetMap()
    {
        Debug.LogWarning("맵 생성을 시작합니다.");

        mapGenRooms.Clear();
        mapGenVisuals.Clear();
        roomGraph.Clear();

        foreach (Transform child in roomParent)
        {
            Destroy(child.gameObject);
        }
        Destroy(startRoom.gameObject);

        isEventPlaced = false;
        isRoomReachedMax = false;

        if (entryRoomPrefab != null)
        {
            Vector3 gridZeroWorldPos = entryRoomParent.transform.position;
            GameObject newEntryRoom = Instantiate(entryRoomPrefab, roomParent, false);
            newEntryRoom.transform.localPosition = gridZeroWorldPos;
            startRoom = newEntryRoom;
            mapGenRooms.Add(Vector2Int.zero, newEntryRoom);
            currentPlayerPos = Vector2Int.zero;
            player.transform.position = GetWorldPositionInPos(currentPlayerPos);
        }
        else
        {
            Logger.LogError("EntryRoom 프리팹이 할당되어 있지 않습니다.");
        }
    }

    /// <summary>
    /// 외부에서 MapLoader를 통해 맵 데이터를 접근할 수 있도록 Dictionary를 반환합니다.
    /// </summary>
    public Dictionary<Vector2Int, GameObject> GetMapRooms()
    {
        return mapGenRooms;
    }

    /// <summary>
    /// 그리드 좌표를 월드상의 실제 좌표로 변환 (방 크기에 맞게 조정)
    /// </summary>
    public Vector3 GetWorldPositionForGrid(Vector2Int gridPos)
    {
        float roomWidth = 10f;  // 방의 폭
        float roomHeight = 10f; // 방의 높이
        return new Vector3(gridPos.x * roomWidth, gridPos.y * roomHeight, 0f);
    }

    /// <summary>
    /// 전달된 방 이름에 따라 해당 프리팹을 반환합니다.
    /// (여기서는 RoomTemplates에 있는 배열을 활용하는 방식입니다.)
    /// </summary>
    public GameObject GetPrefabForRoomName(string roomName)
    {
        // 방향 체크 (현재 프로젝트 상황에 맞게 수정)
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

    // 방 좌표와 등록 (이미 등록되어 있으면 무시)
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
        // 이미 등록된 방들의 연결 정보 (roomGraph)를 활용
        if (!roomGraph.ContainsKey(start) || !roomGraph.ContainsKey(target))
        {
            Logger.LogWarning("pathfinding을 위한 좌표가 등록되어 있지 않습니다.");
            return null;
        }

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        queue.Enqueue(start);
        cameFrom[start] = start;  // 시작 좌표 등록

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

        // 결과가 null인 경우
        if (!cameFrom.ContainsKey(target))
        {
            Debug.LogWarning("경로 찾기 실패: 목적지에 도달할 수 없습니다.");
            return null;
        }

        // 결과 반환 (역순으로 반환된 경로를 정방향으로 변환)
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
        // 플레이어의 이동이 진행 중이거나 전투 상태가 진행 중인 경우
        if (isPlayerMoving || BattleManager.Instance.battleState != BattleStateType.BattleEnd)
        {
            mapUI.OnToggleReachUI("Party Can't move now.");
            //Debug.LogError("플레이어의 이동이 진행 중이거나 전투 상태가 진행 중이므로 이동할 수 없습니다.");
            return;
        }

        // 타겟 방 설정
        Room targetRoom = clickedEvent.Room;
        if (targetRoom == null)
        {
            Debug.LogError("타겟 방이 Room 컴포넌트를 가지고 있지 않습니다.");
            return;
        }

        Vector2Int targetPos = targetRoom.gridPos;

        // 플레이어의 위치를 찾아 경로 찾기
        List<Vector2Int> path = FindPath(currentPlayerPos, targetPos);
        if (path == null)
        {
            mapUI.OnToggleReachUI("Cannot reach destination.");
            return;
        }
        else
        {
            string pathString = "";
            for(int i = 0; i < path.Count; i++)
            {
                pathString += (path[i].ToString() + " ");
            }
            Debug.Log($"플레이어의 이동 경로는 {pathString}입니다.");
        }

        // 경로 상의 모든 방에 이벤트가 배치되어 있는지 확인 (전투 이벤트가 배치되어 있는지 확인)
        // 경로 상의 방에 이벤트가 배치되어 있는 경우, 플레이어의 이동을 중지
        for (int i = 1; i < path.Count - 1; i++)
        {
            if (mapGenRooms.ContainsKey(path[i]))
            {
                Room room = mapGenRooms[path[i]].GetComponent<Room>();
                if (room != null && room.roomEvent != null)
                {
                    mapUI.OnToggleReachUI("There is Event room placed in path.");
                    return;
                }
            }
        }

        // 경로 상의 모든 방에 이벤트가 배치되어 있는 경우, 플레이어의 이동을 허용
        OnMoveRoom?.Invoke();
        StartCoroutine(MovePlayerAlongPath(path));
    }

    private IEnumerator MovePlayerAlongPath(List<Vector2Int> path)
    {
        isPlayerMoving = true;
        // path를 따라 플레이어를 이동시킴
        foreach (Vector2Int roomPos in path)
        {
            Vector3 targetWorldPos = GetWorldPositionInPos(roomPos);
            // (비효율적인 방법) 플레이어를 목적지까지 이동시킴
            while (Vector3.Distance(player.transform.position, targetWorldPos) > 0.01f)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, targetWorldPos, playerMoveSpeed * Time.deltaTime);
                yield return null;
            }
            // 플레이어의 위치를 방의 좌표로 갱신
            currentPlayerPos = roomPos;
            yield return null;
        }
        Debug.Log($"플레이어가 {currentPlayerPos}로 이동했습니다. ");
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
                            //Debug.Log($"연결 확인 {pos}와 {deltaPos}에 대한 연결 확인입니다.");
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
            Logger.LogWarning($"[MapManager] - 좌표 {pos}에 방이 등록되어 있지 않습니다.");
            return null;
        }

        return mapGenRooms[pos].GetComponent<Room>();
    }

    public Vector3 GetWorldPositionInPos(Vector2Int pos)
    {
        if (mapGenRooms.ContainsKey(pos)  == false)
        {
            Debug.LogError($"[MapManager] - 좌표 {pos}에 방이 등록되어 있지 않습니다.");
            return Vector3.zero;
        }

        return mapGenRooms[pos].transform.position;
    }
    public void CalcRoomGenCount(int amount)
    {
        currentRoomGenerating += amount;
        //Logger.Log($"방 생성 횟수 {currentRoomGenerating} 중 남은 방 생성 횟수는 {currentRoomGenerating}입니다.");
        if (currentRoomGenerating <= 0)
        {
            Logger.LogError("방 생성 횟수가 0이 되었습니다.");
        }
    }

    // 방 좌표와 연결 (이미 등록되어 있으면 무시)
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

    // 플레이어의 이동을 위한 가능한 이동 경로 반환
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
        Debug.Log($"현재 {currentPlayerPos}위치에서 이동 가능한 방들의 수는 {currentAvailableMove.Count} ///");
        foreach (var move in currentAvailableMove) 
        {
            Debug.Log(move);
        }
    }


    // 플레이어의 이동을 위한 함수 (플레이어의 위치를 타겟 방으로 갱신)
    public bool MovePlayerTo(Vector2Int targetRoom)
    {
        List<Vector2Int> availableMoves = GetAvailableMoves();
        if (availableMoves.Contains(targetRoom))
        {
            currentPlayerPos = targetRoom;
            // 추가: 플레이어의 위치를 방의 좌표로 갱신
            return true;
        }
        Debug.LogWarning("좌표 입력이 잘못되었습니다: " + targetRoom);
        return false;
    }

    // 추가: 맵 생성이 완료된 후에 랜덤 이벤트 배치
    private void PlaceRandomEvents()
    {
        //Logger.Log("방의 수는 " + mapGenRooms.Count);
        if(battleRoomCount + bossRoomCount + shopRoomCount + treasureRoomCount - 1 > mapGenRooms.Count)
        {
            // 최대 방 개수를 초과하는 경우 에러 로그 출력
            Logger.LogError($"방 생성이 완료되었지만 방 개수가 {battleRoomCount + bossRoomCount + shopRoomCount + treasureRoomCount}개 중에서 {mapGenRooms.Count}개만 생성되었습니다.");
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
                int index = UnityEngine.Random.Range(0, currentRooms.Count);

                Room room = currentRooms[index].GetComponent<Room>();

                if (room == null || room.gridPos == Vector2Int.zero) continue;
                if(room.roomEvent == null)
                {
                    PlaceEventInVisual(currentRooms[index], room.gridPos, eventType);
                    room.roomEvent = currentRooms[index].AddComponent(eventType.GetType()) as RoomEvent;
                    room.roomEvent.Room = room;
                    //Logger.Log($"[MapManager] - {room.gridPos} 에 {eventType.GetType().ToString()} 가 배치되었습니다.");
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
        Debug.Log($"이벤트 배치 시작, 현재 생성된 방들의 수는 {currentRooms.Count}개입니다.");
        /*
        for(int i = 0; i <  currentRooms.Count; i++) 
        {
            Debug.Log($"{i}번째.");
            Debug.Log(currentRooms[i].name + "입니다.");
        }
        */

        GameObject farestRoomObject = null;
        foreach(GameObject room in  currentRooms)
        {
            float distance = room.GetComponent<Room>().gridPos.sqrMagnitude;
            // 거리 비교, 이벤트가 배치되어 있지 않고, 0,0에 배치되어 있는 방은 제외
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
            //Logger.Log($"[MapManager] - {room.gridPos} 에 {eventType.GetType().ToString()} 가 배치되었습니다.");
            isPlacedRight = true;
        }

        if (!isPlacedRight)
        {
            Logger.LogError($"[MapManager] - 최대 거리에 배치되어 있는 방이 없거나 배치되어 있지 않은 방이 존재합니다.");
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
            PlaceVisualInPos(worldPos, pos, RoomTemplate.ShopVisual);
        }
        else if(eventType.GetType() == typeof(TreasureEvent))
        {
            PlaceVisualInPos(worldPos, pos, RoomTemplate.TreatureVisual);
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
            Logger.LogWarning("[MapManager] - 이미 등록된 방이 있는 좌표에 이벤트가 배치되어 있습니다.");
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
        Logger.Log($"[MapManager] - {intPos}에 배치된 이벤트가 제거되었습니다.");
    }

}


