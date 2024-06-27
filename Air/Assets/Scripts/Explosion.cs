using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Explosion : MonoBehaviour
{
    public void doDestroy()
    {
        Destroy(gameObject);
    }

    #region 이름을 다르게 해야 함수가 보인다
    //간단한 자료형의 매개변수만 확인이 가능하다
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
        //이미지 사이즈에 비례해서 해당 사이즈 조절
        Vector3 scale = transform.localScale;
        scale *= _imgSize / 24f;
        transform.localScale = scale;
    }
}
