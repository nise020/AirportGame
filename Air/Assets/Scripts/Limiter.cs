using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limiter : MonoBehaviour
{
    //rect bound
    [Header("<color=red>화면</color>경계")]//viewport 기준
    [SerializeField] Vector2 viewPortLimitMin;
    [Space]//살짝 공간을 띄우는 기능
    [SerializeField] Vector2 viewPortLimitMax;

    [Header("보스용 화면 경계")]
    [SerializeField] Vector2 viewPortLimitMinBoss;
    [SerializeField] Vector2 viewPortLimitMaxBoss;

    /// <summary>
    /// x는 x Min y는 x Max
    /// </summary>
    Vector2 worldPosLimitMin;//실제 데이터는 이 변수가 가지고 있으ㅁ
    public Vector2 WorldPosLimitMin //데이터는 변수로 보이지만 함수로 작동
    { 
        get 
        {  
            return worldPosLimitMin;
        } 
    }

    Vector2 worldPosLimitMax;
    public Vector2 WorldPosLimitMax=> worldPosLimitMax;
    
    Camera cam;
    GameManager gameManager;
    private void Start()
    {
        cam = Camera.main;
        gameManager = GameManager.Instance;
        gameManager._Limiter = this;

        inutWorldPos();
    }
    /// <summary>
    /// 게임 시작시 뷰포인트의 화면경계 변수들을 월드 포지션으로 초기화 합니다
    /// </summary>
    private void inutWorldPos() 
    {
        worldPosLimitMin = cam.ViewportToWorldPoint(viewPortLimitMin);
        worldPosLimitMax = cam.ViewportToWorldPoint(viewPortLimitMax);

        
       //viewPortLimitMin;
       // viewPortLimitMax;
        #region 다른 예시
        //worldPosLimitMin.x = cam.ViewportToWorldPoint(new Vector3(viewPortLimitMin.x, 0)).x;
        //worldPosLimitMax.y = cam.ViewportToWorldPoint(new Vector3(viewPortLimitMax.x, 0)).x;
        //Debug.Log(worldPosLimit);
        #endregion
       // WorldPosLimitMin;
       // worldPosLimitMax;

    }
    #region 설명
    /// <summary>
    /// 코드에 의해서 플레이어가 카메라 밖에 못나가게 하는 기능
    /// </summary>
    #endregion
    public Vector3 checkMovePosition(Vector3 _pos, bool _isBoss = false)//선택적 매계변수는 뒤에 적어둬야 한다
    {
        Vector3 viewPortPos = cam.WorldToViewportPoint(_pos);

        //삼중방식=조건연산자=삼항연산자=다항식

        if (viewPortPos.x < (_isBoss==false ? viewPortLimitMin.x : viewPortLimitMinBoss.x))//0~1
        {
            viewPortPos.x = (_isBoss == false ? viewPortLimitMin.x : viewPortLimitMinBoss.x);
        }
        else if (viewPortPos.x > (_isBoss == false ? viewPortLimitMax.x : viewPortLimitMaxBoss.x))
        {
            viewPortPos.x = (_isBoss == false ? viewPortLimitMax.x : viewPortLimitMaxBoss.x);
        }

        if (viewPortPos.y < viewPortLimitMin.y)
        {
            viewPortPos.y = viewPortLimitMin.y;
        }
        else if (viewPortPos.y > viewPortLimitMax.y)
        {
            viewPortPos.y = viewPortLimitMax.y;
        }

        //Vector3 fixedPos
        return cam.ViewportToWorldPoint(viewPortPos);
        //transform.position = fixedPos;
    }
    public bool checkMovePosition(Vector3 _pos)//선택적 매계변수는 뒤에 적어둬야 한다
    {
        Vector3 viewPortPos = cam.WorldToViewportPoint(_pos);

        //삼중방식=조건연산자=삼항연산자=다항식

        if (viewPortPos.x < viewPortLimitMinBoss.x || viewPortPos.x > viewPortLimitMaxBoss.x)//0~1
        {
            return true;
        }
        return false;
    }
        
    public (bool _x, bool _y) IsReflecItem(Vector3 _pos, Vector3 _dir) //화면경계에 닿았거나 화면밖으로 나갔다면 반사해야한다고 알려줌
    {

        bool rX = false;
        bool rY = false;
        if ((_pos.x < worldPosLimitMin.x && _dir.x < 0) 
            || (_pos.x > worldPosLimitMax.x&& _dir.x > 0))
        {
            rX = true;
        }
        if ((_pos.y < worldPosLimitMin.y&& _dir.y < 0) 
            ||(_pos.y > worldPosLimitMax.y&& _dir.y > 0))
        {
            rY = true;
        }
        return (rX,rY);
    }
}
