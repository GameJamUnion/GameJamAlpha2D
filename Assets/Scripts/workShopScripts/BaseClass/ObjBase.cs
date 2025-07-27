using UnityEngine;
using System.Collections.Generic;

public abstract class ObjBase : MonoBehaviour
{
    /// <summary>
    /// 処理計測用time変数
    /// </summary>
    private float time;

    /// <summary>
    /// 作業実行間隔
    /// </summary>
    [SerializeField]
    private float workInterval;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    virtual protected void Start()
    {
        time = 0.0f;
    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;
        if (time >= workInterval)
        {
            time = 0.0f;

            // 指定秒毎の作業実行処理
            workPerSeconds();
        }

        // フレーム毎の作業実行処理
        workPerFlame();
    }

    /// <summary>
    /// 指定秒毎の作業実行
    /// </summary>
    protected abstract void workPerSeconds();

    /// <summary>
    /// フレーム毎の作業実行
    /// </summary>
    protected abstract void workPerFlame();
}
