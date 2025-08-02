using UnityEngine;

public class SmokeParticle : MonoBehaviour
{
    #region�@Field
    private ParticleSystem _ParticleSystem = null;
    #endregion

    #region Method
    void Start()
    {
        _ParticleSystem = gameObject.GetComponentInChildren<ParticleSystem>();
    }

    private void Update()
    {
        if (_ParticleSystem == null)
            return;

        // ��~������Q�[���I�u�W�F�N�g���\���ɂ���
        if(_ParticleSystem.isStopped && gameObject.activeSelf)
            gameObject.SetActive(false);
    }


    /// <summary>
    /// �p�[�e�B�N���Đ�
    /// </summary>
    public void requestStartParticle()
    {
        if (_ParticleSystem == null)
            return;

        gameObject.SetActive(true);
        _ParticleSystem.Play();
    }

    /// <summary>
    /// �p�[�e�B�N����~
    /// </summary>
    public void requestStopParticle()
    {
        if (_ParticleSystem == null)
            return;

        _ParticleSystem.Stop();
    }
    #endregion

    #region Debug
#if UNITY_EDITOR
    [ContextMenu("StartParticle")]
    private void devStartParticle()
    {
        requestStartParticle();
    }

    [ContextMenu("StopParticle")]
    private void devStopParticle()
    {
        requestStopParticle();
    }
#endif
    #endregion
}
