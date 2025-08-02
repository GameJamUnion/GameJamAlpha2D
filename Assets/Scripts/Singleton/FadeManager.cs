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
    /// �o�^
    /// </summary>
    /// <param name="fadeInOut"></param>
    public void registerFade(FadeInOut fadeInOut)
    {
        _FadeInOut = fadeInOut;
    }

    /// <summary>
    /// �t�F�[�h�J�n
    /// </summary>
    /// <param name="type"> �t�F�[�h�^�C�v </param>
    public void requestStartFade(FadeInOut.FadeInOutArgs args)
    {
        _FadeInOut.requestStartFade(args);
    }
    #endregion
}
