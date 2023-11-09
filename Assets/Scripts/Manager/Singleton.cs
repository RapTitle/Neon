using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Singleton<T> : MonoBehaviour where T : MonoBehaviour
{
    private static T instance;
    public static T Instance
    {
        get 
        {
            if(instance == null)
            {
                instance=(T)FindAnyObjectByType(typeof(T));
                if(instance == null )
                {
                    GameObject obj = new GameObject();
                    instance = obj.AddComponent<T>();
                    obj.name = typeof(T).Name;
                    DontDestroyOnLoad(obj);
                }
            }
            return instance;
        }
    }

    protected virtual void Awake()
    {

    }
}
