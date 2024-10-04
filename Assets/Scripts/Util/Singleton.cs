using Unity.VisualScripting;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance = null;

    public static T instance
    {
        get
        {
            if (_instance == null) 
            {
                _instance = FindAnyObjectByType<T>(FindObjectsInactive.Include);

                if (_instance == null)
                { 
                    _instance = new GameObject(typeof(T).Name, typeof(T)).GetComponent<T>();
                }
            }
            return _instance;
        }
    }

    protected virtual void Awake()
    {
        if (transform.parent != null && transform.root != null) 
        {
            DontDestroyOnLoad(transform.root.gameObject);
        }
        else
        {
            DontDestroyOnLoad(gameObject); 
        }

    }
}