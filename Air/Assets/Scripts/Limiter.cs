using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Limiter : MonoBehaviour
{
    //rect bound
    [Header("<color=red>ȭ��</color>���")]//viewport ����
    [SerializeField] Vector2 viewPortLimitMin;
    [Space]//��¦ ������ ���� ���
    [SerializeField] Vector2 viewPortLimitMax;

    [Header("������ ȭ�� ���")]
    [SerializeField] Vector2 viewPortLimitMinBoss;
    [SerializeField] Vector2 viewPortLimitMaxBoss;

    /// <summary>
    /// x�� x Min y�� x Max
    /// </summary>
    Vector2 worldPosLimitMin;//���� �����ʹ� �� ������ ������ ������
    public Vector2 WorldPosLimitMin //�����ʹ� ������ �������� �Լ��� �۵�
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
    /// ���� ���۽� ������Ʈ�� ȭ���� �������� ���� ���������� �ʱ�ȭ �մϴ�
    /// </summary>
    private void inutWorldPos() 
    {
        worldPosLimitMin = cam.ViewportToWorldPoint(viewPortLimitMin);
        worldPosLimitMax = cam.ViewportToWorldPoint(viewPortLimitMax);

        
       //viewPortLimitMin;
       // viewPortLimitMax;
        #region �ٸ� ����
        //worldPosLimitMin.x = cam.ViewportToWorldPoint(new Vector3(viewPortLimitMin.x, 0)).x;
        //worldPosLimitMax.y = cam.ViewportToWorldPoint(new Vector3(viewPortLimitMax.x, 0)).x;
        //Debug.Log(worldPosLimit);
        #endregion
       // WorldPosLimitMin;
       // worldPosLimitMax;

    }
    #region ����
    /// <summary>
    /// �ڵ忡 ���ؼ� �÷��̾ ī�޶� �ۿ� �������� �ϴ� ���
    /// </summary>
    #endregion
    public Vector3 checkMovePosition(Vector3 _pos, bool _isBoss = false)//������ �Ű躯���� �ڿ� ����־� �Ѵ�
    {
        Vector3 viewPortPos = cam.WorldToViewportPoint(_pos);

        //���߹��=���ǿ�����=���׿�����=���׽�

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
    public bool checkMovePosition(Vector3 _pos)//������ �Ű躯���� �ڿ� ����־� �Ѵ�
    {
        Vector3 viewPortPos = cam.WorldToViewportPoint(_pos);

        //���߹��=���ǿ�����=���׿�����=���׽�

        if (viewPortPos.x < viewPortLimitMinBoss.x || viewPortPos.x > viewPortLimitMaxBoss.x)//0~1
        {
            return true;
        }
        return false;
    }
        
    public (bool _x, bool _y) IsReflecItem(Vector3 _pos, Vector3 _dir) //ȭ���迡 ��Ұų� ȭ������� �����ٸ� �ݻ��ؾ��Ѵٰ� �˷���
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
