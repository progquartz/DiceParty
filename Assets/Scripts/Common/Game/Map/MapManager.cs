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
    [SerializeField] private GameObject entryRoomPrefab; // EntryRoom ������ (Inspector�� �Ҵ�)
    


    // �� ���� �׸��� ��ǥ�� ��� (Vector2Int ���)
    [SerializeField]private Dictionary<Vector2Int, GameObject> mapGenRooms = new Dictionary<Vector2Int, GameObject>();
    [SerializeField] private Dictionary<Vector2Int, GameObject> mapGenVisuals = new Dictionary<Vector2Int, GameObject>();

    // Ű: �ش� ���� gridPos, ��: ����� ���� gridPos ���
    private Dictionary<Vector2Int, List<Vector2Int>> roomGraph = new Dictionary<Vector2Int, List<Vector2Int>>();

    public int RoomCount { get { return mapGenRooms.Count; } }
    public GameObject startRoom;

    // �ִ� �� ���� (���ϴ� ������ ����)
    public int maxRooms = 10;
    private bool isRoomReachedMax;
    public float waitTime;
    private bool spawnedBoss;
    private int currentRoomGenerating = 0;

    // �÷��̾��� ���� ��ġ (���۹��� ���� (0,0)���� ����)
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
        // �� ��ȯ �� �������� �ʵ��� (���ϴ� ���)
        IsDestroyOnLoad = true;
        Init();
        
    }

    void Update()
    {
        // 0,0���� ���� �� ��ǥ�� ���� �� ����.

        // ������ �濡 ����� �ٸ� ��� �ֱ�.
        // �߰�: ��� �� ������ �Ϸ�Ǹ� �����͸� �����ϰų� �ٸ� ���� ó�� ����
        //��...
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
                    Debug.LogError($"{i}��° �����Ϳ� ���� ����.");
                    isObjectDataValid = false;
                }
            }
            if(isObjectDataValid)
            {
                PlaceRandomEvents();
                isEventPlaced = true;
                Debug.Log($"�̺�Ʈ�� ��� ��ġ�ϱ���� {debugEventPlacingTime}�� �ɸ��ϴ�.");
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
        // �ʿ��� �ʱ�ȭ �۾� ���� (��: ���� �� ������ �ʱ�ȭ)
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

    // �־��� ��ǥ�� ���� �̹� �����ϴ��� Ȯ��
    public bool IsRoomExistAt(Vector2Int pos)
    {
        return mapGenRooms.ContainsKey(pos);
    }

    // �� ���� ���: ���� ������ ����� �����ϰ� ���� �����͸� �ʱ�ȭ�� �� ���۹�(EntryRoom) ����
    public void ResetMap()
    {
        // roomParent ������ ��� ��(GameObject) ����
        foreach (Transform child in roomParent)
        {
            Destroy(child.gameObject);
        }
        mapGenRooms.Clear();
        mapGenVisuals.Clear();
        roomGraph.Clear();
        isEventPlaced = false;
        isRoomReachedMax = false;

        // (�ʿ��� �ٸ� �����͵鵵 �ʱ�ȭ)
        currentPlayerPos = Vector2Int.zero;

        // EntryRoom ���������� ���۹� ����
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
            roomComp.roomName = "EntryRoom"; // �� �̸� ���� (����/�ε带 ����)
            // ���۹� ����
            currentPlayerPos = Vector2Int.zero;
        }
        else
        {
            Logger.LogError("EntryRoom �������� �Ҵ�Ǿ� ���� �ʽ��ϴ�.");
        }
    }

    /// <summary>
    /// �ܺο��� MapLoader�� ���� �� �����͸� ������ �� �ֵ��� Dictionary�� ��ȯ�մϴ�.
    /// </summary>
    public Dictionary<Vector2Int, GameObject> GetMapRooms()
    {
        return mapGenRooms;
    }

    /// <summary>
    /// �׸��� ��ǥ�� �������� ���� ��ǥ�� ��� (�� ũ�⿡ �°� ����)
    /// </summary>
    public Vector3 GetWorldPositionForGrid(Vector2Int gridPos)
    {
        float roomWidth = 10f;  // ���� ��
        float roomHeight = 10f; // ���� ��
        return new Vector3(gridPos.x * roomWidth, gridPos.y * roomHeight, 0f);
    }

    /// <summary>
    /// ����� �� �̸��� ���� �ش� �������� ��ȯ�մϴ�.
    /// (���⼭�� RoomTemplates�� �ִ� �迭�� Ȱ���ϴ� �����Դϴ�.)
    /// </summary>
    public GameObject GetPrefabForRoomName(string roomName)
    {
        // ���� ���� (���� ������Ʈ ��Ȳ�� �°� ����)
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
                Debug.LogWarning("�� �� ���� �� �̸�: " + roomName);
                return null;
        }
    }

    // �� ��ǥ�� ��� (�̹� ��ϵǾ� ������ ���)
    public void RegisterRoom(Vector2Int pos, GameObject room)
    {
        if (!mapGenRooms.ContainsKey(pos))
        {
            mapGenRooms.Add(pos, room);
            CheckConnection(pos);
        }
        else
        {
            Debug.LogWarning($"��ǥ {pos}���� �̹� ���� ��ϵǾ� �ֽ��ϴ�.");
        }
    }

    public void OnMapRoomCountFull()
    {
        isRoomReachedMax = true;
    }

    /// <summary>
    /// bfs ��� Pathfinding
    /// </summary>
    public List<Vector2Int> FindPath(Vector2Int start, Vector2Int target)
    {
        // �̹� ��ϵ� �� �� ���� ���� (roomGraph)�� ���
        if (!roomGraph.ContainsKey(start) || !roomGraph.ContainsKey(target))
        {
            Logger.LogWarning("pathfinding�� ���� �Ǵ� ��ǥ ���� ���� ������ �����ϴ�.");
            return null;
        }

        Queue<Vector2Int> queue = new Queue<Vector2Int>();
        Dictionary<Vector2Int, Vector2Int> cameFrom = new Dictionary<Vector2Int, Vector2Int>();

        queue.Enqueue(start);
        cameFrom[start] = start;  // �������� ǥ��

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

        // ��ΰ� ������ null ����
        if (!cameFrom.ContainsKey(target))
        {
            Debug.LogWarning("��ǥ �̺�Ʈ������ ��θ� ã�� �� �����ϴ�.");
            return null;
        }

        // ��� �籸�� (�ڿ������� ���󰡼� �������� ���� �� reverse)
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
            Debug.LogError("���� �÷��̾ �����̰� �־� ȣ���� �����մϴ�.");
            return;
        }
        // Ŭ���� �̺�Ʈ�� ������ ���� ��ǥ�� ������
        Room targetRoom = clickedEvent.Room;
        if (targetRoom == null)
        {
            Debug.LogError("Ŭ���� �̺�Ʈ�� Room ������Ʈ�� �����ϴ�.");
            return;
        }
        Vector2Int targetPos = targetRoom.gridPos;

        // �÷��̾� ���� �� (MapManager.currentPlayerRoom)���� ��ǥ������ ��� ã��
        List<Vector2Int> path = FindPath(currentPlayerPos, targetPos);
        if (path == null)
        {
            Debug.LogWarning("��θ� ã�� ���߽��ϴ�.");
            return;
        }
        else
        {
            string pathString = "";
            for(int i = 0; i < path.Count; i++)
            {
                pathString += (path[i].ToString() + " ");
            }
            Debug.Log($"������ ��ΰ� {pathString}�Դϴ�.");
        }

        // ����� �߰��� �ٸ� �̺�Ʈ�� �ִ��� Ȯ�� (���۹�� ��ǥ���� ����)
        // ���� �߰��� roomEvent�� �ִٸ�, �̵��� ���� �α� ���
        for (int i = 1; i < path.Count - 1; i++)
        {
            if (mapGenRooms.ContainsKey(path[i]))
            {
                Room room = mapGenRooms[path[i]].GetComponent<Room>();
                if (room != null && room.roomEvent != null)
                {
                    Debug.Log("��ο� �ٸ� �̺�Ʈ�� �־� �̵��� �� �����ϴ�.");
                    return;
                }
            }
        }

        // ��ΰ� ��� �����ϸ�, �ڷ�ƾ�� �����ؼ� �÷��̾� �̵�
        StartCoroutine(MovePlayerAlongPath(path));
    }

    private IEnumerator MovePlayerAlongPath(List<Vector2Int> path)
    {
        isPlayerMoving = true;
        // path�� �� �� ��ǥ�� ������� �̵��մϴ�.
        foreach (Vector2Int roomPos in path)
        {
            Vector3 targetWorldPos = GetWorldPositionInPos(roomPos);
            // (����) Ÿ�� ��ǥ���� ���� �������� �̵�
            while (Vector3.Distance(player.transform.position, targetWorldPos) > 0.01f)
            {
                player.transform.position = Vector3.MoveTowards(player.transform.position, targetWorldPos, playerMoveSpeed * Time.deltaTime);
                yield return null;
            }
            // �̵��� ������ ���� �÷��̾� �� ��ǥ ������Ʈ
            currentPlayerPos = roomPos;
            yield return null;
        }
        Debug.Log($"�÷��̾ {currentPlayerPos}�� �̵� �Ϸ�. ");
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
                            //Debug.Log($"������ Ȯ���� {pos}�� {deltaPos}������ ������ ����ϴ�.");
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
            Logger.LogWarning($"[MapManager] - ���� ���� ��ǥ�� {pos}�� Room�� �����ϰ� �ֽ��ϴ�.");
            return null;
        }

        return mapGenRooms[pos].GetComponent<Room>();
    }

    public Vector3 GetWorldPositionInPos(Vector2Int pos)
    {
        if (mapGenRooms.ContainsKey(pos)  == false)
        {
            Debug.LogError($"[MapManager] - ���� �������� �ʴ� ���� ��ǥ�� {pos}�� �����Ϸ� �մϴ�.");
            return Vector3.zero;
        }

        return mapGenRooms[pos].transform.position;
    }
    public void CalcRoomGenCount(int amount)
    {
        currentRoomGenerating += amount;
        //Logger.Log($"���� {currentRoomGenerating} ���� ���� ���� ���Դϴ�.");
        if (currentRoomGenerating <= 0)
        {
            Logger.LogError("�� ���� ������ 0 �����Դϴ�.");
        }
    }

    // �� ���� ����Ǿ����� ����ϴ� �Լ� (����� ����)
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

    // ���� �÷��̾��� �濡�� �̵� ������ (�����) ����� ��ǥ ����� ����
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
        Debug.Log($"���� {currentPlayerPos}���� �̵������� ���� ���� = {currentAvailableMove.Count} ///");
        foreach (var move in currentAvailableMove) 
        {
            Debug.Log(move);
        }
    }


    // �÷��̾� �̵� ó�� �Լ� �̵� ������ ������ Ȯ���ϰ�, �̵�
    public bool MovePlayerTo(Vector2Int targetRoom)
    {
        List<Vector2Int> availableMoves = GetAvailableMoves();
        if (availableMoves.Contains(targetRoom))
        {
            currentPlayerPos = targetRoom;
            // �߰�: �÷��̾� ������Ʈ�� ���� ��ġ �̵� ó�� ��
            return true;
        }
        Debug.LogWarning("��ǥ ������ �̵��� �� �����ϴ�: " + targetRoom);
        return false;
    }

    // ����: ��� �� ������ �Ϸ�� ��, ������ ���� �� �濡 �̺�Ʈ�� �Ҵ��Ѵ�.
    private void PlaceRandomEvents()
    {
        //Logger.Log("���� ���� = " + mapGenRooms.Count);
        if(battleRoomCount + bossRoomCount + shopRoomCount + treasureRoomCount - 1 > mapGenRooms.Count)
        {
            // ���� �ִ� �溸�� �����ؾ� �� ���� ���ƾ� �� ���..?
            Logger.LogError($"�����ؾ� �� ���� {battleRoomCount + bossRoomCount + shopRoomCount + treasureRoomCount} ������ ���� �ִ� ���� ������ {mapGenRooms.Count}�� �� �����ϴ�.");
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
                    //Logger.Log($"[MapManager] - {room.gridPos} �� {eventType.GetType().ToString()} �� ��ġ�մϴ�.");
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
        Debug.Log($"�̺�Ʈ ��ġ ��, ���� �ִ� ���� ������ {currentRooms.Count}����");
        for(int i = 0; i <  currentRooms.Count; i++) 
        {
            Debug.Log($"{i}��°.");
            Debug.Log(currentRooms[i].name + "��");
        }

        GameObject farestRoomObject = null;
        foreach(GameObject room in  currentRooms)
        {
            float distance = room.GetComponent<Room>().gridPos.sqrMagnitude;
            // �Ÿ��� ���� �ְ�, �̺�Ʈ�� �Ҵ�Ǿ����� ������, 0,0�� ��ġ�� �ƴ� ���.
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
            //Logger.Log($"[MapManager] - {room.gridPos} �� {eventType.GetType().ToString()} �� ��ġ�մϴ�.");
            isPlacedRight = true;
        }

        if (!isPlacedRight)
        {
            Logger.LogError($"[MapManager] - ���� �� �濡 ��ġ�ϴ� �� ������ ������������ �۵��Ǿ����ϴ�.");
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
            Logger.LogWarning("[MapManager] - ���� ���־�� �̹� �ִ� �̺�Ʈ ��ġ�� ��ġ�Ϸ��� �մϴ�.");
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
        Logger.Log($"[MapManager] - {intPos}�� ��ġ�� �̺�Ʈ�� ���־�� �����߽��ϴ�.");
    }

}


