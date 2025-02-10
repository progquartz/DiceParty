using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Rendering;

public class MapManager : SingletonBehaviour<MapManager>
{
    public int currentStageNum = 0;

    [SerializeField] private RoomTemplates _roomTemplate;
    [SerializeField] private Transform roomParent;

    public List<GameObject> rooms;
    private int roomCount { get { return rooms.Count; } }

    public float waitTime;
    private bool spawnedBoss;

    private void Awake()
    {
        // ���� ����� ������ �׳� ����.
        IsDestroyOnLoad = true;
        Init();
    }

    void Update()
    {

        if (waitTime <= 0 && spawnedBoss == false)
        {
            // ���������� ��ȯ�� ���� ���, ���� �� �� �ִ� ���ɼ��� ����.
            Instantiate(RoomTemplates.boss, rooms.Last<GameObject>().transform.position, Quaternion.identity);
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
        // ���� �� ��ȯ�Ǹ�... �� ����������, �ش� �����͸� �����ͺ��̽��� �����Ű�ų� ���� �������� ����� ����.
        // �������� �ε� / ����� ��츦 ���� ����.
    }

    protected override void Init()
    {
        base.Init();
        InitializeData();
    }

    private void InitializeData()
    {

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


}
