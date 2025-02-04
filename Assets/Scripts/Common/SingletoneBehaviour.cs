using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    // 씬 전환시 파괴 여부 설정 가능.
    protected bool IsDestroyOnLoad { get; set; } = false;

    private static T s_instance;

    public static T Instance
    {
        // 늦게 찾을 경우 호출되며 생성.
        get
        {
            if (s_instance == null)
            {
                s_instance = FindObjectOfType<T>();

                if (s_instance == null)
                {
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    s_instance = singletonObject.AddComponent<T>();
                }
            }
            return s_instance;
        }
    }

    protected virtual void Awake()
    {
        if (s_instance == null)
        {
            s_instance = this as T;
            Init();  // 상속받은 클래스에서 원하는 초기화 작업을 수행할 수 있게 만들기.
            if (!IsDestroyOnLoad)
            {
                DontDestroyOnLoad(gameObject);
            }
        }
        else if (s_instance != this)
        {
            Destroy(gameObject);
        }
    }

    /// <summary>
    /// 싱글톤 초기화용 메서드.
    /// </summary>
    protected virtual void Init()
    {
        // 기본 초기화 로직(필요 시 상속받은 클래스에서 오버라이드)
    }

    protected virtual void OnDestroy()
    {
        if (s_instance == this)
        {
            s_instance = null;
        }
    }
}
