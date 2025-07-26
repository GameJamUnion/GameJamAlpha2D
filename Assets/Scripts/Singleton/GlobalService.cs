using UnityEngine;
using System;
using System.Collections;
using System.Runtime.CompilerServices;

public class GlobalService : MonoBehaviour
{
    private void Start()
    {
        GlobalServiceManager.Instance.onStart();
    }
    private void Update()
    {
        GlobalServiceManager.Instance.onUpdate();
    }
    private void LateUpdate()
    {
        GlobalServiceManager.Instance.onLateUpdate();
    }

    private void OnDestroy()
    {
        GlobalServiceManager.Instance.onDestroy();
    }
}

/// <summary>
/// シングルトン統括クラス
/// </summary>
public class GlobalServiceManager : SingletonBase<GlobalServiceManager>
{
    private Action StartEvents;
    private Action UpdateEvents;
    private Action LateUpdateEvents;
    private Action OnDestroyEvents;

    /// <summary>
    /// 処理登録
    /// </summary>
    /// <param name="singleton"></param>
    public void registerSingleton(ISingleton singleton)
    {
        StartEvents += singleton.Start;
        UpdateEvents += singleton.Update;
        LateUpdateEvents += singleton.LateUpdate;
        OnDestroyEvents += singleton.OnDestroy;
    }


    public void onStart()
    {
        StartEvents?.Invoke();
    }

    public void onUpdate()
    {
        UpdateEvents?.Invoke();
    }

    public void onLateUpdate()
    {
        LateUpdateEvents?.Invoke();
    }

    public void onDestroy()
    {
        OnDestroyEvents?.Invoke();
       
        // 登録をすべてクリア
        StartEvents = null;
        UpdateEvents = null;
        LateUpdateEvents = null;
        OnDestroyEvents = null;
    }
}