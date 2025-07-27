using System;
using UnityEngine;

public class SoundManager : SingletonBase<SoundManager>
{

	#region Field
	private Action<BGMKind> _PlayBGM;
	private Action<SEKind> _PlaySE;

	private Action<BGMKind> _StopBGM;
	private Action<SEKind> _StopSE;

	private BGMKind? _ReservePlayBGM = null;
	#endregion Field
	#region Method
	public void registerSoundTable(Action<BGMKind> playBGM, Action<BGMKind> stopBGM, Action<SEKind> playSE, Action<SEKind> stopSE)
	{
		_PlayBGM = playBGM;
		_PlaySE = playSE;

		_StopBGM = stopBGM;
		_StopSE = stopSE;
	}


    public override void LateUpdate()
    {
        base.LateUpdate();

		if (_ReservePlayBGM != null)
		{
			requestPlaySound(_ReservePlayBGM.Value);
			_ReservePlayBGM = null;
		}

    }
	#region Request
	/// <summary>
	/// BGM再生リクエスト
	/// </summary>
	/// <param name="kind"></param>
	public void requestPlaySound(BGMKind kind)
	{
		if (_PlayBGM == null)
		{
            // _PlayBGMが登録される前にリクエストされたら
            // 予約に入れる
            _ReservePlayBGM = kind;
			return;
		}

		_PlayBGM?.Invoke(kind);
	}

	/// <summary>
	/// BGM停止リクエスト
	/// </summary>
	/// <param name="kind"></param>
	public void requestStopSound(BGMKind kind)
	{
		_StopBGM?.Invoke(kind);
	}

	/// <summary>
	/// SE再生リクエスト
	/// </summary>
	/// <param name="kind"></param>
	public void requestPlaySound(SEKind kind)
	{
		if (_PlaySE == null)
		{
			// _PlaySEが登録される前にリクエストされたら
			// なにもしない
			return;
		}

		_PlaySE?.Invoke(kind);
	}

	/// <summary>
	/// SE停止リクエスト
	/// </summary>
	/// <param name="kind"></param>
	public void requestStopSound(SEKind kind)
	{
		_StopSE?.Invoke(kind);
	}
	#endregion Requst
	#endregion Method
}
