using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{//����� �ݺ��ؼ� �귯 ������ ���
    Material matTop; //���� ã��
    Material matMid;
    Material matBot;

    [SerializeField] private float speedTop;//private�� ���������� �ν����Ϳ��� ���� �ְ� �ȴ�
    [SerializeField] private float speedMid;//inspector-�ν�����
    [SerializeField] private float speedBot;//�����̳�,��ȹ�ڰ� ���� �����ϱ� ���� �׸� �����
                                            //ī��ǥ���,m_fSpeedBot => F Speed Bot(ǥ���),_�� ǥ�� �ȵ�
                                            //[SerializeField] private GameObject objBackGrounds;
    GameObject objBackGround;
    //null,�̷��� ����ϸ� �������� �ִ�

    //gameObject
    //transform
    // �����͸� ã���� ��ġ�� ã�´�

    void Start()
    {
        objBackGround = transform.Find("BackGround").gameObject;
        //objBackGround = GameObject.Find("BackGround");//�ڵ�� ã��
        Transform trsTop = objBackGround.transform.Find("Top");
        Transform trsMid = objBackGround.transform.Find("Mid");
        Transform trsBot = objBackGround.transform.Find("Bot");

        SpriteRenderer sprTopRenderer = trsTop.GetComponent<SpriteRenderer>();
        SpriteRenderer sprMidRenderer = trsMid.GetComponent<SpriteRenderer>();
        SpriteRenderer sprBotRenderer = trsBot.GetComponent<SpriteRenderer>();

        matTop = sprTopRenderer.material;
        matMid = sprMidRenderer.material;
        matBot = sprBotRenderer.material;
    }
    //�����÷ο�:��ġ ���� Maxġ�� �վ� ������ 0�� �ƴ� �ּڰ����� �̵��Ѵ�
    void Update()//����� ���� ��ǻ�� 1�ʿ� 700��, ����� ������ ��ǻ�� 1�ʿ� 30��
    {
        Vector2 vecTop = matTop.mainTextureOffset;//Offset ��ġ ����
        Vector2 vecMid = matMid.mainTextureOffset;//Value ���̶� �ѹ� ������  
        Vector2 vecBot = matBot.mainTextureOffset;//���� ����� �Ѵ�    

        vecTop += new Vector2(0, speedTop * Time.deltaTime);
        vecMid += new Vector2(0, speedMid * Time.deltaTime);
        vecBot += new Vector2(0, speedBot * Time.deltaTime);
        //deltaTime:1�ʰ� �������� ��Ȯ�� �����Ӵ� ������ ���̴� ���

        //if (vecTop.y > 1.0f)
        //{//�������� ���� ����ó�� 
        //    vecTop.y = 0.0f;
        //}

        vecTop.y = Mathf.Repeat(vecTop.y, 1.0f);
        vecMid.y = Mathf.Repeat(vecMid.y, 1.0f);
        vecBot.y = Mathf.Repeat(vecBot.y, 1.0f);
        //Mathf ������ ���
        //Repeat= 0���� �۾����� ����

        matTop.mainTextureOffset = vecTop;
        matMid.mainTextureOffset = vecMid;
        matBot.mainTextureOffset = vecBot;

    }
}


        
        
        