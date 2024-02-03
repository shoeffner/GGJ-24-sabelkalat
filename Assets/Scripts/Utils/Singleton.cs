using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : Component
{

    private static T _instance;
    protected static bool HasInstance => _instance != null;

    public static T Instance
    {
        get
        {
          return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (_instance != null)
        {
            Destroy(gameObject);
            return;
        }

        _instance = this as T;
    }
}