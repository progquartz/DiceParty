using UnityEngine;

public class SingletonBehaviour<T> : MonoBehaviour where T : SingletonBehaviour<T>
{
    // �� ��ȯ�� �ı� ���� ���� ����.
    protected bool IsDestroyOnLoad { get; set; } = false;

    private static T s_instance;

    public static T Instance
    {
        // �ʰ� ã�� ��� ȣ��Ǹ� ����.
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
            Init();  // ��ӹ��� Ŭ�������� ���ϴ� �ʱ�ȭ �۾��� ������ �� �ְ� �����.
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
    /// �̱��� �ʱ�ȭ�� �޼���.
    /// </summary>
    protected virtual void Init()
    {
        // �⺻ �ʱ�ȭ ����(�ʿ� �� ��ӹ��� Ŭ�������� �������̵�)
    }

    protected virtual void OnDestroy()
    {
        if (s_instance == this)
        {
            s_instance = null;
        }
    }
}
