using UnityEngine;


#region Base


/// <summary>
/// ステート基底
/// 排他でステートを切り替える
/// シーン制御に利用
/// </summary>
public abstract class SceneStateBase
{
    public abstract SceneNames[] SceneName { get; }

    public void OnEnter()
    {
        for (int i = 0; i < SceneName.Length; i++)
        {
            // 対象シーンロード
            SceneManager.Instance.loadScene(SceneName[i]);
        }
        
    }

    public void OnExit()
    {
        for (int i = 0; i < SceneName.Length; i++)
        {
            // 対象シーンアンロード
            SceneManager.Instance.unloadScene(SceneName[i]);
        }        
    }

    /// <summary>
    /// 次のステートに進むかどうかをチェックする
    /// </summary>
    /// <returns></returns>
    public virtual SceneStateBase checkNext()
    {
        return checkNextCommon();
    }

    public virtual SceneStateBase checkNextCommon()
    {
        return null;
    }
}
#endregion Base

#region Title

/// <summary>
/// タイトルシーン制御
/// </summary>
public class TitleSceneState : SceneStateBase
{
    public override SceneNames[] SceneName => new SceneNames[] { SceneNames.Title };

    public override SceneStateBase checkNext()
    {
        var result = base.checkNext();
        
        if (checkInput() == true)
        {
            // インゲームシーンへ
            return new InGameSceneState0();
        }

        return result;
    }

    /// <summary>
    /// キー入力を見る
    /// </summary>
    /// <returns></returns>
    private bool checkInput()
    {
        // マウスクリック
        if (Input.GetMouseButtonDown(0))
        {
            return true;
        }

        // キー入力
        if (Input.anyKeyDown == true)
        {
            return true;
        }
        return false;
    }
}

#endregion Title

#region InGame
public abstract class InGameSceneStateBase : SceneStateBase
{
}

public class InGameSceneState0 : InGameSceneStateBase
{
    public override SceneNames[] SceneName => new SceneNames[] { SceneNames.komugi_workshop, SceneNames.dev_ozaki };
    public override SceneStateBase checkNext()
    {
        var result = base.checkNext();
        
        return result;
    }
}
#endregion InGame
