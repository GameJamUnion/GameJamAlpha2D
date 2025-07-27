using UnityEngine;

public class WorkColorManager : MonoBehaviour
{
    /// <summary>
    /// 稼働なし時のカラー情報
    /// </summary>
    [SerializeField]
    private Color emptyColor;

    /// <summary>
    /// 作業中のカラー情報
    /// </summary>
    [SerializeField]
    private Color workingColor;

    /// <summary>
    /// 炎上時のカラー情報
    /// </summary>
    [SerializeField]
    private Color burningColor;

    /// <summary>
    /// なにもなし時のカラー情報
    /// </summary>
    [SerializeField]
    private Color nullColor;

    /// <summary>
    /// カラー変更メソッド
    /// </summary>
    public void spriteColorChange(SpriteRenderer sprite, WorkCommon.WorkState state)
    {
        if (state == WorkCommon.WorkState.EMPTY)
        {
            // とりあえず青色
            sprite.color = emptyColor;
        }
        else if (state == WorkCommon.WorkState.WORKING)
        {
            // とりあえず緑色
            sprite.color = workingColor;
        }
        else if (state == WorkCommon.WorkState.BURNING)
        {
            // とりあえず赤色
            sprite.color = burningColor;
        }
        else
        {
            // とりあえず白色
            sprite.color = burningColor;
        }
    }
}
