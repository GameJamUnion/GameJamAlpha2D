using UnityEngine;

public class WorkColorManager : MonoBehaviour
{
    /// <summary>
    /// �ғ��Ȃ����̃J���[���
    /// </summary>
    [SerializeField]
    private Color emptyColor;

    /// <summary>
    /// ��ƒ��̃J���[���
    /// </summary>
    [SerializeField]
    private Color workingColor;

    /// <summary>
    /// ���㎞�̃J���[���
    /// </summary>
    [SerializeField]
    private Color burningColor;

    /// <summary>
    /// �Ȃɂ��Ȃ����̃J���[���
    /// </summary>
    [SerializeField]
    private Color nullColor;

    /// <summary>
    /// �J���[�ύX���\�b�h
    /// </summary>
    public void spriteColorChange(SpriteRenderer sprite, WorkCommon.WorkState state)
    {
        if (state == WorkCommon.WorkState.EMPTY)
        {
            // �Ƃ肠�����F
            sprite.color = emptyColor;
        }
        else if (state == WorkCommon.WorkState.WORKING)
        {
            // �Ƃ肠�����ΐF
            sprite.color = workingColor;
        }
        else if (state == WorkCommon.WorkState.BURNING)
        {
            // �Ƃ肠�����ԐF
            sprite.color = burningColor;
        }
        else
        {
            // �Ƃ肠�������F
            sprite.color = burningColor;
        }
    }
}
