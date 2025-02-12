using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class MapManager : SingletonBehaviour<MapManager>
{
    public int currentStageNum = 0;

    [SerializeField] private RoomTemplates _roomTemplate;
    [SerializeField] private Transform roomParent;

    // �� ���� �׸��� ��ǥ�� ��� (Vector2Int ���)
    private Dictionary<Vector2Int, bool> mapGenRooms = new Dictionary<Vector2Int, bool>();

    // ������ �� ����Ʈ (AddRoom ��ũ��Ʈ���� �߰�)
    public List<GameObject> rooms = new List<GameObject>();
    public GameObject startRoom;

    // �ִ� �� ���� (���ϴ� ������ ����)
    public int maxRooms = 10;

    private bool isRoomReachedMax;
    public float waitTime;
    private bool spawnedBoss;
    private int currentRoomGenerating = 0;

    // �÷��̾��� ���� ��ġ (���۹��� ���� (0,0)���� ����)
    public Vector2Int currentPlayerRoom = Vector2Int.zero;

    // Ű: �ش� ���� gridPos, ��: ����� ���� gridPos ���
    private Dictionary<Vector2Int, List<Vector2Int>> roomGraph = new Dictionary<Vector2Int, List<Vector2Int>>();

    [SerializeField] private int battleRoomCount;
    [SerializeField] private int bossRoomCount;
    [SerializeField] private int treasureRoomCount;
    [SerializeField] private int shopRoomCount;

    private bool isEventPlaced = false;
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
        // �ʱ�ȭ�� �ʿ��ϴٸ� ���⿡ ����
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

    // �־��� ��ǥ�� ���� �̹� �����ϴ��� Ȯ��
    public bool IsRoomExistAt(Vector2Int pos)
    {
        return mapGenRooms.ContainsKey(pos);
    }

    // �� ��ǥ�� ��� (�̹� ��ϵǾ� ������ ���)
    public void RegisterRoom(Vector2Int pos)
    {
        if (!mapGenRooms.ContainsKey(pos))
        {
            mapGenRooms.Add(pos, true);
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
                            Debug.Log($"������ Ȯ���� {pos}�� {deltaPos}������ ������ ����ϴ�.");
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
        Logger.Log($"���� {currentRoomGenerating} ���� ���� ���� ���Դϴ�.");
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
        if (roomGraph.ContainsKey(currentPlayerRoom))
        {
            return roomGraph[currentPlayerRoom];
        }
        return new List<Vector2Int>();
    }

    public void DebugCheckCurrentAvailableMoves()
    {
        List<Vector2Int> currentAvailableMove = GetAvailableMoves();
        Debug.Log($"���� {currentPlayerRoom}���� �̵������� ���� ���� = {currentAvailableMove.Count} ///");
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
            currentPlayerRoom = targetRoom;
            // �߰�: �÷��̾� ������Ʈ�� ���� ��ġ �̵� ó�� ��
            return true;
        }
        Debug.LogWarning("��ǥ ������ �̵��� �� �����ϴ�: " + targetRoom);
        return false;
    }

    // ����: ��� �� ������ �Ϸ�� ��, ������ ���� �� �濡 �̺�Ʈ�� �Ҵ��Ѵ�.
    private void PlaceRandomEvents()
    {
        Logger.Log("���� ���� = " + rooms.Count);
        if(battleRoomCount + bossRoomCount + shopRoomCount + treasureRoomCount > rooms.Count)
        {
            // ���� �ִ� �溸�� �����ؾ� �� ���� ���ƾ� �� ���..?
            Logger.LogError($"�����ؾ� �� ���� {battleRoomCount + bossRoomCount + shopRoomCount + treasureRoomCount} ������ ���� �ִ� ���� ������ {rooms.Count}�� �� �����ϴ�.");
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
                    Debug.Log($"{room.gridPos} �� {eventType.GetType().ToString()} �� ��ġ�մϴ�.");
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
                    Debug.Log($"{room.gridPos} �� {eventType.GetType().ToString()} �� ��ġ�մϴ�.");
                    isPlacedRight = true;
                    break;
                }

            }
            if(!isPlacedRight)
            {
                Logger.LogError($"�ڿ������� �����ϴ� �� ������ ������ �۵����� �ʾҽ��ϴ�.");
            }
        }
    }

}
