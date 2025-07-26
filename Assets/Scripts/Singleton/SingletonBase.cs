using UnityEngine;

public class SingletonBase<T> where T : SingletonBase<T> , new()
{
    private static T _Instance;
    public static T Instance
    {
        get
        {
            if (_Instance == null)
            {
                _Instance = new T();
            }

            return _Instance;
        }
    }

    public SingletonBase()
    {
    }
}
