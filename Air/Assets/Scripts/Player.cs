using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    Animator anim;
    
    [Header("�÷��̾� ����"), SerializeField, Tooltip("�÷��̾��� �̵��ӵ�")] float moveSpeed;
    #region �޸�
    //�Ǵ� [Header("�÷��̾� ����")][SerializeField] float moveSpeed;
    //[Tooltip("����")]- ���콺 ���� ��� ������ �ߴ� ���
    //[Header("����")]- �������� �����ϴ� ���
    #endregion
    Vector3 moveDir;

    [Header("�Ѿ�")]
    [SerializeField] GameObject fabBullet;//�÷��̾ �����ؼ� ����� ���� �Ѿ�
    [SerializeField] GameObject fabBullet2;//�÷��̾ �����ؼ� ����� ���� �Ѿ�
    [SerializeField] GameObject fabBullet3;//�÷��̾ �����ؼ� ����� ���� �Ѿ�
    [SerializeField] GameObject fabBullet4;//�÷��̾ �����ؼ� ����� ���� �Ѿ�
    [SerializeField] GameObject fabBullet5;//�÷��̾ �����ؼ� ����� ���� �Ѿ�
    [SerializeField] Transform dynamicObject;//�ش� �׸� ����
    [SerializeField] bool autoFire = false;//�ڵ����ݱ��
    [SerializeField] float fireRateTime = 0.5f;//�̽ð��� ������ �Ѿ��� �߻��
    float fireTimer = 0;
    #region �޸�
    //[SerializeField, TextArea] string Text;
    //TextArea-���� �ؽ�Ʈ ���� ����(enter ����)
    #endregion
    GameManager gameManager;
    GameObject fabExplosion;
    Limiter limiter;
    SpriteRenderer spriteRenderer;

    [Header("ü��")]
    [SerializeField] int maxHp = 3;
    [SerializeField] int curHp;
    int beforeHp;
    bool invicibility = false;
    [SerializeField] float invicibilityTime=2f;
    float invicibilityTimer;

    [Header("�÷��̾� ����")]
    [SerializeField] int minlevel=1;
    [SerializeField] int maxLevel=5;
    [SerializeField, Range(1, 5)] int curLevel;

    //[SerializeField] float distanceBullet;//2���� �̻�� �Ѿ��� �߽����� ���� �������� �Ÿ�
    //[SerializeField] float angleBullet;//4���� �̻�� �Ѿ��� ȸ���ϴ� ��
    [SerializeField] Transform shootTrs;
    //[SerializeField] Transform shootTrsLevel4;//4���� �̻�� �Ѿ��� �߻�� ��ġ
    //[SerializeField] Transform shootTrsLevel5;//4���� �̻�� �Ѿ��� �߻�� ��ġ

    private void OnValidate()//�ν����Ϳ��� ����� ������ ����� ȣ��
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
    /// ������Ʈ �浹�Ҷ� ���
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.tag == Tool.GetTag(GameTags.Enemy)) 
        {
            //ü�� ����
            Hit();

            //ü���� 0�� �Ǹ� ������ ����
            //������ ��ũ���� �Ǹ� �̸� �Է��ϴ� ���
            //���� �޴����� 1~10�� ��ũ

            //ª���ð� ����

            //������ ��ȭ�ڵ� ����
        }
        else if (collision.tag == Tool.GetTag(GameTags.Item))
        {
            Item item = collision.GetComponent<Item>();
            Destroy(item.gameObject);//�� �Լ��� �� �Լ��� ��� ������ ��ġ�� �Ǹ� �����ش޶�� ����

            if (item.GetItemType() == Item.eItemType.PowerUp)
            {
                curLevel++;
                if (curLevel >= maxLevel) 
                {
                    curLevel = maxLevel;
                }
                //���ݹ�� ��ȭ

                //�߻�ü �߰�
            }
            else if (item.GetItemType() == Item.eItemType.HpRecovery) 
            {
                //ü�� ȸ��
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
        //�Լ� ���� ������ �߿��ϴ�
        moving();
        doAnimation();
        checkPlayerPos();
        
        shoot();
        checkinvicibility();
    }

    private void checkinvicibility()//�����϶��� �۵��� 
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
    #region ����
    /// <summary>
    /// �÷��� ��ü�� �⵿�� ���� �մϴ�.
    /// </summary>
    #endregion
    private void moving()
    {
        moveDir.x = Input.GetAxisRaw("Horizontal");//���� Ȥ�� ������ �Է�// -1 0 1
        moveDir.y = Input.GetAxisRaw("Vertical");//�� Ȥ�� �Ʒ� �Է� // -1 0 1

        transform.position += moveDir * moveSpeed * Time.deltaTime;
    }

    /// <summary>
    /// �ִϸ��̼ǿ� � �ִϸ��̼��� �������� �Ķ���͸� ���� �մϴ�.
    /// </summary>
    private void doAnimation()//�ϳ��� �Լ����� �ϳ��� ���
    {
        anim.SetInteger("Horizontal", (int)moveDir.x);
    }
    #region �޸�
    //transform.position ���������� ��ǥ
    //transform.localPosition => �� �����Ͱ� Root�����Ͷ�� �˾Ƽ� ���� ������ ��ǥ�� ���
    //                            �� ����;��
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
        #region Input �޸�
        //    //�Է�,����Ű
        //    Input.GetMouseButtonDown(0);//���콺 ��ư�� ������
        //    Input.GetMouseButton(0);//���콺 ��ư�� ������ ������
        //    Input.GetMouseButtonUp(0);//���콺 ��ư���� ���� ������
        //    //()�ȿ� int������
        //    //0���� ���ʹ�ư,1���� �����ʹ�ư,2���� ��Ŭ��

        //    Input.GetKeyDown(KeyCode.);
        //    Input.GetKey(KeyCode.);
        //    Input.GetKeyUp(KeyCode.);
        //    //Alpha0~9 ���� �����е�
        #endregion
        if (autoFire == false && Input.GetKeyDown(KeyCode.Space) == true)//������ �����̽� Ű�� �����ٸ�
        {
            createBullet();
        }
        else if (autoFire == true)
        {
            //�����ð��� ������ �Ѿ� �ѹ� �߻�
            fireTimer += Time.deltaTime;//1�ʰ� ������ 1�� �ɼ��ֵ��� �Ҽ������� fireTimer�� ����
            if(fireTimer > fireRateTime) 
            {
                createBullet();
                fireTimer = 0;
            }
        }
    }
    
    private void createBullet()//�Ѿ��� �����Ѵ�
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
        else if (curLevel == 4)//�̻��� 5��
        {
            Instantiate(fabBullet4, shootTrs.position, Quaternion.identity, dynamicObject);

            //instBullet(shootTrs.position, Quaternion.identity);
            //instBullet(shootTrs.position + new Vector3(distanceBullet, 0, 0), Quaternion.identity);
            //instBullet(shootTrs.position - new Vector3(distanceBullet, 0, 0), Quaternion.identity);

            //Vector3 lv4Pos = shootTrsLevel4.position;
            ////��ġ ����
            //LinstBullet(lv4Pos, new Vector3(0, 0, angleBullet));
            ////0,0,ȸ����
            //Vector3 lv4localPos = shootTrsLevel4.localPosition;
            ////localPosition: �θ�(shootTrsLevel4)�� ������
            //lv4localPos.x *= -1;
            //lv4localPos += transform.position;

            //LinstBullet(lv4localPos, new Vector3(0, 0, -angleBullet));
            ////�ݴ��� ��ġ�� ���� �̻��� ����
        }
        else if (curLevel == 5)//�̻��� 7��
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
            ////��ġ ����
            //LinstBullet(lv5Pos, new Vector3(0, 0, angleBullet));
            ////0,0,ȸ����
            //Vector3 lv5localPos = shootTrsLevel5.localPosition;
            ////localPosition: �θ�(shootTrsLevel5)�� ������
            //lv5localPos.x *= -1;
            //lv5localPos += transform.position;

            //LinstBullet(lv5localPos, new Vector3(0, 0, -angleBullet));
            ////�ݴ��� ��ġ�� ���� �̻��� ����
        }
    }
    #region �Ѿ˹߻� �ڵ�� ����ϴ� ����
    private void LinstBullet(Vector3 _pos,Vector3 _angle) 
    {
        //���ʹϾ�
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
        //�������¶�� �������� ���� ����
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

            //���簢��
            goSc.setImageSize(spriteRenderer.sprite.rect.width);//���� ��ü�� �̹��� ���̸� �־���

            gameManager.GameOver();
        }
    
    }
}
