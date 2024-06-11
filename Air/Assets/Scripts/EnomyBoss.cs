using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBoss : Enemy
{

    public float Hp => hp;//get
    //{
    //    get 
    //    {
    //        return hp;
    //    }
    //}
    //Enemy에 기능을 상속 받는다
    Transform trsBossPosition;//도착할 위치

    bool isMovingTrsBossPosition=false;//보스가 원위치까지 이동을 완료했는지
    bool patternChange = false;//패턴을 바꾸고 그동안 유저가 극딜할 타이밍을 만들어줌

    Vector3 createPos = Vector3.zero;
    Vector3 velocity = Vector3.zero;
    float timer = 0.0f;
    float velocityX = 0f;
    float velocityY = 0f;

    bool isSwayRight = false;

    [Header("현재위치에서 전방으로")]
    [SerializeField] private int pattern1Count = 10;
    [SerializeField] private float pattern1Reload = 0.5f;
    [SerializeField] private GameObject pattern1Fab;

    [Header("샷건")]
    [SerializeField] private int pattern2Count = 5;
    [SerializeField] private float pattern2Reload = 0.3f;
    [SerializeField] private GameObject pattern2Fab;

    [Header("조준발사")]
    [SerializeField] private int pattern3Count = 30;
    [SerializeField] private float pattern3Reload = 0.1f;
    [SerializeField] private GameObject pattern3Fab;

    Limiter limiter;

    private int curPattern = 1;
    private int curPatternShootCount = 0;
    //Vector3 moveDirection;


   [Header("발사위치")]
    [SerializeField] List<Transform> trsShootPos;
    //public으로 선언하거나 시리얼라이즈 필드로 선언하여
    //인스펙터에서 변형해서 사용시에는 따로 동적할당을 받을 필요가 없음
    Animator anim;


    //[System.Serializable]
    //List<cPattern>데이터를 정렬화,직렬화 하는 기능
    //public class cPattern//내가 직접 정의했음 
    //{
    //    //RAM 상태라서 찾을수 없다
    //    [TextArea] public string explain;//셜명문,대제목 지정하기
    //    //[TextArea]<- enter가 가능한 영역생성
    //    public int pattern3Count = 30;
    //    public float pattern3Reload = 0.1f;
    //    public GameObject pattern3Fab;
    //}

    //[SerializeField] List<cPattern> listPattern;

    //부모 스크립트에 protected virtual을 추가해두면
    //자식의 스크립트가 실행할때 같이 실행됨
    // protected override 상속받기+수정해서 사용 가능

    protected override void Start()
    {
        gameManager = GameManager.Instance;
        trsBossPosition = gameManager.TrsBossPosition;
        fabExplosion = gameManager.FabExplosion;
        createPos = transform.position;//현재위치
        anim = GetComponent<Animator>();


    }

    protected override void moving()
    {
        if (isMovingTrsBossPosition == false)
        {
            if (timer < 1.0f) 
            {
                timer += Time.deltaTime;
                //transform.position = Vector3.Lerp(createPos, trsBossPosiTion.position, timer);
                #region Lerp 종류
                //transform.position = Vector3.SmoothDamp(createPos, trsBossPosiTion.position,ref velocity, 3f);//3개의 값을
                //Lerp:선형보간
                //LerpUnclamped
                //Slerp:곡선을 그리며 해당 위치 이동
                //SmoothDamp:도착시간을 정할수 있다
                #endregion

                #region SmoothDamp 예시
                //float posX = Mathf.SmoothDamp(transform.position.x, trsBossPosiTion.position.x,ref velocityX,2f);
                //float posy = Mathf.SmoothDamp(transform.position.y, trsBossPosiTion.position.y,ref velocityy,2f);
                //transform.position=new Vector3(posX,posy,0);
                //단점 시간으로 움직인다
                #endregion

                #region SmoothStep 예시
                //float posX = Mathf.SmoothStep(createPos.x, trsBossPosiTion.position.x, timer/3);
                //float posY = Mathf.SmoothStep(createPos.y, trsBossPosiTion.position.y, timer/3);
                //transform.position = new Vector3(posX, posY, 0);
                //시간 비율에 따라 속도가 다르다
                #endregion

                float posX = Mathf.SmoothStep(createPos.x, trsBossPosition.position.x, timer);
                float posY = Mathf.SmoothStep(createPos.y, trsBossPosition.position.y, timer);
                transform.position = new Vector3(posX, posY, 0);

                if (timer >= 1.0f) 
                {
                    isMovingTrsBossPosition = true;
                    timer = 0f;
                }
            }
            return;
        }
         //이동완료후 좌우로 이동하면서 패턴에 의해서 공격
        if( isSwayRight==true)
        {
            transform.position += Vector3.right * Time.deltaTime * moveSpeed;
        }
        else  //(isSwayRight == false)
        {
            transform.position += Vector3.left * Time.deltaTime * moveSpeed;
        }
        cheakMovingLimit();
        //base.moving();<-인용할때 base를 붙인다
        //화면 위에서 어느위치 까지 이동
        //이동 완료후 좌우로 이동하면서 패턴 공격
    }
    protected override void shooting()
    {
        if (isMovingTrsBossPosition == false) 
        {
            return;
        }

        timer += Time.deltaTime;

        if (patternChange == true) 
        {
            if (timer >= 1.0f) 
            {
                timer = 0.0f;
                patternChange = false;
            }
            return ;
        }
        if (curPattern == 1) //전방으로 발사
        {
            if (timer >= pattern1Reload) 
            {
                timer = 0.0f;
                shootStraght();
                if (curPatternShootCount >= pattern1Count)
                {
                    curPattern++;
                    patternChange = true;
                    curPatternShootCount = 0;
                }

            }
        }
        else if (curPattern == 2)//샷건
        {

            if (timer >= pattern2Reload)
            {
                timer = 0.0f;
                shootShotgun();
                if (curPatternShootCount >= pattern2Count)
                {
                    curPattern++;
                    patternChange = true;
                    curPatternShootCount = 0;
                }

            }
        }
        else if (curPattern == 3)//조준발사
        {

            if (timer >= pattern3Reload)
            {
                timer = 0.0f;
                shootMinigun();
                if (curPatternShootCount >= pattern3Count)
                {
                    curPattern = 1;//다시 1번 패턴으로
                    patternChange = true;
                    curPatternShootCount = 0;
                }

            }
        }
    }

    private void shootMinigun()
    {
        //마그니튜트
        //플레이어의 위치로 발사
        Vector3 playerPos;//디폴트:x 0,y 0,z 0
        if(gameManager.GetPlayerPosition(out playerPos)==true) 
        {

            Vector3 distans = playerPos -transform.position;//플레이어의 위치로 부터 내위치의 거리
            //float force = distans.magnitude;
            //Vector3.Distance(transform.position, playerPos);//마그니튜트

            float angle = Quaternion.FromToRotation(Vector3.up, distans).eulerAngles.z;
            //플레이어와 보스왕, 거리 오차를 이용해 Y축 0도로 부터 오차 위치의 각도 z를 구함

            Instantiate(pattern3Fab, trsShootPos[4].position, Quaternion.Euler(new Vector3(0, 0, angle))
        , transform.parent);
        }
        curPatternShootCount++;
    }

    private void shootShotgun()
    {
        
        Instantiate(pattern2Fab, trsShootPos[4].position, Quaternion.Euler(new Vector3(0, 0, 180f)), transform.parent);
        Instantiate(pattern2Fab, trsShootPos[4].position, Quaternion.Euler(new Vector3(0, 0, 180f - 15f)), transform.parent);
        Instantiate(pattern2Fab, trsShootPos[4].position, Quaternion.Euler(new Vector3(0, 0, 180f + 15f)) , transform.parent);
        Instantiate(pattern2Fab, trsShootPos[4].position, Quaternion.Euler(new Vector3(0, 0, 180f - 30f)), transform.parent);
        Instantiate(pattern2Fab, trsShootPos[4].position, Quaternion.Euler(new Vector3(0, 0, 180f + 30f)) , transform.parent);
        Instantiate(pattern2Fab, trsShootPos[4].position, Quaternion.Euler(new Vector3(0, 0, 180f - 45f))  , transform.parent);
        Instantiate(pattern2Fab, trsShootPos[4].position, Quaternion.Euler(new Vector3(0, 0, 180f + 45f)) , transform.parent);
        Instantiate(pattern2Fab, trsShootPos[4].position, Quaternion.Euler(new Vector3(0, 0, 180f + 60f)) , transform.parent);
        Instantiate(pattern2Fab, trsShootPos[4].position, Quaternion.Euler(new Vector3(0, 0, 180f))  , transform.parent);
        curPatternShootCount++;
    }

    private void shootStraght()
    {
        int count = 4;
        for (int iNum = 0; iNum < count; iNum++) 
        {
            Instantiate(pattern1Fab, trsShootPos[iNum].position, Quaternion.Euler(new Vector3(0, 0, 180f))
            , transform.parent);
        }
        curPatternShootCount++;
    }

    private void cheakMovingLimit() 
    {
        if (limiter == null) 
        {
           limiter = gameManager._Limiter;
        }

        if (limiter.checkMovePosition(transform.position) == true)
        {
            isSwayRight = !isSwayRight;
        }

        //float halfWidth = spriteRenderer.sprite.rect.width*0.5f;
        //float posX =transform.position.x;//중점
        //if((isSwayRight == false && limiter.checkMovePosition(transform.position) == true)||
        //   (isSwayRight == true  && limiter.checkMovePosition(transform.position) == true))
        //{
        //    isSwayRight = !isSwayRight;
        //}

    }

    

    public override void Hit(float _damage)
    {
        if (isDied == true)
        {
            return;
        }

        hp -= _damage;
        gameManager.modifyBossHp(hp);

        if (hp <= 0)
        {
            isDied = true;
            Destroy(gameObject);
            //매니저를 호출후 현재 내 위치를 전달하면 매니저가 아이템을 그 위치에 만들어줌
  
            GameObject go = Instantiate(fabExplosion, transform.position, Quaternion.identity, transform.parent);
            Explosion goSc = go.GetComponent<Explosion>();

            //직사각형
            goSc.setImageSize(spriteRenderer.sprite.rect.width);//현재 기체의 이미지 길이를 넣어줌

            gameManager.CreateItem(transform.position, Item.eItemType.PowerUp);
            gameManager.CreateItem(transform.position, Item.eItemType.HpRecovery);

            gameManager.AddScore(score);
            //gameManager.AddKillCount();보스가 죽었다고 전달 //다시 적들이 스폰되게 변경
            gameManager.KillBoss();
        }
        else
        {
            //이 친구는 스프라이트만 활용하는것이 아닌 스프라이트 애니메이션을
            //활용함으로서 에니메이션에서 히트애니를 남김
            anim.SetTrigger("BossHit");
        }
    }
}
