using UnityEngine;
using System.Collections.Generic;

public enum PauseType
{
    Invalid,

    InGamePause,

}

public class PauseManager : SingletonBase<PauseManager>
{
    public class PauseRequestArgs
    {
        public GameObject Owner;
        public PauseType Type;
    }

    private PauseFunction_InGamePause _InGamePauseFunc = new PauseFunction_InGamePause();

    private List<PauseRequestArgs> _PauseRequest = new List<PauseRequestArgs>(4);

    public override void LateUpdate()
    {
        base.LateUpdate();

        updatePause();
    }

    /// <summary>
    /// �|�[�Y�����X�V
    /// </summary>
    private void updatePause()
    {
        var inGamePause = false;
        lock (_PauseRequest)
        {
            var pauseRequestCount = _PauseRequest.Count;
            if (_PauseRequest.Count != 0)
            {
                for (int i = 0; i < pauseRequestCount; i++)
                {
                    var request = _PauseRequest[i];
                    switch (request.Type)
                    {
                        case PauseType.InGamePause:
                            if (_InGamePauseFunc.Active == false)
                            {
                                _InGamePauseFunc.onStartPause();
                            }
                            inGamePause = true;
                            break;
                        default:
                            Debug.LogError($"Unknown PauseType: {request.Type}");
                            break;
                    }
                }
            }
        }

        if (inGamePause == false)
        {
            _InGamePauseFunc.onEndPause();
        }
    }

    /// <summary>
    /// �|�[�Y���������Ă��邩
    /// </summary>
    /// <returns></returns>
    public bool checkPauseAny()
    {
        lock (_PauseRequest)
        {
            return _PauseRequest.Count > 0;
        }
    }
    #region Request
    /// <summary>
    /// �|�[�Y�J�n���N�G�X�g
    /// </summary>
    /// <param name="args"></param>
    public void requestStartPause(PauseRequestArgs args)
    {
        lock (_PauseRequest)
        {
            _PauseRequest.Add(args);
        }
    }

    /// <summary>
    /// �|�[�Y�I�����N�G�X�g
    /// </summary>
    /// <param name="owner"></param>
    public void requestEndPause(GameObject owner)
    {
        lock(_PauseRequest)
        {
            _PauseRequest.RemoveAll(request => request.Owner == owner);
        }
    }
    #endregion Request
}

#region Pause����
public abstract class PauseFunctionBase
{
    public bool Active { get; protected set; } = false;

    /// <summary>
    /// �|�[�Y�J�n����
    /// </summary>
    public virtual void onStartPause()
    {
        Active = true;
    }
    /// <summary>
    /// �|�[�Y�I������
    /// </summary>
    public virtual void onEndPause()
    {
        Active = false;
    }
}

public class PauseFunction_InGamePause : PauseFunctionBase
{
    public override void onStartPause()
    {
        base.onStartPause();

        Time.timeScale = 0f; // �Q�[���̎��Ԃ��~
    }

    public override void onEndPause()
    {
        base.onEndPause();

        Time.timeScale = 1f; // �Q�[���̎��Ԃ��ĊJ
    }
}
#endregion