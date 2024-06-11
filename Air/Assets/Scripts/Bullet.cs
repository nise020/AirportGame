using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    bool isSootEnemy = true;
    //���⿡ ������� or �÷��̾ �������
    //���ʵڿ� ������ٰ� ���������
    //ȭ������� ��������

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
        //ī�޶� ������ ��������� ���
        //OnBecameVisible�� �ݴ���
    }
    /// <summary>
    /// ������Ʈ �浹�Ҷ� ���
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)//collision�� ��� �ݸ���
    {
        //���� ����� ��Ȯ�� �� �ʿ䰡 ����
        if (isSootEnemy == false && collision.tag == "Enemy")
        {
            Destroy(gameObject);//�Ѿ� ������ ����
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.Hit(1);
        }
        if (isSootEnemy == true && collision.tag == "Player")
        {
            Destroy(gameObject);//�Ѿ� ������ ����
            Player player = collision.GetComponent<Player>();
            player.Hit();
        }
    }

    void Start()
    {
        //Destroy(gameObject, 2.5f);
        //2.5�� �� �������
    }

    void Update()
    {
        #region ����+�÷�����
        //transform.position = new Vector3(0,1,0)* moveSpeed*Time.deltaTime;
        //transform.position += transform.rotation * Vector3.up * moveSpeed*Time.deltaTime;
        //ȸ���� ���� ���� �������� ����
        //transform.rotation
        #endregion 
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }
    public void ShootPlayer() 
    {
        isSootEnemy= false;
    }
}
