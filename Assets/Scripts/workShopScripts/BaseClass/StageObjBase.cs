using System.Collections.Generic;
using UnityEngine;

public abstract class StageObjBase : ObjBase
{
    /// <summary>
    /// ��ƕ����X�g
    /// </summary>
    protected List<Product> productList;

    /// <summary>
    /// ��ƕ��o�͐�I�u�W�F�N�g
    /// </summary>
    [SerializeField]
    protected StageObjBase outputObj;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    override protected void Start()
    {
        base.Start();
        productList = new List<Product>();
    }

    /// <summary>
    /// ��ƕ���ǉ�����
    /// </summary>
    /// <param name="product"></param>
    public abstract void addProduct(Product product);
}
