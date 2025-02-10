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
        // 씬이 변경될 때마다 그냥 삭제.
        IsDestroyOnLoad = true;
        Init();
    }

    void Update()
    {

        if (waitTime <= 0 && spawnedBoss == false)
        {
            // 마지막으로 소환된 방의 경우, 가장 멀 수 있는 가능성이 존재.
            Instantiate(RoomTemplates.boss, rooms.Last<GameObject>().transform.position, Quaternion.identity);
        }
        else
        {
            waitTime -= Time.deltaTime;
        }
        // 만약 다 소환되면... 방 마무리짓고, 해당 데이터를 데이터베이스에 저장시키거나 파일 형식으로 만들어 보관.
        // 스테이지 로드 / 재실행 경우를 위해 저장.
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
