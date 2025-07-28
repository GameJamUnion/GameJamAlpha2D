using System.Collections;
using UnityEngine;


// �֗��֐�����Ă������

 
/// <summary>
/// MonoBehavior��p���N���X����StartCoroutine�𗘗p���邽�߂̃N���X
/// </summary>
public class CoroutineHandler : MonoBehaviour
{
    static protected CoroutineHandler _Instance;
    static public CoroutineHandler Instance
    {
        get
        {
            if (_Instance == null)
            {
                var obj = new GameObject("CoroutineHandler");
                DontDestroyOnLoad(obj);
                _Instance = obj.AddComponent<CoroutineHandler>();
            }
            return _Instance;
        }
    }

    public void OnDisable()
    {
        if (_Instance != null)
        {
            Destroy(_Instance.gameObject);
        }
    }

    static public Coroutine startCoroutine(IEnumerator coroutine)
    {
        return Instance.StartCoroutine(coroutine);
    }
}

