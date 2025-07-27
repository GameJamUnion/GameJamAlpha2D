using System;
using UnityEngine;

public class SoundManager : SingletonBase<SoundManager>
{

	#region Field
	private Action<BGMKind> _PlayBGM;
	private Action<SEKind> _PlaySE;

	private Action<BGMKind> _StopBGM;
	private Action<SEKind> _StopSE;
	#endregion Field
	#region Method
	public void registerSoundTable(Action<BGMKind> playBGM, Action<BGMKind> stopBGM, Action<SEKind> playSE, Action<SEKind> stopSE)
	{
		_PlayBGM = playBGM;
		_PlaySE = playSE;

		_StopBGM = stopBGM;
		_StopSE = stopSE;
	}

	#region Request
	/// <summary>
	/// BGM再生リクエスト
	/// </summary>
	/// <param name="kind"></param>
	public void requestPlaySound(BGMKind kind)
	{
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
