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
    public void spriteColorChange(SpriteRenderer sprite, WorkState state)
    {
        if (state == WorkState.EMPTY)
        {
            // �Ƃ肠�����F
            sprite.color = emptyColor;
        }
        else if (state == WorkState.WORKING)
        {
            // �Ƃ肠�����ΐF
            sprite.color = workingColor;
        }
        else if (state == WorkState.BURNING)
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
