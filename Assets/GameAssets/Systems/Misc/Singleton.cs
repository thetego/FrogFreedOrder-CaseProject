using Unity.VisualScripting;
using UnityEngine;

public abstract class Singleton<T> : MonoBehaviour where T: Singleton<T>
{

    public bool dontDestroy;
    public static bool IsTemporaryInstance { private set; get; }
    private static T _instance;

    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType(typeof(T)).GetComponent<T>();

                if (_instance = null)
                {
                    Debug.LogWarning("No instance of " + typeof(T).ToString() + ", a temporary one is created.");

                    IsTemporaryInstance = true;
                    
                    _instance = new GameObject("Temp Instance of " + typeof(T).ToString(), typeof(T)).GetComponent<T>();
                    //_instance.hideFlags = HideFlags.DontSave;

                    if (_instance == null)
                    {
                        Debug.LogError("Problem during the creation of " + typeof(T).ToString());
                    }
                }
                
                if (!_isInitialized)
                {
                    _isInitialized = true;
                    if (_instance != null) _instance.Init();
                }
            }
            return _instance;
        }
    }
    private static bool _isInitialized;
    void Awake()
    {
        if (_instance == null)
        {
             _instance = this as T;
        }
        else if (_instance != this)
        {
            Debug.LogError("Another instance of " + GetType() + " is already exist! Destroying self...");
            DestroyImmediate(this);
            return;
        }
        
         if (!_isInitialized)
        {
            if (dontDestroy)
                DontDestroyOnLoad(gameObject);

            _isInitialized = true;
            if (_instance != null) _instance.Init();
        }
        

    }
    public virtual void Init()
    {
    }
    private void OnApplicationQuit()
    {
        _instance = null;
    }
}