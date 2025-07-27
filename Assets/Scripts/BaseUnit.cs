using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    // ���O
    [SerializeField] private string _name = "NoName";
    // ��J�x
    [SerializeField] private float _fatigueLevel = 0f;
    // ���C
    [SerializeField] private float _motivationLevel = 0f;

    // �e�Z�N�V�����̐��Y����(%)
    [SerializeField] private float _productionEfficiency1 = 1f;
    [SerializeField] private float _productionEfficiency2 = 1f;
    [SerializeField] private float _productionEfficiency3 = 1f;

    // �R���w��
    [SerializeField] private float _liarIndex = 0f;

    // �z��
    [SerializeField] private RI.PlacementState _placementState = RI.PlacementState.Section1;

    [SerializeField] private BaseResume _baseResume;
    [SerializeField] private ResumeData _resumeData;
    [SerializeField] private ResumeInterface _resumeInterface;

    #region �v���p�e�B
    public string Name
    {
        get { return _name; }
    }
    public ResumeInterface ResumeInterface
    {
        set { _resumeInterface = value; }
    }
    public RI.PlacementState PlacementState
    {
        get { return _placementState; }
    }
    #endregion

    void Start()
    {

    }

    void Update()
    {
        
    }

    // ���j�b�g�̏���������
    public void Initialize(OfficeGameMaster officeGameMaster)
    {
        _baseResume.SetResume(this);
        _baseResume.SetOfficeGameMaster(officeGameMaster);
        _resumeInterface.Initialize(_baseResume);
    }

    // �e�K���̂����P���w�肵�ĕ\������e�L�X�g�ɕύX���鏈�� float > string
    public string GetProductionEfficiencyRank(ProductionEfficiency productionEfficiency)
    {
        float productionEfficiencyNum = 0f;

        switch (productionEfficiency)
        {
            case ProductionEfficiency.SECTION_1:
                productionEfficiencyNum = _productionEfficiency1;
                break;

            case ProductionEfficiency.SECTION_2:
                productionEfficiencyNum = _productionEfficiency2;
                break;

            case ProductionEfficiency.SECTION_3:
                productionEfficiencyNum = _productionEfficiency3;
                break;

            default:
                Debug.Log("#Elalice:BaseUnit > GetProductionEfficiencyRank /// Error");
                break;
        }

        if(_resumeData.RankS.MinProductionEfficiency <= productionEfficiencyNum)
        {
            return _resumeData.RankS.ScoreStr;
        }
        else if (_resumeData.RankA.MinProductionEfficiency <= productionEfficiencyNum)
        {
            return _resumeData.RankA.ScoreStr;
        }
        else if (_resumeData.RankB.MinProductionEfficiency <= productionEfficiencyNum)
        {
            return _resumeData.RankB.ScoreStr;
        }
        else if (_resumeData.RankC.MinProductionEfficiency <= productionEfficiencyNum)
        {
            return _resumeData.RankC.ScoreStr;
        }
        else if (_resumeData.RankD.MinProductionEfficiency <= productionEfficiencyNum)
        {
            return _resumeData.RankD.ScoreStr;
        }
        else if (_resumeData.RankE.MinProductionEfficiency <= productionEfficiencyNum)
        {
            return _resumeData.RankE.ScoreStr;
        }
        else
        {
            return _resumeData.RankF.ScoreStr;
        }
    }

    /// <summary>
    /// �z����̕ύX
    /// </summary>
    public void ChangePlacementState()
    {
        int num = (int)_placementState;
        num++;
        if (num >= (int)RI.PlacementState.END)
        {
            num = 1;
        }
        _placementState = (RI.PlacementState)num;
    }
}

public enum ProductionEfficiency
{
    SECTION_1,
    SECTION_2,
    SECTION_3
}
