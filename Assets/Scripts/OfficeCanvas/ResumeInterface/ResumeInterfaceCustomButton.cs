using UnityEngine;

public class ResumeInterfaceCustomButton : BaseCustomButton
{
    [Header("アタッチ")]
    [SerializeField] ResumeInterface _resumeInterface;

    [Header("長押し")]
    [SerializeField] bool _holdTap = false;
    [SerializeField] Vector3 _currentMousePos = new Vector3();
    [SerializeField] Vector3 _saveRusumeInterfacePos = new Vector3();

    [Header("ダブルタップ")]
    [SerializeField] float _doubleTapTimer = 0f;
    [SerializeField] float _maxDoubleTapTime = 0.2f;
    [SerializeField] bool _doubleTap = false;

    #region プロパティ
    public bool HoldTap
    {
        get { return _holdTap; }
    }
    public Vector3 CurrentMousePos
    {
        get { return _currentMousePos; }
    }
    public Vector3 SaveRusumeInterfacePos
    {
        get { return _saveRusumeInterfacePos; }
    }
    #endregion

    protected override void Start()
    {
        _doubleTapTimer = 100000f;
        base.Start();
    }
    private void Update()
    {
        if (_doubleTapTimer < _maxDoubleTapTime)
            _doubleTapTimer += Time.deltaTime;
    }
    public override void PointEnter()
    {
        base.PointEnter();
    }
    public override void PointExit()
    {
        if (_doubleTap)
        {
            _doubleTap = false;
        }
        base.PointExit();
    }
    public override void PointDown()
    {
        _holdTap = true;
        _currentMousePos = Input.mousePosition;
        _saveRusumeInterfacePos = _resumeInterface.GetComponent<RectTransform>().localPosition;
        _doubleTapTimer = 0f;
        if (_doubleTapTimer <= _maxDoubleTapTime)
        {
            _doubleTap = true;
            _resumeInterface.Close();
        }
        base.PointDown();
    }
    public void PointUp()
    {
        _holdTap = false;
    }
}
