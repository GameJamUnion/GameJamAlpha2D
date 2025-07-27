using System;
using UnityEngine;

public class SoundManagerBehavior : MonoBehaviour
{
    public BGMReferenceTalble BGMTable
    {
        get => _BGMTable;
        set => _BGMTable = value;
    }
    [SerializeField]
    private BGMReferenceTalble _BGMTable = null;


    public SEReferenceTable SETable
    {
        get => _SETable;
        set => _SETable = value;
    }
    [SerializeField]
    private SEReferenceTable _SETable = null;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        SoundManager.Instance.registerSoundTable(playBGM, stopBGM, playSE, stopSE);
    }

    /// <summary>
    /// BGM再生
    /// </summary>
    /// <param name="kind"></param>
    private void playBGM(BGMKind kind)
    {
        if (BGMTable == null)
        {
            Debug.LogError("BGMTableが設定されていません。");
            return;
        }

        BGMTable.PlayBGM(kind);
    }

    /// <summary>
    /// BGM停止
    /// </summary>
    /// <param name="kind"></param>
    private void stopBGM(BGMKind kind)
    {
        if (BGMTable == null)
        {
            Debug.LogError("BGMTableが設定されていません。");
            return;
        }

    }

    /// <summary>
    /// SE再生
    /// </summary>
    /// <param name="kind"></param>
    private void playSE(SEKind kind) 
    {
        if (SETable == null)
        {
            Debug.LogError("SETableが設定されていません。");
            return;
        }

        SETable.PlaySE(kind);
    }

    /// <summary>
    /// SE停止
    /// </summary>
    /// <param name="kind"></param>
    private void stopSE(SEKind kind)
    {
        if (SETable == null)
        {
            Debug.LogError("SETableが設定されていません。");
            return;
        }

        SETable.StopSE(kind);
    }
}
