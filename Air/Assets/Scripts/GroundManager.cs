using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GroundManager : MonoBehaviour
{//배경이 반복해서 흘러 내리기 기능
    Material matTop; //변수 찾기
    Material matMid;
    Material matBot;

    [SerializeField] private float speedTop;//private로 선언했지만 인스팩터에서 볼수 있게 된다
    [SerializeField] private float speedMid;//inspector-인스팩터
    [SerializeField] private float speedBot;//디자이너,기획자가 직접 수정하기 위해 항목 만들기
                                            //카멜표기법,m_fSpeedBot => F Speed Bot(표기됨),_는 표시 안됨
                                            //[SerializeField] private GameObject objBackGrounds;
    GameObject objBackGround;
    //null,이렇게 사용하면 느려질수 있다

    //gameObject
    //transform
    // 데이터를 찾을때 위치로 찾는다

    void Start()
    {
        objBackGround = transform.Find("BackGround").gameObject;
        //objBackGround = GameObject.Find("BackGround");//코드로 찾음
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
    //오버플로우:수치 값이 Max치를 뚫어 버리면 0이 아닌 최솟값으로 이동한다
    void Update()//사양이 좋은 컴퓨터 1초에 700번, 사양이 안좋은 컴퓨터 1초에 30번
    {
        Vector2 vecTop = matTop.mainTextureOffset;//Offset 수치 변경
        Vector2 vecMid = matMid.mainTextureOffset;//Value 값이라서 한번 꺼내서  
        Vector2 vecBot = matBot.mainTextureOffset;//복제 해줘야 한다    

        vecTop += new Vector2(0, speedTop * Time.deltaTime);
        vecMid += new Vector2(0, speedMid * Time.deltaTime);
        vecBot += new Vector2(0, speedBot * Time.deltaTime);
        //deltaTime:1초가 지났을때 정확이 프레임당 같은값 쌓이는 기능

        //if (vecTop.y > 1.0f)
        //{//안전성을 위한 예외처리 
        //    vecTop.y = 0.0f;
        //}

        vecTop.y = Mathf.Repeat(vecTop.y, 1.0f);
        vecMid.y = Mathf.Repeat(vecMid.y, 1.0f);
        vecBot.y = Mathf.Repeat(vecBot.y, 1.0f);
        //Mathf 수학적 기능
        //Repeat= 0보다 작아질수 없다

        matTop.mainTextureOffset = vecTop;
        matMid.mainTextureOffset = vecMid;
        matBot.mainTextureOffset = vecBot;

    }
}


        
        
        