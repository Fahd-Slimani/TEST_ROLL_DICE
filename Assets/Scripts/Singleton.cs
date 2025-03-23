using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T _instance;

    // Property of the instance
    public static T Instance
    {
        get
        {
            if (_instance == null)
            {
                // Find the instance if not already set
                _instance = FindFirstObjectByType<T>();

                if (_instance == null)
                {
                    // If no instance is found, create a new GameObject with the Singleton attached
                    GameObject singletonObject = new GameObject(typeof(T).Name);
                    _instance = singletonObject.AddComponent<T>();
                }
            }
            return _instance;
        }
    }

    // Make sur that there is only one instance of the singleton in the scene
    private void Awake()
    {
        // If the instance already exists and it's not this instance, destroy the new object
        if (_instance != null && _instance != this)
            Destroy(gameObject);
        else
            _instance = this as T;
    }
}
