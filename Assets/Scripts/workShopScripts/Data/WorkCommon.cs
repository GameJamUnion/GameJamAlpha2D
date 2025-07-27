using UnityEngine;

namespace WorkCommon
{
    /// <summary>
    /// 作業場の状態
    /// </summary>
    public enum WorkState
    {
        /// <summary>
        /// 稼働無し
        /// </summary>
        EMPTY,

        /// <summary>
        /// 作業中
        /// </summary>
        WORKING,

        /// <summary>
        /// 炎上中
        /// </summary>
        BURNING
    }

    /// <summary>
    /// 作業員の状態
    /// </summary>
    public enum WorkerState
    {
        /// <summary>
        /// 仕事中
        /// </summary>
        WORKING,

        /// <summary>
        /// 走ってる
        /// </summary>
        RUNNING,

        /// <summary>
        /// 呼び出し中
        /// </summary>
        COLL,
        
        /// <summary>
        /// 解雇
        /// </summary>
        REMOVE
    }

}
