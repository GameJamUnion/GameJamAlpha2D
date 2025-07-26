using UnityEngine;
using UnityEngine.UI;
using TMPro;

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

    [Header("StampCustomButton")]
    [SerializeField] StampCustomButton _stampCustomButton;

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
    }

    public void OnStamp()
    {
        _baseResume.OnStamp = true;
        _baseResume.OnStampProcess();
        _officeGameMaster.Employment(_baseResume.BaseUnit);
    }
}
