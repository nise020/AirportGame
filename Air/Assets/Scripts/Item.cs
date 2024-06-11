using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour
{

    public enum eItemType //열거형 타입
    {
        //int, string 둘다 표현된다
        //앞에 자료에 숫자를 정의해주면 그의 따라 값이 변화
        None,
        PowerUp,
        HpRecovery,

    }

    [SerializeField] eItemType ItemType;

    float moveSpeed;//움직이는 속도
    Vector3 moveDirection;//움직이는 방향

    [SerializeField] float minSpeed = 1;
    [SerializeField] float maxSpeed = 3;

    //사운드 기능 제공할때 전달하는 기능
    //List<AudioClip> listOb = new List<AudioClip>();
    Limiter limiter;
    

  


    private void Awake()
    {

        //eItemType enumValue = eItemType.None;

        moveSpeed = Random.Range(minSpeed, maxSpeed);
        moveDirection.x = Random.Range(-1.0f, 1.0f);
        moveDirection.y = Random.Range(-1.0f, 1.0f);
        //float directionX = Random.Range(-1.0f, 1.0f);
        //float directiony = Random.Range(-1.0f, 1.0f);

        moveDirection.Normalize();//벡터에서 힘을 버리고 방향만 지시
        //moveDirection = Vector3.Normalize(new Vector3(1,1,0));


    }

    // Start is called before the first frame update
    void Start()
    {
        limiter = GameManager.Instance._Limiter;
    }

    // Update is called once per frame
    void Update()
    {
        //+=를 해야 transform.position에 수치를 증가 시킬수 있다
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        checkItemPos();

    }

    private void checkItemPos()
    {
        (bool _x, bool _y) rDate = limiter.IsReflecItem(transform.position, moveDirection);
        //var rDate = limiter.IsReflecItem(transform.position, moveDirection);
        //IsReflecItem(포지션,자료형)
        //뒤에 자료형에 따라서 변수를 정의한다 
        //마우스 갖다대도 자료형이 보인다
        if (rDate._x == true) 
        {
            //moveDirection.x *= -1;
            moveDirection = Vector3.Reflect(moveDirection, Vector3.right);
        }
        if (rDate._y == true)
        {
            //moveDirection.x *= -1;
            moveDirection = Vector3.Reflect(moveDirection, Vector3.up);
        }

    }

    public eItemType GetItemType() 
    {
        return ItemType;
    }

}

