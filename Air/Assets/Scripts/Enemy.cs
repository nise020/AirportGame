using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Enemy : MonoBehaviour
{
    //privat �ڽĿ��� �ƹ� �����͵� ���� ����
    //protected �ڽĵ� ��� �����ϰ� ����

    public enum eEnoeyType 
    {
        EnemyA,
        Enemyb,
        Enemyc,
        EnemyBoss
    }
    [SerializeField] eEnoeyType scores;
    #region ������Ƽ�� ������
    [SerializeField] protected float moveSpeed;
    [SerializeField] protected float hp;
    protected bool isDied = false;//���Ⱑ �װ��� ���̻� ����� �ݺ��������� �ʵ��� ����
    protected GameObject fabExplosion;
    protected GameManager gameManager;
    protected SpriteRenderer spriteRenderer;
    #endregion

    #region ��������Ʈ ������
    Sprite defaultSprite;
    [SerializeField] Sprite hitSprite;
    bool haveItem = false;
    [Header("������ ������ �÷�")]
    [SerializeField] Color colorHaveItem;
    #endregion

    [Header("�ı��� ����")]
    [SerializeField] protected int score;
    private void OnBecameInvisible()
    {
        Destroy(gameObject);
    }
    protected virtual void Awake()
    {
        //�θ� ��ũ��Ʈ�� protected virtual�� �߰��صθ�
        //�ڽ��� ��ũ��Ʈ�� �����Ҷ� ���� �����
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
        #region ����
        //spriteRenderer = GetComponent<SpriteRenderer>();
        //defaultSprite = spriteRenderer.sprite;
        //gameManager = GameManager.Instance;

        ////�Լ��� �̿��� ���
        //fabExplosion = gameManager.GetExplosionObject();
        //gameManager.setExplosionObject(fabExplosion);

        ////get , set����� �̿��ѹ��
        //fabExplosion = gameManager.FabExplosion;
        //gameManager.FabExplosion = fabExplosion;

        //fadExplosion = Resources.Load<GameObject>("Esffext/fadExplosion ");
        //�̷������� ���ҽ� �ȿ� �ִ� �����͸� �������� ��쿡�� Ȯ���ڸ� �Է��ϸ� �ȵ�
        //ã�� ����� �������̶� ������,�� �Ȼ�� �ϰų� ���� �����縸 ����
        //("����/������Ʈ ��")
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
            Destroy(gameObject);//������ ����
            #region �����¿��� �� �ڸ�
            //�����¿��� �� �ڸ�
            //�Ŵ����κ��� �޾ƿ� ���� ������ �� ��ġ�� �����ϰ�
            //�θ�� ������� ���̾ �������
            #endregion
            GameObject go = Instantiate(fabExplosion, transform.position, Quaternion.identity, transform.parent);
            Explosion goSc = go.GetComponent<Explosion>();

            //���簢��
            goSc.setImageSize(spriteRenderer.sprite.rect.width);//���� ��ü�� �̹��� ���̸� �־���

            //�Ŵ����� ȣ���� ���� �� ��ġ�� �����ϸ� �Ŵ����� �������� �� ��ġ�� �������
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
            //hit ���� ��������Ʈ ������
            spriteRenderer.sprite = hitSprite;
            //�ణ�� �ð��� �����ڿ� � �Լ��� �����ϰ� ������
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
