using UnityEngine;

public class RoomSpawner : MonoBehaviour
{
    public int openingDirection;
    // 1,2,3,4 // -> 아래, 위, 왼, 오

    private RoomTemplates templates;
    private int rand;
    public bool spawned = false;

    public float waitTime = 4f;

    void Start()
    {
        Destroy(gameObject, waitTime);
        templates = MapManager.Instance.RoomTemplates;        
        Invoke("Spawn", 0.1f);
    }


    void Spawn()
    {
        if (spawned == false)
        {
            if (openingDirection == 1)
            {
                // 아래에 문 있어서 소환
                rand = Random.Range(0, templates.bottomRooms.Length);
                Instantiate(templates.bottomRooms[rand], transform.position, templates.bottomRooms[rand].transform.rotation, MapManager.Instance.RoomParent);
            }
            else if (openingDirection == 2)
            {
                // 위에 문 있어서 소환
                rand = Random.Range(0, templates.topRooms.Length);
                Instantiate(templates.topRooms[rand], transform.position, templates.topRooms[rand].transform.rotation, MapManager.Instance.RoomParent);
            }
            else if (openingDirection == 3)
            {
                // 왼쪽에 문 있어서 소환
                rand = Random.Range(0, templates.leftRooms.Length);
                Instantiate(templates.leftRooms[rand], transform.position, templates.leftRooms[rand].transform.rotation, MapManager.Instance.RoomParent);
            }
            else if (openingDirection == 4)
            {
                // 오른쪽에 문 있어서 소환
                rand = Random.Range(0, templates.rightRooms.Length);
                Instantiate(templates.rightRooms[rand], transform.position, templates.rightRooms[rand].transform.rotation, MapManager.Instance.RoomParent);
            }
            spawned = true;
        }
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        RoomSpawner roomspawner;
        if (other.TryGetComponent<RoomSpawner>(out roomspawner))
        {
            if(roomspawner.spawned == false && spawned == false)
            {
                Instantiate(templates.closedRoom, transform.position, Quaternion.identity, MapManager.Instance.RoomParent);
                Destroy(gameObject);
            }
            spawned = true;
        }
    }
}
