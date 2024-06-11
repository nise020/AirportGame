using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// �÷��̾��� HP�� ������ ����� HP�������� ��� �����ϰ� Effect�� �ش� �������� �ʴ� �����ϰ� ����� �ݴϴ�
/// </summary>
public class FunctionHP : MonoBehaviour
{
    //ȸ�翡���� ���� ���α׷��� ��� ���Ѵ�
    //�����Ҷ� ������ �߻��ؼ�..
    //��Ʈ��+.

    [SerializeField] Image imgHp;
    [SerializeField] Image imgEffect;

    [SerializeField, Range(0.1f, 10f)] float effectTime = 1;//0�� ������ ������ ����
    GameManager gameManager;

    bool isEnded=false;
    //Transform trsPosPlayer;

    private void Awake()
    {
        initHp();
    }
    private void Start()
    {
        gameManager = GameManager.Instance;
        //trsPosPlayer = GameManager.Instance._Player.transform;
    }
    private void initHp()
    {
        imgHp.fillAmount = 1;
        imgEffect.fillAmount = 1;
        //fillAmount �������� 1�� �����Ѵ�
    }
    void Update()
    {
        checkFillAmount();
        chasePlayer();
        chekedPlayerDestroy();
    }
    private void checkFillAmount()
    {
        if (imgHp.fillAmount == imgEffect.fillAmount)
        {
            return;
        }
        if (imgHp.fillAmount < imgEffect.fillAmount)
        {
            // (Time.deltaTime * 0.5f) �ӵ� ����
            imgEffect.fillAmount -= (Time.deltaTime / effectTime);
            if (imgHp.fillAmount > imgEffect.fillAmount)
            {
                imgEffect.fillAmount = imgHp.fillAmount;
            }
        }
        else if (imgHp.fillAmount > imgEffect.fillAmount)
        {
            imgEffect.fillAmount = imgHp.fillAmount;
        }
    }
    private void chasePlayer()
    {
        if (gameManager.GetPlayerPosition(out Vector3 pos) == true)
        {
            pos.y -= 0.7f;
            transform.position = pos;// = pos - new Vector3(0,0.7f,0);
        }
        //imgEffect.enabled = true; //���� �ִ�
    }
    public void SetHp(float _maxHp, float _curHp)//0~1
    {
        imgHp.fillAmount = _curHp / _maxHp;
    }

    private void chekedPlayerDestroy()
    {
        //isEnded = true;
        //if (isEnded = false && gameManager._Player == null)
        //{
        //    isEnded = true;
        //    Destroy();
        //}
        if (imgEffect.fillAmount == 0.1f) 
        {
            Destroy(gameObject);       
        }
    }
}
