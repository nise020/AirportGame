using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public void doDestroy()
    {
        Destroy(gameObject);
    }

    #region �̸��� �ٸ��� �ؾ� �Լ��� ���δ�
    //������ �ڷ����� �Ű������� Ȯ���� �����ϴ�
    //public void doDestroy2(string value)
    //{
    //    Destroy(gameObject);
    //}
    //public void doDestroy3(float value)
    //{
    //    Destroy(gameObject);
    //}
    #endregion

    public void setImageSize(float _imgSize)
    {
        Vector3 scale = transform.localScale;
        scale *= _imgSize / 24f;
        transform.localScale = scale;
    }
}
