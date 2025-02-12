using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class MapManager : SingletonBehaviour<MapManager>
{
    public int currentStageNum = 0;

    [SerializeField] private RoomTemplates _roomTemplate;
    [SerializeField] private Transform roomParent;

    // 각 방의 그리드 좌표를 기록 (Vector2Int 사용)
    private Dictionary<Vector2Int, bool> mapGenRooms = new Dictionary<Vector2Int, bool>();

    // 생성된 방 리스트 (AddRoom 스크립트에서 추가)
    public List<GameObject> rooms = new List<GameObject>();
    public GameObject startRoom;

    // 최대 방 개수 (원하는 값으로 설정)
    public int maxRooms = 10;

    private bool isRoomReachedMax;
    public float waitTime;
    private bool spawnedBoss;
    private int currentRoomGenerating = 0;

    // 플레이어의 현재 위치 (시작방은 보통 (0,0)으로 설정)
    public Vector2Int currentPlayerRoom = Vector2Int.zero;

    // 키: 해당 방의 gridPos, 값: 연결된 방의 gridPos 목록
    private Dictionary<Vector2Int, List<Vector2Int>> roomGraph = new Dictionary<Vector2Int, List<Vector2Int>>();

    [SerializeField] private int battleRoomCount;
    [SerializeField] private int bossRoomCount;
    [SerializeField] private int treasureRoomCount;
    [SerializeField] private int shopRoomCount;

    private bool isEventPlaced = false;
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
        
        if(!isEventPlaced && isRoomReachedMax)
        {
            PlaceRandomEvents();
            isEventPlaced = true;
        }
        
    }

    protected override void Init()
    {
        base.Init();
        InitializeData();
    }

    private void InitializeData()
    {
        // 초기화가 필요하다면 여기에 구현
        //rooms.Add(startRoom);
    }

    public Transform RoomParent { get { return roomParent; } }

    public RoomTemplates RoomTemplates
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

    // 방 좌표를 등록 (이미 등록되어 있으면 경고)
    public void RegisterRoom(Vector2Int pos)
    {
        if (!mapGenRooms.ContainsKey(pos))
        {
            mapGenRooms.Add(pos, true);
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
                            Debug.Log($"연결을 확인해 {pos}와 {deltaPos}사이의 연결을 만듭니다.");
                            RegisterConnection(pos, deltaPos);
                            
                        }
                    }
                }
            }

        }

    }

    private Room GetRoomInPos(Vector2Int pos)
    {
        foreach(GameObject roomObject in rooms) 
        {
            if(roomObject != null)
            {
                if (roomObject.GetComponent<Room>().gridPos == pos)
                {
                    return roomObject.GetComponent<Room>();
                }
            }
        }
        return null;
    }

    public void CalcRoomGenCount(int amount)
    {
        currentRoomGenerating += amount;
        Logger.Log($"현재 {currentRoomGenerating} 개의 방이 생성 중입니다.");
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
        if (roomGraph.ContainsKey(currentPlayerRoom))
        {
            return roomGraph[currentPlayerRoom];
        }
        return new List<Vector2Int>();
    }

    public void DebugCheckCurrentAvailableMoves()
    {
        List<Vector2Int> currentAvailableMove = GetAvailableMoves();
        Debug.Log($"현재 {currentPlayerRoom}에서 이동가능한 방의 개수 = {currentAvailableMove.Count} ///");
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
            currentPlayerRoom = targetRoom;
            // 추가: 플레이어 오브젝트의 실제 위치 이동 처리 등
            return true;
        }
        Debug.LogWarning("목표 방으로 이동할 수 없습니다: " + targetRoom);
        return false;
    }

    // 예시: 모든 방 생성이 완료된 후, 로직에 따라 각 방에 이벤트를 할당한다.
    private void PlaceRandomEvents()
    {
        Logger.Log("방의 개수 = " + rooms.Count);
        if(battleRoomCount + bossRoomCount + shopRoomCount + treasureRoomCount > rooms.Count)
        {
            // 현재 있는 방보다 생성해야 할 방이 많아야 할 경우..?
            Logger.LogError($"생성해야 할 방이 {battleRoomCount + bossRoomCount + shopRoomCount + treasureRoomCount} 개지만 현재 있는 방의 개수가 {rooms.Count}로 더 많습니다.");
            return;
        }

        BossEvent bossEvent = new BossEvent();
        PlaceFromLastIndexEvent(bossRoomCount, bossEvent);

        BattleEvent battleEvent = new BattleEvent();
        PlaceRandomEvent(battleRoomCount, battleEvent);

        TreasureEvent treasureEvent = new TreasureEvent();
        PlaceRandomEvent(treasureRoomCount, treasureEvent);

        ShopEvent shopEvent = new ShopEvent();
        PlaceRandomEvent(shopRoomCount, shopEvent);
    }

    private void PlaceRandomEvent(int count, RoomEvent eventType)
    {
        for(int i = 0; i < count; i++)
        {
            bool isPlacedRight = false;
            while(!isPlacedRight)
            {
                int index = Random.Range(0, rooms.Count - 1);
                Room room = rooms[index].GetComponent<Room>();
                if (room == null) continue;
                if(room.roomEvent == null)
                {
                    room.roomEvent = rooms[index].AddComponent(eventType.GetType()) as RoomEvent;
                    Debug.Log($"{room.gridPos} 에 {eventType.GetType().ToString()} 을 배치합니다.");
                    isPlacedRight = true;
                    break;
                }
            }
        }
    }

    private void PlaceFromLastIndexEvent(int count, RoomEvent eventType)
    {
        for(int i = 0; i < count;i++)
        {
            bool isPlacedRight = false;
            for(int j = rooms.Count - 1; j >= 0; j--)
            {
                Room room = rooms[j].GetComponent<Room>();
                if (room == null) continue;

                if(room.roomEvent == null)
                {
                    room.roomEvent = rooms[j].AddComponent(eventType.GetType()) as RoomEvent;
                    Debug.Log($"{room.gridPos} 에 {eventType.GetType().ToString()} 을 배치합니다.");
                    isPlacedRight = true;
                    break;
                }

            }
            if(!isPlacedRight)
            {
                Logger.LogError($"뒤에서부터 시작하는 방 배정이 끝까지 작동되지 않았습니다.");
            }
        }
    }

}
