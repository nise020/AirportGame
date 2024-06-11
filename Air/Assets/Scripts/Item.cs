using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class Item : MonoBehaviour
{

    public enum eItemType //������ Ÿ��
    {
        //int, string �Ѵ� ǥ���ȴ�
        //�տ� �ڷῡ ���ڸ� �������ָ� ���� ���� ���� ��ȭ
        None,
        PowerUp,
        HpRecovery,

    }

    [SerializeField] eItemType ItemType;

    float moveSpeed;//�����̴� �ӵ�
    Vector3 moveDirection;//�����̴� ����

    [SerializeField] float minSpeed = 1;
    [SerializeField] float maxSpeed = 3;

    //���� ��� �����Ҷ� �����ϴ� ���
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

        moveDirection.Normalize();//���Ϳ��� ���� ������ ���⸸ ����
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
        //+=�� �ؾ� transform.position�� ��ġ�� ���� ��ų�� �ִ�
        transform.position += moveDirection * moveSpeed * Time.deltaTime;
        checkItemPos();

    }

    private void checkItemPos()
    {
        (bool _x, bool _y) rDate = limiter.IsReflecItem(transform.position, moveDirection);
        //var rDate = limiter.IsReflecItem(transform.position, moveDirection);
        //IsReflecItem(������,�ڷ���)
        //�ڿ� �ڷ����� ���� ������ �����Ѵ� 
        //���콺 ���ٴ뵵 �ڷ����� ���δ�
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

