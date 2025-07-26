using UnityEngine;

/// <summary>
///  作業場シーンメイン処理
/// </summary>
public class WorkShopSceneMain : MonoBehaviour
{
    /// <summary>
    /// 処理計測用time変数
    /// </summary>
    private float time;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {

    }

    // Update is called once per frame
    private void Update()
    {
        time += Time.deltaTime;
        if (time >= 1.0f)
        {
            time = 0.0f;
            // 1秒毎に実行する処理をここに記載

        }

        // 毎フレーム実行する処理をここに記述

    }
}
