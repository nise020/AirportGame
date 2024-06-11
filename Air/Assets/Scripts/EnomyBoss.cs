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
    //Enemy�� ����� ��� �޴´�
    Transform trsBossPosition;//������ ��ġ

    bool isMovingTrsBossPosition=false;//������ ����ġ���� �̵��� �Ϸ��ߴ���
    bool patternChange = false;//������ �ٲٰ� �׵��� ������ �ص��� Ÿ�̹��� �������

    Vector3 createPos = Vector3.zero;
    Vector3 velocity = Vector3.zero;
    float timer = 0.0f;
    float velocityX = 0f;
    float velocityY = 0f;

    bool isSwayRight = false;

    [Header("������ġ���� ��������")]
    [SerializeField] private int pattern1Count = 10;
    [SerializeField] private float pattern1Reload = 0.5f;
    [SerializeField] private GameObject pattern1Fab;

    [Header("����")]
    [SerializeField] private int pattern2Count = 5;
    [SerializeField] private float pattern2Reload = 0.3f;
    [SerializeField] private GameObject pattern2Fab;

    [Header("���ع߻�")]
    [SerializeField] private int pattern3Count = 30;
    [SerializeField] private float pattern3Reload = 0.1f;
    [SerializeField] private GameObject pattern3Fab;

    Limiter limiter;

    private int curPattern = 1;
    private int curPatternShootCount = 0;
    //Vector3 moveDirection;


   [Header("�߻���ġ")]
    [SerializeField] List<Transform> trsShootPos;
    //public���� �����ϰų� �ø�������� �ʵ�� �����Ͽ�
    //�ν����Ϳ��� �����ؼ� ���ÿ��� ���� �����Ҵ��� ���� �ʿ䰡 ����
    Animator anim;


    //[System.Serializable]
    //List<cPattern>�����͸� ����ȭ,����ȭ �ϴ� ���
    //public class cPattern//���� ���� �������� 
    //{
    //    //RAM ���¶� ã���� ����
    //    [TextArea] public string explain;//�ȸ�,������ �����ϱ�
    //    //[TextArea]<- enter�� ������ ��������
    //    public int pattern3Count = 30;
    //    public float pattern3Reload = 0.1f;
    //    public GameObject pattern3Fab;
    //}

    //[SerializeField] List<cPattern> listPattern;

    //�θ� ��ũ��Ʈ�� protected virtual�� �߰��صθ�
    //�ڽ��� ��ũ��Ʈ�� �����Ҷ� ���� �����
    // protected override ��ӹޱ�+�����ؼ� ��� ����

    protected override void Start()
    {
        gameManager = GameManager.Instance;
        trsBossPosition = gameManager.TrsBossPosition;
        fabExplosion = gameManager.FabExplosion;
        createPos = transform.position;//������ġ
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
                #region Lerp ����
                //transform.position = Vector3.SmoothDamp(createPos, trsBossPosiTion.position,ref velocity, 3f);//3���� ����
                //Lerp:��������
                //LerpUnclamped
                //Slerp:��� �׸��� �ش� ��ġ �̵�
                //SmoothDamp:�����ð��� ���Ҽ� �ִ�
                #endregion

                #region SmoothDamp ����
                //float posX = Mathf.SmoothDamp(transform.position.x, trsBossPosiTion.position.x,ref velocityX,2f);
                //float posy = Mathf.SmoothDamp(transform.position.y, trsBossPosiTion.position.y,ref velocityy,2f);
                //transform.position=new Vector3(posX,posy,0);
                //���� �ð����� �����δ�
                #endregion

                #region SmoothStep ����
                //float posX = Mathf.SmoothStep(createPos.x, trsBossPosiTion.position.x, timer/3);
                //float posY = Mathf.SmoothStep(createPos.y, trsBossPosiTion.position.y, timer/3);
                //transform.position = new Vector3(posX, posY, 0);
                //�ð� ������ ���� �ӵ��� �ٸ���
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
         //�̵��Ϸ��� �¿�� �̵��ϸ鼭 ���Ͽ� ���ؼ� ����
        if( isSwayRight==true)
        {
            transform.position += Vector3.right * Time.deltaTime * moveSpeed;
        }
        else  //(isSwayRight == false)
        {
            transform.position += Vector3.left * Time.deltaTime * moveSpeed;
        }
        cheakMovingLimit();
        //base.moving();<-�ο��Ҷ� base�� ���δ�
        //ȭ�� ������ �����ġ ���� �̵�
        //�̵� �Ϸ��� �¿�� �̵��ϸ鼭 ���� ����
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
        if (curPattern == 1) //�������� �߻�
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
        else if (curPattern == 2)//����
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
        else if (curPattern == 3)//���ع߻�
        {

            if (timer >= pattern3Reload)
            {
                timer = 0.0f;
                shootMinigun();
                if (curPatternShootCount >= pattern3Count)
                {
                    curPattern = 1;//�ٽ� 1�� ��������
                    patternChange = true;
                    curPatternShootCount = 0;
                }

            }
        }
    }

    private void shootMinigun()
    {
        //���״�ƩƮ
        //�÷��̾��� ��ġ�� �߻�
        Vector3 playerPos;//����Ʈ:x 0,y 0,z 0
        if(gameManager.GetPlayerPosition(out playerPos)==true) 
        {

            Vector3 distans = playerPos -transform.position;//�÷��̾��� ��ġ�� ���� ����ġ�� �Ÿ�
            //float force = distans.magnitude;
            //Vector3.Distance(transform.position, playerPos);//���״�ƩƮ

            float angle = Quaternion.FromToRotation(Vector3.up, distans).eulerAngles.z;
            //�÷��̾�� ������, �Ÿ� ������ �̿��� Y�� 0���� ���� ���� ��ġ�� ���� z�� ����

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
        //float posX =transform.position.x;//����
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
            //�Ŵ����� ȣ���� ���� �� ��ġ�� �����ϸ� �Ŵ����� �������� �� ��ġ�� �������
  
            GameObject go = Instantiate(fabExplosion, transform.position, Quaternion.identity, transform.parent);
            Explosion goSc = go.GetComponent<Explosion>();

            //���簢��
            goSc.setImageSize(spriteRenderer.sprite.rect.width);//���� ��ü�� �̹��� ���̸� �־���

            gameManager.CreateItem(transform.position, Item.eItemType.PowerUp);
            gameManager.CreateItem(transform.position, Item.eItemType.HpRecovery);

            gameManager.AddScore(score);
            //gameManager.AddKillCount();������ �׾��ٰ� ���� //�ٽ� ������ �����ǰ� ����
            gameManager.KillBoss();
        }
        else
        {
            //�� ģ���� ��������Ʈ�� Ȱ���ϴ°��� �ƴ� ��������Ʈ �ִϸ��̼���
            //Ȱ�������μ� ���ϸ��̼ǿ��� ��Ʈ�ִϸ� ����
            anim.SetTrigger("BossHit");
        }
    }
}
