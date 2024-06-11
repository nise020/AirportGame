using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anim;
    
    [Header("플레이어 설정"), SerializeField, Tooltip("플레이어의 이동속도")] float moveSpeed;
    #region 메모
    //또는 [Header("플레이어 설정")][SerializeField] float moveSpeed;
    //[Tooltip("설명")]- 마우스 갖다 대면 설명이 뜨는 기능
    //[Header("설명")]- 대제목을 설정하는 기능
    #endregion
    Vector3 moveDir;

    [Header("총알")]
    [SerializeField] GameObject fabBullet;//플레이어가 복제해서 사용할 원본 총알
    [SerializeField] GameObject fabBullet2;//플레이어가 복제해서 사용할 원본 총알
    [SerializeField] GameObject fabBullet3;//플레이어가 복제해서 사용할 원본 총알
    [SerializeField] GameObject fabBullet4;//플레이어가 복제해서 사용할 원본 총알
    [SerializeField] GameObject fabBullet5;//플레이어가 복제해서 사용할 원본 총알
    [SerializeField] Transform dynamicObject;//해당 항목에 생성
    [SerializeField] bool autoFire = false;//자동공격기능
    [SerializeField] float fireRateTime = 0.5f;//이시간이 지나면 총알이 발사됨
    float fireTimer = 0;
    #region 메모
    //[SerializeField, TextArea] string Text;
    //TextArea-넓은 텍스트 공간 생성(enter 가능)
    #endregion
    GameManager gameManager;
    GameObject fabExplosion;
    Limiter limiter;
    SpriteRenderer spriteRenderer;

    [Header("체력")]
    [SerializeField] int maxHp = 3;
    [SerializeField] int curHp;
    int beforeHp;
    bool invicibility = false;
    [SerializeField] float invicibilityTime=2f;
    float invicibilityTimer;

    [Header("플레이어 레벨")]
    [SerializeField] int minlevel=1;
    [SerializeField] int maxLevel=5;
    [SerializeField, Range(1, 5)] int curLevel;

    //[SerializeField] float distanceBullet;//2레벨 이상시 총알이 중심으로 부터 벌어지는 거리
    //[SerializeField] float angleBullet;//4레벨 이상시 총알이 회전하는 값
    [SerializeField] Transform shootTrs;
    //[SerializeField] Transform shootTrsLevel4;//4레벨 이상시 총알이 발사될 위치
    //[SerializeField] Transform shootTrsLevel5;//4레벨 이상시 총알이 발사될 위치

    private void OnValidate()//인스펙터에서 어떤값이 변동이 생기면 호출
    {
        if (Application.isPlaying == false) 
        {
            return;
        }

        if (beforeHp != curHp) 
        {
            beforeHp = curHp;
            GameManager.Instance.SetHp(maxHp, curHp);
        }
    }
    /// <summary>
    /// 오브젝트 충돌할때 기능
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Tool.GetTag(GameTags.Enemy)) 
        {
            //체력 감소
            Hit();

            //체력이 0이 되면 게임이 끝남
            //점수가 랭크인이 되면 이름 입력하는 기능
            //메인 메뉴에서 1~10등 랭크

            //짧은시간 무적

            //게이지 변화코드 실행
        }
        else if (collision.tag == Tool.GetTag(GameTags.Item))
        {
            Item item = collision.GetComponent<Item>();
            Destroy(item.gameObject);//이 함수는 이 함수가 모든 동작을 마치게 되면 삭제해달라고 예약

            if (item.GetItemType() == Item.eItemType.PowerUp)
            {
                curLevel++;
                if (curLevel >= maxLevel) 
                {
                    curLevel = maxLevel;
                }
                //공격방식 강화

                //발사체 추가
            }
            else if (item.GetItemType() == Item.eItemType.HpRecovery) 
            {
                //체력 회복
                curHp++;
                if (curHp > maxHp) 
                {
                    curHp = maxHp;
                }
                gameManager.SetHp(maxHp, curHp);
            }
        }
    }


    //[RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    //private static  void initCode()
    //{
    //    Debug.Log("initCode");
    //}

    private void Awake()
    {
        anim = transform.GetComponent<Animator>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        //gameManager = GameManager.Instance;
        curLevel = minlevel;
        curHp = maxHp;
    }

    private void Start()
    {
       //cam = GameObject.Find("Main Camera").GetComponent<Camera>();
        gameManager = GameManager.Instance;
        fabExplosion = gameManager.FabExplosion;
        gameManager._Player = this;
    }

    void Update()
    {
        //함수 실행 순서가 중요하다
        moving();
        doAnimation();
        checkPlayerPos();
        
        shoot();
        checkinvicibility();
    }

    private void checkinvicibility()//무적일때만 작동함 
    {
        if(invicibility==false) return;

        if (invicibilityTimer >= 0f) 
        {
            invicibilityTimer-= Time.deltaTime;
            if(invicibilityTimer < 0f) 
            {
                setSprincibilitu(false);
            }
        
        }
    }

    private void setSprincibilitu(bool _value) 
    {
        Color color = spriteRenderer.color ;

        if (_value == true)
        {
            color.a = 0.5f;
            invicibility = true;
            invicibilityTimer = invicibilityTime;
        }
        else 
        {
            color.a = 1.0f;
            invicibility = false;
            invicibilityTimer = 0;
        }
        spriteRenderer.color = color;
    }
    #region 설명
    /// <summary>
    /// 플레이 기체의 기동을 정의 합니다.
    /// </summary>
    #endregion
    private void moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal");//왼쪽 혹은 오른쪽 입력// -1 0 1
        moveDir.y = Input.GetAxisRaw("Vertical");//위 혹은 아래 입력 // -1 0 1

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// 애니메이션에 어떤 애니메이션을 실행할지 파라미터를 전달 합니다.
    /// </summary>
    private void doAnimation()//하나의 함수에는 하나의 기능
    {
        anim.SetInteger("Horizontal", (int)moveDir.x);
    }
    #region 메모
    //transform.position 월드포지션 좌표
    //transform.localPosition => 이 데이터가 Root데이터라면 알아서 월드 포지션 좌표를 출력
    //                            이 데이;터
    #endregion 

    private void checkPlayerPos() 
    {
        if (limiter == null)
        {
            limiter = gameManager._Limiter;
        }
        transform.position =limiter.checkMovePosition(transform.position,false);
    }


    private void shoot()
    {
        #region Input 메모
        //    //입력,방향키
        //    Input.GetMouseButtonDown(0);//마우스 버튼을 누를때
        //    Input.GetMouseButton(0);//마우스 버튼을 누르고 있을때
        //    Input.GetMouseButtonUp(0);//마우스 버튼에서 손을 땠을때
        //    //()안에 int데이터
        //    //0번은 왼쪽버튼,1번은 오른쪽버튼,2번은 휠클릭

        //    Input.GetKeyDown(KeyCode.);
        //    Input.GetKey(KeyCode.);
        //    Input.GetKeyUp(KeyCode.);
        //    //Alpha0~9 위쪽 숫자패드
        #endregion
        if (autoFire == false && Input.GetKeyDown(KeyCode.Space) == true)//유저가 스페이스 키를 누른다면
        {
            createBullet();
        }
        else if (autoFire == true)
        {
            //일정시간이 지나면 총알 한발 발사
            fireTimer += Time.deltaTime;//1초가 지나면 1이 될수있도록 소수점들이 fireTimer에 쌓임
            if(fireTimer > fireRateTime) 
            {
                createBullet();
                fireTimer = 0;
            }
        }
    }
    
    private void createBullet()//총알을 생성한다
    {
        if (curLevel == 1) 
        {
            GameObject go = Instantiate(fabBullet, shootTrs.position, Quaternion.identity, dynamicObject);
            Bullet goSc = go.GetComponent<Bullet>();
            goSc.ShootPlayer();

            //instBullet(shootTrs.position, Quaternion.identity);          
        }  
        else if(curLevel == 2) 
        {
            Instantiate(fabBullet2, shootTrs.position, Quaternion.identity, dynamicObject);

            //instBullet(shootTrs.position + new Vector3(distanceBullet, 0, 0), Quaternion.identity);
            //instBullet(shootTrs.position - new Vector3(distanceBullet, 0, 0), Quaternion.identity); 
        }
        else if (curLevel == 3)
        {
            Instantiate(fabBullet3, shootTrs.position, Quaternion.identity, dynamicObject);

            //instBullet(shootTrs.position, Quaternion.identity);
            //instBullet(shootTrs.position + new Vector3(distanceBullet, 0, 0), Quaternion.identity);
            //instBullet(shootTrs.position - new Vector3(distanceBullet, 0, 0), Quaternion.identity);
        }
        else if (curLevel == 4)//미사일 5개
        {
            Instantiate(fabBullet4, shootTrs.position, Quaternion.identity, dynamicObject);

            //instBullet(shootTrs.position, Quaternion.identity);
            //instBullet(shootTrs.position + new Vector3(distanceBullet, 0, 0), Quaternion.identity);
            //instBullet(shootTrs.position - new Vector3(distanceBullet, 0, 0), Quaternion.identity);

            //Vector3 lv4Pos = shootTrsLevel4.position;
            ////위치 지정
            //LinstBullet(lv4Pos, new Vector3(0, 0, angleBullet));
            ////0,0,회전값
            //Vector3 lv4localPos = shootTrsLevel4.localPosition;
            ////localPosition: 부모(shootTrsLevel4)의 포지션
            //lv4localPos.x *= -1;
            //lv4localPos += transform.position;

            //LinstBullet(lv4localPos, new Vector3(0, 0, -angleBullet));
            ////반대편 위치에 같은 미사일 생성
        }
        else if (curLevel == 5)//미사일 7개
        {
            Instantiate(fabBullet5, shootTrs.position, Quaternion.identity, dynamicObject);

            //instBullet(shootTrs.position, Quaternion.identity);
            //instBullet(shootTrs.position + new Vector3(distanceBullet, 0, 0), Quaternion.identity);
            //instBullet(shootTrs.position - new Vector3(distanceBullet, 0, 0), Quaternion.identity);

            //Vector3 lv4Pos = shootTrsLevel4.position;
            //LinstBullet(lv4Pos, new Vector3(0, 0, angleBullet));

            //Vector3 lv4localPos = shootTrsLevel4.localPosition;
            //lv4localPos.x *= -1;
            //lv4localPos += transform.position;

            //LinstBullet(lv4localPos, new Vector3(0, 0, -angleBullet));

            //Vector3 lv5Pos = shootTrsLevel5.position;
            ////위치 지정
            //LinstBullet(lv5Pos, new Vector3(0, 0, angleBullet));
            ////0,0,회전값
            //Vector3 lv5localPos = shootTrsLevel5.localPosition;
            ////localPosition: 부모(shootTrsLevel5)의 포지션
            //lv5localPos.x *= -1;
            //lv5localPos += transform.position;

            //LinstBullet(lv5localPos, new Vector3(0, 0, -angleBullet));
            ////반대편 위치에 같은 미사일 생성
        }
    }
    #region 총알발사 코드로 사용하는 예시
    private void LinstBullet(Vector3 _pos,Vector3 _angle) 
    {
        //쿼터니언
        GameObject go = Instantiate(fabBullet,_pos, Quaternion.Euler(_angle), dynamicObject);
        Bullet goSc = go.GetComponent<Bullet>();
        goSc.ShootPlayer();
    }

    private void instBullet(Vector3 _pos, Quaternion _quat)
    {
        GameObject go = Instantiate(fabBullet, _pos, _quat, dynamicObject);
        Bullet goSc = go.GetComponent<Bullet>();
        goSc.ShootPlayer();
    }
    #endregion
    public void Hit() 
    {
        //무적상태라면 데미지를 받지 않음
        if (invicibility == true) return;

        setSprincibilitu(true);
        curHp--;
        if (curHp < 0) 
        {
            curHp= 0;
        }

        GameManager.Instance.SetHp(maxHp, curHp);

        curLevel--;
        if (curLevel < minlevel) 
        {
            curLevel= minlevel;  
        }
        if (curHp == 0) 
        {
            Destroy(gameObject);
            GameObject go = Instantiate(fabExplosion, transform.position, Quaternion.identity, transform.parent);
            Explosion goSc = go.GetComponent<Explosion>();

            //직사각형
            goSc.setImageSize(spriteRenderer.sprite.rect.width);//현재 기체의 이미지 길이를 넣어줌

            gameManager.GameOver();
        }
    
    }
}
