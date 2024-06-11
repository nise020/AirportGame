using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //privat 자식에게 아무 데이터도 전달 안함
    //protected 자식도 사용 가능하게 해줌

    public enum eEnoeyType 
    {
        EnemyA,
        Enemyb,
        Enemyc,
        EnemyBoss
    }
    [SerializeField] eEnoeyType scores;
    #region 프로텍티드 데이터
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float hp;
    protected bool isDied = false;//적기가 죽고나면 더이상 기능을 반복실행하지 않도록 해줌
    protected GameObject fabExplosion;
    protected GameManager gameManager;
    protected SpriteRenderer spriteRenderer;
    #endregion

    #region 프리베이트 데이터
    Sprite defaultSprite;
    [SerializeField] Sprite hitSprite;
    bool haveItem = false;
    [Header("아이템 보유시 컬러")]
    [SerializeField] Color colorHaveItem;
    #endregion

    [Header("파괴시 점수")]
    [SerializeField] protected int score;
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    protected virtual void Awake()
    {
        //부모 스크립트에 protected virtual을 추가해두면
        //자식의 스크립트가 실행할때 같이 실행됨
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;
    }

    protected virtual void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        defaultSprite = spriteRenderer.sprite;

        if (haveItem == true)
        {
            spriteRenderer.color = colorHaveItem;
            //ex. new Color(0.3f, 0.5f, 0.1f)<- RGB;
        }
        gameManager = GameManager.Instance;
        fabExplosion = gameManager.FabExplosion;
        #region 주의
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //defaultSprite = spriteRenderer.sprite;
        //gameManager = GameManager.Instance;

        ////함수를 이용한 방법
        //fabExplosion = gameManager.GetExplosionObject();
        //gameManager.setExplosionObject(fabExplosion);

        ////get , set기능을 이용한방법
        //fabExplosion = gameManager.FabExplosion;
        //gameManager.FabExplosion = fabExplosion;

        //fadExplosion = Resources.Load<GameObject>("Esffext/fadExplosion ");
        //이런식으로 리소스 안에 있는 데이터를 가져오는 경우에는 확장자를 입력하면 안됨
        //찾는 방식이 유동적이라 느려짐,잘 안사용 하거나 적말 적은양만 쓸것
        //("폴더/오브젝트 명")
        #endregion
    }

    protected virtual void Update()
    {
        moving();
        shooting();
    }

    protected virtual void moving()
    {
        transform.position -= transform.up * moveSpeed * Time.deltaTime;

        //transform.position += Vector3.down * moveSpeed * Time.deltaTime;
        //transform.position += transform.rotation * Vector3.down * moveSpeed * Time.deltaTime;
    }
    protected virtual void shooting()
    {
        
    }

    public virtual void Hit(float _damage)
    {
        if (isDied == true)
        {
            return;
        }

        hp -= _damage;

        if (hp <= 0)
        {
            isDied = true;
            Destroy(gameObject);//삭제를 예약
            #region 터지는연출 들어갈 자리
            //터지는연출 들어갈 자리
            //매니저로부터 받아온 폭발 연출을 내 위치에 생성하고
            //부모로 사용중인 레이어에 만들어줌
            #endregion
            GameObject go = Instantiate(fabExplosion, transform.position, Quaternion.identity, transform.parent);
            Explosion goSc = go.GetComponent<Explosion>();

            //직사각형
            goSc.setImageSize(spriteRenderer.sprite.rect.width);//현재 기체의 이미지 길이를 넣어줌

            //매니저를 호출후 현재 내 위치를 전달하면 매니저가 아이템을 그 위치에 만들어줌
            if (haveItem == true)
            {
                gameManager.createItem(transform.position);
            }
            gameManager.AddKillCount();
            gameManager.AddScore(score);
            
            //int score = 0;
            //if (enemyType == eEnoeyType.EnemyA) 
            //{
            //    score = 100;
            //}

        }
        else
        {
            //hit 연출 스프라이트 변경기능
            spriteRenderer.sprite = hitSprite;
            //약간의 시간이 지난뒤에 어떤 함수를 실행하고 싶을때
            Invoke("setDefaultSprite", 0.04f);
        }
    }

    private void setDefaultSprite()
    {
        spriteRenderer.sprite = defaultSprite;
    }
    public void SetItem() 
    {
        haveItem = true;
    }
}
