using UnityEngine;

public class SingletonBase<T> : ISingleton  where T : SingletonBase<T> , new()
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
        if (this is not GlobalServiceManager)
        {
            GlobalServiceManager.Instance.registerSingleton(this);
        }
    }

    ~SingletonBase()
    {

    }

    /// <summary>
    /// シングルトン生成時呼ばれる
    /// </summary>
    public virtual void Start()
    {

    }

    /// <summary>
    /// Update
    /// </summary>
    public virtual void Update()
    {

    }

    /// <summary>
    /// LateUpdate
    /// </summary>
    public virtual void LateUpdate()
    {

    }

    /// <summary>
    /// OnDestroy
    /// </summary>
    public virtual void OnDestroy()
    {
        // インスタンス削除
        if (_Instance != null)
        {
            _Instance = null;
        }
    }
}

public interface ISingleton
{
    public void Start();
    public void Update();
    public void LateUpdate();
    public void OnDestroy();
}
