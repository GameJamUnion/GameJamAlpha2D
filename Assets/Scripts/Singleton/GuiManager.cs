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
        // GUIの登録処理
        _GuiControllerDict[type] = gui;
    }

    public override void OnDestroy()
    {
        
        _GuiControllerDict.Clear();
    }

    /// <summary>
    /// 指定Gui開くリクエスト
    /// </summary>
    /// <param name="type"></param>
    public void requestOpenGui(GuiType type, OpenParamBase param)
    {
        // とりあえず開く
        open(type, param);
    }

    /// <summary>
    /// 指定Gui閉じるリクエスト
    /// </summary>
    /// <param name="type"></param>
    public void requestCloseGui(GuiType type)
    {
        close(type);
    }

    /// <summary>
    /// Gui開く処理
    /// </summary>
    private void open(GuiType type, OpenParamBase param)
    {
        if (_GuiControllerDict.TryGetValue(type, out GuiControllerBase guiController))
        {
            // GUIのパネルをアクティブにする
            guiController.setActive(true);
            guiController.onOpen(param);
        }
    }


    /// <summary>
    /// Gui閉じるリクエスト
    /// </summary>
    private void close(GuiType type)
    {
        if (_GuiControllerDict.TryGetValue(type, out GuiControllerBase guiController))
        {
            // GUIのパネルを非アクティブにする
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

    [DisplayName("Panelオブジェクト")]
    public GameObject[] GuiPanelObject
    {
        get => _GuiPanelObject;
        set => _GuiPanelObject = value;
    }
    [SerializeField]
    private GameObject[] _GuiPanelObject = null;


    private void Awake()
    {
        // GUIの登録
        GuiManager.Instance.registerGui(GuiType, this);

        setActive(false);
    }

    /// <summary>
    /// Gui開いたとき
    /// </summary>
    public virtual void onOpen(OpenParamBase openParam = null)
    {

    }
    
    /// <summary>
    /// Gui閉じたとき
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
            // 初期状態は非アクティブ
            panel.SetActive(set);
        }
    }
}
