using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using static FadeManager;

public class FadeInOut : MonoBehaviour
{
    #region Const
    /// <summary>
    /// �ŏ��l(����)
    /// </summary>
    private const float Alfa_Min = 0.0f;

    /// <summary>
    /// �ő�l(�s����)
    /// </summary>
    private const float Alfa_Max = 1.0f;
    #endregion

    #region Definition
    private enum StepType
    {
        None,
        Start,
        Action,
        End,
    }
    #endregion

    #region Field
    [SerializeField]
    private GameObject _PanelObject = null;

    [SerializeField]
    private float _FadeTime = 1.0f;

    private StepType _StepType = StepType.None;
    private FadeType _FadeType = FadeType.None;
    private Image _Image = null;
    private float _Red = 0.0f;
    private float _Green = 0.0f;
    private float _Blue = 0.0f;
    private float _Alfa = 0.0f;
    #endregion

    #region Method

    private void Awake()
    {
        FadeManager.Instance.registerFade(this);
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        if(_PanelObject != null)
        {
            _Image = _PanelObject.GetComponent<Image>();
            if(_Image != null)
            {
                _Red = _Image.color.r;
                _Green = _Image.color.g;
                _Blue = _Image.color.b;
                _Alfa = _Image.color.a;
            }

            _PanelObject.SetActive(false);
        }
    }

    // Update is called once per frame
    void Update()
    {
        if (_StepType == StepType.None || _FadeType == FadeType.None)
            return;

        switch (_StepType)
        {
            case StepType.Start:
                {
                    _Alfa = (_FadeType == FadeType.FadeIn) ? Alfa_Max : Alfa_Min;
                    _StepType = StepType.Action;
                }
                break;

            case StepType.Action:
                {
                    // �A���t�@�l�ύX
                    if (_FadeType == FadeType.FadeIn)
                        _Alfa -= Time.deltaTime / _FadeTime;
                    else if (_FadeType == FadeType.FadeOut)
                        _Alfa += Time.deltaTime / _FadeTime;

                    // �A���t�@�l���f
                    setAlfa(_Alfa);

                    // �A���t�@�l���ő� or �ŏ��ɂȂ�����I����
                    if (_Alfa >= Alfa_Max || _Alfa <= Alfa_Min)
                        _StepType = StepType.End;
                }
                break;

            case StepType.End:
                {
                    // ���S�ɓ��� or �s�����ɂ���
                    _Alfa = (_FadeType == FadeType.FadeIn) ? Alfa_Min : Alfa_Max;
                    setAlfa(_Alfa);

                    _StepType = StepType.None;
                    _FadeType = FadeType.None;

                    if (_PanelObject != null && _Alfa == Alfa_Min)
                        _PanelObject.SetActive(false);
                }
                break;
        }
    }

    /// <summary>
    /// �A���t�@�l�𔽉f����
    /// </summary>
    /// <param name="alfa"></param>
    private void setAlfa(float alfa)
    {
        if (_Image == null)
            return;

        _Image.color = new Color(_Red, _Green, _Blue, alfa);
    }

    /// <summary>
    /// �t�F�[�h�J�n���N�G�X�g
    /// </summary>
    /// <param name="fadeType"></param>
    public void requestStartFade(FadeType fadeType)
    {
        _PanelObject.SetActive(true);
        _FadeType = fadeType;
        _StepType = StepType.Start;
    }

    #endregion

    #region Debug
    [ContextMenu("FadeIn")]
    public void devFadeIn()
    {
        requestStartFade(FadeType.FadeIn);
    }

    [ContextMenu("FadeOut")]
    public void devFadeOut()
    {
        requestStartFade(FadeType.FadeOut);
    }
    #endregion

}
