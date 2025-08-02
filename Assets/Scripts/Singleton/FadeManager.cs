using Unity.Collections;
using UnityEngine;

public class FadeManager : SingletonBase<FadeManager>
{

    #region Definition
    public enum FadeType
    {
        None,
        FadeIn, 
        FadeOut,
    }
    #endregion

    #region Field
    private FadeInOut _FadeInOut = null;
    #endregion

    #region Method
    /// <summary>
    /// 登録
    /// </summary>
    /// <param name="fadeInOut"></param>
    public void registerFade(FadeInOut fadeInOut)
    {
        _FadeInOut = fadeInOut;
    }

    /// <summary>
    /// フェード開始
    /// </summary>
    /// <param name="type"> フェードタイプ </param>
    public void requestStartFade(FadeInOut.FadeInOutArgs args)
    {
        _FadeInOut.requestStartFade(args);
    }
    #endregion
}
