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
    /// BGM�Đ�
    /// </summary>
    /// <param name="kind"></param>
    private void playBGM(BGMKind kind)
    {
        if (BGMTable == null)
        {
            Debug.LogError("BGMTable���ݒ肳��Ă��܂���B");
            return;
        }

        BGMTable.PlayBGM(kind);
    }

    /// <summary>
    /// BGM��~
    /// </summary>
    /// <param name="kind"></param>
    private void stopBGM(BGMKind kind)
    {
        if (BGMTable == null)
        {
            Debug.LogError("BGMTable���ݒ肳��Ă��܂���B");
            return;
        }

    }

    /// <summary>
    /// SE�Đ�
    /// </summary>
    /// <param name="kind"></param>
    private void playSE(SEKind kind) 
    {
        if (SETable == null)
        {
            Debug.LogError("SETable���ݒ肳��Ă��܂���B");
            return;
        }

        SETable.PlaySE(kind);
    }

    /// <summary>
    /// SE��~
    /// </summary>
    /// <param name="kind"></param>
    private void stopSE(SEKind kind)
    {
        if (SETable == null)
        {
            Debug.LogError("SETable���ݒ肳��Ă��܂���B");
            return;
        }

        SETable.StopSE(kind);
    }
}
