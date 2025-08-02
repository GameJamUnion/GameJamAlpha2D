using UnityEngine;

public class BaseUnit : MonoBehaviour
{
    // �ŗLID
    [SerializeField] private int _originID = 0;
    // ���O(�\����)
    [SerializeField] private string _name = "NoName";

    // �e�Z�N�V�����̐��Y����(%)
    [SerializeField] private float _productionEfficiency1 = 1f;
    [SerializeField] private float _productionEfficiency2 = 1f;
    [SerializeField] private float _productionEfficiency3 = 1f;

    // �R���w�� -1�`1
    [SerializeField] private float _liarIndex = 0f;

    [SerializeField] private float _obstaclePower = 0f;

    // �̗� -1�`1
    [SerializeField] private float _endurance = 0f;

    // ���x -1�`1
    [SerializeField] private float _movementSpeed = 0f;

    // ���� -1�`1
    [SerializeField] private float _hearingPower = 0f;

    // �z��
    [SerializeField] private RI.PlacementState _placementState = RI.PlacementState.Section1;

    [SerializeField] private BaseResume _baseResume;
    [SerializeField] private ResumeData _resumeData;
    [SerializeField] private ResumeInterface _resumeInterface;
    [SerializeField] private IntervieweeUnitAbility _intervieweeUnitAbility;

    [SerializeField] private bool _liar = false;

    #region �v���p�e�B
    public int Origin
    {
        get { return _originID; }
    }
    public string Name
    {
        get { return _name; }
    }
    public float ProductionEfficiency1
    {
        get { return _productionEfficiency1; }
    }
    public float ProductionEfficiency2
    {
        get { return _productionEfficiency2; }
    }
    public float ProductionEfficiency3
    {
        get { return _productionEfficiency3; }
    }
    public float LiarIndex
    {
        get { return _liarIndex; }
    }
    public float ObstaclePower
    {
        get { return _obstaclePower; }
    }
    public float Endurance
    {
        get { return _endurance; }
    }
    public float MovementSpeed
    {
        get { return _movementSpeed; }
    }
    public float HearingPower
    {
        get { return _hearingPower; }
    }
    public ResumeInterface ResumeInterface
    {
        get { return _resumeInterface; }
        set { _resumeInterface = value; }
    }
    public RI.PlacementState PlacementState
    {
        get { return _placementState; }
    }
    public BaseResume MyResume
    {
        get { return _baseResume; }
    }
    public IntervieweeUnitAbility IntervieweeUnitAbility
    {
        get { return _intervieweeUnitAbility; }
        set { _intervieweeUnitAbility = value; }
    }
    public bool Liar
    {
        get { return _liar; }
    }
    #endregion

    void Start()
    {

    }

    void Update()
    {

    }

    #region
    public BaseUnit()
    {

    }
    public BaseUnit(BaseUnit baseUnit)
    {
        _name = name;

        _productionEfficiency1 = baseUnit.ProductionEfficiency1;
        _productionEfficiency2 = baseUnit.ProductionEfficiency2;
        _productionEfficiency3 = baseUnit.ProductionEfficiency3;

        _liarIndex = baseUnit.LiarIndex;
        _obstaclePower = baseUnit.ObstaclePower;
        _endurance = baseUnit._endurance;
        _movementSpeed = baseUnit._movementSpeed;
        _hearingPower = baseUnit._hearingPower;
    }
    #endregion

    #region SetData
    // ���j�b�g�̏���������
    public void Initialize(int originID, OfficeGameMaster officeGameMaster)
    {
        _originID = originID;
        _baseResume.SetResume(this);
        _baseResume.SetOfficeGameMaster(officeGameMaster);
        _resumeInterface.Initialize(_baseResume);
    }
    // �X�e�[�^�X�ݒ�
    public void SetState(string name, float productionEfficiency1, float productionEfficiency2, float productionEfficiency3, float liarIndex, float obstaclePower, float endurance, float movementSpeed, float hearingPower)
    {
        _name = name;
        _productionEfficiency1 = productionEfficiency1;
        _productionEfficiency2 = productionEfficiency2;
        _productionEfficiency3 = productionEfficiency3;
        _liarIndex = liarIndex;
        _obstaclePower = obstaclePower;
        _endurance = endurance;
        _movementSpeed = movementSpeed;
        _hearingPower = hearingPower;

        // _liarIndex���Q�Ƃ��� ���U�̒l������(����̂�)
        // �R�����Ă��邩�ǂ����m�F
        if(_liarIndex > 0)// 0�ȉ��̏ꍇ�R�͕K�����Ȃ�
        {
            // �R�����Ă���ꍇ�������Ⴉ�߂����Ⴉ�Ȓl������悤�ɕύX
            if (_liarIndex >= 0.8f)
            {
                // 100%
                _liar = true;
            }
            else if (_liarIndex >= 0.6f)
            {
                if (Random.Range(0f, 1f) >= 0.5f)// 50%
                {
                    _liar = true;
                }
            }
            else if (_liarIndex >= 0.4f)
            {
                if (Random.Range(0f, 1f) >= 0.8f)// 20%
                {
                    _liar = true;
                }
            }
            else if (_liarIndex >= 0.2)
            {
                if (Random.Range(0f, 1f) >= 0.95f)// 5%
                {
                    _liar = true;
                }
            }
        }
    }
    public void SetState(BaseUnit baseUnit)
    {
        _name = baseUnit.Name;

        _productionEfficiency1 = baseUnit.ProductionEfficiency1;
        _productionEfficiency2 = baseUnit.ProductionEfficiency2;
        _productionEfficiency3 = baseUnit.ProductionEfficiency3;

        _liarIndex = baseUnit.LiarIndex;
        _obstaclePower = baseUnit.ObstaclePower;
        _endurance = baseUnit.Endurance;
        _movementSpeed = baseUnit.MovementSpeed;
        _hearingPower = baseUnit.HearingPower;

        _liar = baseUnit.Liar;
    }
    #endregion SetData

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

        if (!_liar)
        {
            // �R������Ȃ�
            if (_resumeData.RankS.MinProductionEfficiency <= productionEfficiencyNum)
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
        else
        {
            switch (Random.Range(0, 7))
            {
                case 0:
                    return _resumeData.RankS.ScoreStr;

                case 1:
                    return _resumeData.RankA.ScoreStr;

                case 2:
                    return _resumeData.RankB.ScoreStr;

                case 3:
                    return _resumeData.RankC.ScoreStr;

                case 4:
                    return _resumeData.RankD.ScoreStr;

                case 5:
                    return _resumeData.RankE.ScoreStr;

                case 6:
                    return _resumeData.RankF.ScoreStr;

                default:
                    Debug.Log("NONE RANGE");
                    return _resumeData.RankF.ScoreStr;
            }
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
