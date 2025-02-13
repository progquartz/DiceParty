using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    // ���� ������� ģ���� �Ա��� ...
    // 1: �Ʒ�, 2: ��, 3: ����, 4: ������ (�ⱸ ����)
    public int openingDirection;

    private RoomTemplates templates;
    private int rand;
    public bool spawned = false;
    public float waitTime = 4f;

    public void Init()
    {
        // ���� �ð��� ������ spawner ������Ʈ ��ü�� ����
        Destroy(gameObject, waitTime);
        templates = MapManager.Instance.RoomTemplate;
        Spawn();
    }

    void Spawn()
    {
        if (spawned) return;

        // �θ� ���� Room ������Ʈ���� ���� ��ǥ�� �����ɴϴ�.
        Room parentRoom = GetComponentInParent<Room>();
        if (parentRoom == null)
        {
            Debug.LogError("RoomSpawner�� �θ� Room ������Ʈ�� �����ϴ�!");
            return;
        }
        Vector2Int parentPos = parentRoom.gridPos;

        // �ⱸ ���⿡ ���� ��ǥ ������ ���
        Vector2Int offset = Vector2Int.zero;
        switch (openingDirection)
        {
            case 1: // ��
                offset = new Vector2Int(0, 1);
                break;
            case 2: // �Ʒ�
                offset = new Vector2Int(0, -1);
                break;
            case 3: // ������
                offset = new Vector2Int(1, 0);
                break;
            case 4: // ����
                offset = new Vector2Int(-1, 0);
                break;
            default:
                Debug.LogError("�ùٸ��� ���� openingDirection ��!");
                break;
        }
        Vector2Int spawnPos = parentPos + offset;

        GameObject roomPrefab = null;

        // �ִ� �� ���� ���� üũ
        if (MapManager.Instance.rooms.Count >= MapManager.Instance.maxRooms)
        {
            MapManager.Instance.OnMapRoomCountFull();
            // ���� �ִ� �� ������ �������� ���, �Ա��� 1���� �־ �� ��ȯ�� ���� �ʴ� �� ����.
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
            // �迭�� 1~������ �ε����� 2�� �̻��� �Ա��� �ִ� ���. ���� �÷��� �� ��� ���.
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

        // �̹� �ش� ��ǥ�� ���� �����Ǿ����� Ȯ��
        if (MapManager.Instance.IsRoomExistAt(spawnPos))
        {
            spawned = true;
            return;
        }

        // �� �� ����
        GameObject newRoom = Instantiate(roomPrefab, transform.position, roomPrefab.transform.rotation, MapManager.Instance.RoomParent);

        // �� �濡 Room ������Ʈ�� ���ٸ� �߰��ϰ�, �׸��� ��ǥ ����
        Room newRoomComp = newRoom.GetComponent<Room>();
        if (newRoomComp == null)
        {
            newRoomComp = newRoom.AddComponent<Room>();
        }
        newRoomComp.gridPos = spawnPos;

        // MapManager�� �� ���� ��ǥ ���
        MapManager.Instance.RegisterRoom(spawnPos);

        spawned = true;
    }

    /*
    // �ٸ� RoomSpawner�� �浹 �� ó�� (�ߺ� ���� ���� �� �� �� ���� ���� ���)
    void OnTriggerEnter2D(Collider2D other)
    {
        // RoomSpawner�� ������ �ٸ� ������Ʈ�� �浹���� �� ó���մϴ�.
        Room otherRoom = other.GetComponent<Room>();
        if (otherRoom != null)
        {
            Room currentRoom = GetComponentInParent<Room>();
            if (currentRoom != null && otherRoom != null)
            {
                // �� ���� ������ �浹�Ͽ� ����Ǿ����� MapManager�� ����մϴ�.
                MapManager.Instance.RegisterConnection(currentRoom.gridPos, otherRoom.gridPos);
            }
            spawned = true;
        }
    }
    */
}
