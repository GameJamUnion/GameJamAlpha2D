using UnityEngine;
using UnityEngine.UI;
using TMPro;

// 履歴書インターフェイスが持っている
public class ResumeInterface : MonoBehaviour
{
    [SerializeField] OfficeGameMaster _officeGameMaster;

    [SerializeField] bool _active = false;

    [SerializeField] BaseResume _baseResume;
    [SerializeField] TextMeshProUGUI _tName;
    [SerializeField] TextMeshProUGUI _tSection1Rank;
    [SerializeField] TextMeshProUGUI _tSection2Rank;
    [SerializeField] TextMeshProUGUI _tSection3Rank;
    [SerializeField] GameObject _stamp;

    [Header("アタッチ")]
    [SerializeField] StampCustomButton _stampCustomButton;
    [SerializeField] ResumeInterfaceCustomButton _resumeInterfaceCustomButton;
    [SerializeField] RIStampManager _riStampManager;
    [SerializeField] RIPlacementManager _riPlacementManager;

    #region プロパティ
    public BaseResume BaseResume
    {
        get { return _baseResume; }
    }
    public bool Active
    {
        get { return _active; }
    }
    #endregion

    private void Update()
    {
        if (_resumeInterfaceCustomButton.HoldTap)
        {
            Hold();
        }
    }

    /// <summary>
    /// 初期化
    /// </summary>
    public void Initialize(BaseResume baseResume)
    {
        _active = true;
        _baseResume = baseResume;
        if (_baseResume != null)
        {
            SetResumeInterface();
        }
        else
        {
            Debug.Log("#Elalice:ResumeInterface > Initialize /// Error");
        }
    }
   
    private void SetResumeInterface()
    {
        gameObject.SetActive(_active);

        _tName.text = _baseResume.Name;
        _tSection1Rank.text = _baseResume.ProductionEfficiency1;
        _tSection2Rank.text = _baseResume.ProductionEfficiency2;
        _tSection3Rank.text = _baseResume.ProductionEfficiency3;

        _stamp.SetActive(_baseResume.OnStamp);
        _stamp.GetComponent<RectTransform>().localRotation = Quaternion.Euler(0f, 0f, _baseResume.StampRotation);

        // セクションのUI初期化
        RIPlacementUpdate();
    }

    public void OnStamp()
    {
        // スタンプボタンが押された場合の処理
        _baseResume.OnStamp = true;
        _baseResume.OnStampProcess();
        _officeGameMaster.Employment(_baseResume.BaseUnit);
    }
    public void SectionChange()
    {
        _baseResume.BaseUnit.ChangePlacementState();
        RIPlacementUpdate();
    }
    public void Close()
    {
        
    }
    public void Hold()
    {
        // マウスの移動距離取得
        Vector3 newPos = _resumeInterfaceCustomButton.CurrentMousePos - Input.mousePosition;
        // 移動距離分座標をずらす
        this.gameObject.transform.localPosition = _resumeInterfaceCustomButton.SaveRusumeInterfacePos - newPos;
    }

    // Update
    void RIPlacementUpdate()
    {
        _riPlacementManager.ChangePlacementState(_baseResume.BaseUnit.PlacementState);
    }
}
