using System.Collections;
using UnityEngine;


// 便利関数入れておくやつ

 
/// <summary>
/// MonoBehavior非継承クラスからStartCoroutineを利用するためのクラス
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

