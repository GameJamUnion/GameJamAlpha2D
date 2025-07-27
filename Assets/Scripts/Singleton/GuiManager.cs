using UnityEngine;
using System.Collections.Generic;
using System.ComponentModel;

public class GuiManager : SingletonBase<GuiManager>
{
    public enum GuiType
    {
        Invalid = -1,
        Pause,
        GameOver,

        [Browsable(false)]
        MaxNum,
    }

    private Dictionary<GuiType, GuiControllerBase> _GuiControllerDict = new Dictionary<GuiType, GuiControllerBase>((int)GuiType.MaxNum);

    public void registerGui(GuiType type, GuiControllerBase gui)
    {
        // GUI�̓o�^����
        _GuiControllerDict[type] = gui;
    }

    public override void OnDestroy()
    {
        
        _GuiControllerDict.Clear();
    }

    /// <summary>
    /// �w��Gui�J�����N�G�X�g
    /// </summary>
    /// <param name="type"></param>
    public void requestOpenGui(GuiType type, OpenParamBase param)
    {
        // �Ƃ肠�����J��
        open(type, param);
    }

    /// <summary>
    /// �w��Gui���郊�N�G�X�g
    /// </summary>
    /// <param name="type"></param>
    public void requestCloseGui(GuiType type)
    {
        close(type);
    }

    /// <summary>
    /// Gui�J������
    /// </summary>
    private void open(GuiType type, OpenParamBase param)
    {
        if (_GuiControllerDict.TryGetValue(type, out GuiControllerBase guiController))
        {
            // GUI�̃p�l�����A�N�e�B�u�ɂ���
            guiController.setActive(true);
            guiController.onOpen(param);
        }
    }


    /// <summary>
    /// Gui���郊�N�G�X�g
    /// </summary>
    private void close(GuiType type)
    {
        if (_GuiControllerDict.TryGetValue(type, out GuiControllerBase guiController))
        {
            // GUI�̃p�l�����A�N�e�B�u�ɂ���
            guiController.setActive(false);
            guiController.onClose();
        }
    }
}

public class OpenParamBase
{

}

public abstract class GuiControllerBase : MonoBehaviour
{
    public abstract GuiManager.GuiType GuiType { get; }

    [DisplayName("Panel�I�u�W�F�N�g")]
    public GameObject[] GuiPanelObject
    {
        get => _GuiPanelObject;
        set => _GuiPanelObject = value;
    }
    [SerializeField]
    private GameObject[] _GuiPanelObject = null;


    private void Awake()
    {
        // GUI�̓o�^
        GuiManager.Instance.registerGui(GuiType, this);

        setActive(false);
    }

    /// <summary>
    /// Gui�J�����Ƃ�
    /// </summary>
    public virtual void onOpen(OpenParamBase openParam = null)
    {

    }
    
    /// <summary>
    /// Gui�����Ƃ�
    /// </summary>
    public virtual void onClose()
    {

    }

    /// <summary>
    /// 
    /// </summary>
    /// <param name="set"></param>
    public void setActive(bool set)
    {
        for (int i = 0; i < GuiPanelObject.Length; i++)
        {
            var panel = GuiPanelObject[i];
            if (panel == null)
            {
                continue;
            }
            // ������Ԃ͔�A�N�e�B�u
            panel.SetActive(set);
        }
    }
}
