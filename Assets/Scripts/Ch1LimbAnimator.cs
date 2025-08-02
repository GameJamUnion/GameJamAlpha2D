using Unity.VisualScripting;
using UnityEngine;

/// <summary>
/// キャラクターの四肢を動かすクラス
/// </summary>
public class Ch1LimbAnimator
{
    public Ch1LimbAnimator(Ch1LimbController controller)
    {
        _Controller = controller;
    }

    #region Definition
    public enum AnimationTypes
    {
        /// <summary>
        /// 通常待機
        /// </summary>
        Waiting,

        /// <summary>
        /// 休憩中
        /// </summary>
        Rest,
    }
    #endregion
    #region Property
    public AnimationTypes CurrentType
    {
        get => _CurrentPhase;
        set => _CurrentPhase = value;
    }      
    private AnimationTypes _CurrentPhase = AnimationTypes.Waiting;
    #endregion Property
    #region Field
    private GameObject _TargetObj = null;
    private Ch1LimbAnimationScriptableObject _Data = null;
    private Ch1LimbController _Controller = null;
    private Ch1CharacterBehavior _CharacterBehavior = null;

    // 基本の位置
    private Vector3 _BasePosition = Vector3.zero;
    private Quaternion _BaseRotation = Quaternion.identity;

    // 前回のループからの経過時間
    private float _ElapsedTime = 0f;
    #endregion Field
    #region Method
    /// <summary>
    /// 初期化
    /// </summary>
    /// <param name="target"></param>
    public void setup(Ch1LimbController.LimbAnimatorParam targetParam)
    {
        _TargetObj = targetParam.TargetObj;
        if (_TargetObj == null)
        {
            return;
        }

        _BasePosition = _TargetObj.transform.localPosition;
        _BaseRotation = _TargetObj.transform.localRotation;
        _Data = targetParam.ScriptableObject;
    }

    public void lateUpdate()
    {
        if (_TargetObj == null || _Data == null)
        {
            return;
        }

        var time = Time.deltaTime;

        _ElapsedTime += time;
        switch (CurrentType)
        {
            case AnimationTypes.Waiting:
                updateWaiting();
                break;
            case AnimationTypes.Rest:
                updateRest();
                break;
            default:
                break;
        }
    }

    private void changePhase(AnimationTypes type)
    {
        if (type == CurrentType)
        {
            return;
        }

        reset();

        CurrentType = type;
    }

    /// <summary>
    /// 初期状態に戻す
    /// </summary>
    private void reset()
    {
        setLocalPosition(_BasePosition);
        setLocalRotation(_BaseRotation);
        _ElapsedTime = 0f;
    }

    /// <summary>
    /// 外部からAnimationTypeを変更する
    /// </summary>
    /// <param name="type"></param>
    public void setAnimationType(AnimationTypes type)
    {
        changePhase(type);
    }
    #region update
    /// <summary>
    /// 通常待機更新
    /// </summary>
    private void updateWaiting()
    {
        var data = _Data.Waiting;

        // 縦移動
        if (data.VerticalMove.Enable == true)
        {
            moveVertical(data.VerticalMove, data.LoopTime);
        }



        if (_ElapsedTime > data.LoopTime)
        {
            _ElapsedTime = 0f;
        }
    }

    /// <summary>
    /// 休憩中更新
    /// </summary>
    private void updateRest()
    {
        var data = _Data.Rest;

        if (data.VerticalMove.Enable == true)
        {
            moveVertical(data.VerticalMove, data.LoopTime);
        }

        if (data.RotationMove.Enable == true)
        {
            moveRotation(data.RotationMove, data.LoopTime);
        }


        if (_ElapsedTime > data.LoopTime)
        {
            _ElapsedTime = 0f;
        }
    }
    #endregion update
    #region Move
    /// <summary>
    /// 縦方向の移動
    /// </summary>
    /// <param name="param"></param>
    /// <param name="loopTime"></param>
    private void moveVertical(Ch1LimbAnimationScriptableObject.VerticalMoveParam param, float loopTime)
    {
        var timeRate = _ElapsedTime / loopTime;
        var interpolateValue = param.MoveCurve.Evaluate(timeRate);
        var addPosY = interpolateValue * param.MoveValue;

        // 移動
        setLocalPosition(_BasePosition + new Vector3(0f, addPosY, 0f));
    }

    /// <summary>
    /// 回転
    /// </summary>
    /// <param name="param"></param>
    /// <param name="loopTime"></param>
    private void moveRotation(Ch1LimbAnimationScriptableObject.RotationMoveParam param, float loopTime)
    {
        var timeRate = _ElapsedTime / loopTime;
        var interpolateValue = param.MoveCurve.Evaluate(timeRate);
        var rot = interpolateValue * param.MoveValue;

        // 回転
        var rad = rot;
        setLocalRotation(rad);
    }
    #endregion Move


    /// <summary>
    /// 対象のLocalPositionを変更する
    /// 実行履歴を追うためにラッピング
    /// </summary>
    /// <param name="localPos"></param>
    private void setLocalPosition(Vector3 localPos)
    {
        _TargetObj.transform.localPosition = localPos;
    }

    /// <summary>
    /// 対象のLocalRotationを変更する
    /// </summary>
    /// <param name="localRot"></param>
    private void setLocalRotation(Vector3 localRot)
    {
        _TargetObj.transform.localRotation = Quaternion.Euler(localRot);
    }
    private void setLocalRotation(Quaternion localRot)
    {
        _TargetObj.transform.localRotation = localRot;
    }
    #endregion
}
