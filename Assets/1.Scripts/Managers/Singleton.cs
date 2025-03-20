using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    private static readonly object lockObj = new();

    [Tooltip("Scene�̵� true �ı�/ false ��ȣ")]
    [SerializeField] protected bool isDestroyOnLoad = false;

    public static T Instance
    {
        get
        {
            if (instance == null)
            {
                lock (lockObj)
                {
                    instance = FindFirstObjectByType<T>();

                    if (instance == null)
                    {
                        GameObject go = new(typeof(T).Name);
                        instance = go.AddComponent<T>();
                    }
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {
        lock (lockObj)
        {
            if (instance == null)
            {
                instance = this as T;

                if (!isDestroyOnLoad)
                    DontDestroyOnLoad(gameObject);
            }
            else if (instance != this) Destroy(gameObject);
        }
    }

    private void OnDestroy()
    {
        if (instance == this)
        {
            instance = null;
        }
    }
}