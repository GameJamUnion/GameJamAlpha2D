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

    [Header("最大移動距離")]
    [SerializeField] Vector2 _maxTopBottom;
    [SerializeField] Vector2 _maxRightLeft;

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
    public bool HoldFlg
    {
        get { return _resumeInterfaceCustomButton.HoldTap; }
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

    public void SetDefault()
    {
        _tName.text = "NoData";
        _tSection1Rank.text = "-No Data-";
        _tSection2Rank.text = "-No Data-";
        _tSection3Rank.text = "-No Data-";
        _riPlacementManager.ChangePlacementState(RI.PlacementState.NONE);
        _stamp.SetActive(false);
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

        DisplayAtTheTop();
        // セクションのUI初期化
        RIPlacementUpdate(true);
    }

    public void OnStamp()
    {
        // スタンプボタンが押された場合の処理
        _baseResume.OnStamp = true;
        _baseResume.OnStampProcess();

        SoundManager.Instance.requestPlaySound(SEKind.Stamp);

        DisplayAtTheTop();
    }
    public void SectionChange()
    {
        _baseResume.BaseUnit.ChangePlacementState();
        RIPlacementUpdate(false);

        DisplayAtTheTop();
    }
    public void Close()
    {
        _active = false;
    }
    public void Hold()
    {
        // マウスの移動距離取得
        Vector3 newPos = _resumeInterfaceCustomButton.SaveRusumeInterfacePos - (_resumeInterfaceCustomButton.CurrentMousePos - Input.mousePosition);
        // 範囲外チェック
        newPos.x = Mathf.Clamp(newPos.x, _maxRightLeft.y, _maxRightLeft.x);
        newPos.y = Mathf.Clamp(newPos.y, _maxTopBottom.y, _maxTopBottom.x);

        // 移動距離分座標をずらす
        this.gameObject.GetComponent<RectTransform>().transform.localPosition =  newPos;

        DisplayAtTheTop();
    }
    public void Rejected()
    {
        _active = false;
        gameObject.SetActive(false);

        _officeGameMaster.Reject(_baseResume.BaseUnit);
    }

    // Update
    void RIPlacementUpdate(bool init)
    {
        if (_officeGameMaster.AmIInTheRoom(_baseResume.BaseUnit) || init)
        {
            _riPlacementManager.ChangePlacementState(_baseResume.BaseUnit.PlacementState);
        }
        else
        {
            Debug.Log("今部屋にいないからまずは呼ばないと...");
        }
    }

    public void DisplayAtTheTop()
    {
        transform.SetSiblingIndex(transform.parent.childCount - 1);
    }
}
