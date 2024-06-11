using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Bullet : MonoBehaviour
{
    [SerializeField] float moveSpeed;
    bool isSootEnemy = true;
    //적기에 닿았을때 or 플레이어에 닿았을때
    //몇초뒤에 사라진다고 명령했을때
    //화면밖으로 나갔을때

    private void OnBecameInvisible()
    {
        Destroy(gameObject);
        //카메라 밖으로 사라졌을때 기능
        //OnBecameVisible은 반대기능
    }
    /// <summary>
    /// 오브젝트 충돌할때 기능
    /// </summary>
    private void OnTriggerEnter2D(Collider2D collision)//collision은 상대 콜리전
    {
        //때릴 대상을 정확히 할 필요가 있음
        if (isSootEnemy == false && collision.tag == "Enemy")
        {
            Destroy(gameObject);//총알 본인이 삭제
            Enemy enemy = collision.GetComponent<Enemy>();
            enemy.Hit(1);
        }
        if (isSootEnemy == true && collision.tag == "Player")
        {
            Destroy(gameObject);//총알 본인이 삭제
            Player player = collision.GetComponent<Player>();
            player.Hit();
        }
    }

    void Start()
    {
        //Destroy(gameObject, 2.5f);
        //2.5초 뒤 사라진다
    }

    void Update()
    {
        #region 설명+늘려쓴것
        //transform.position = new Vector3(0,1,0)* moveSpeed*Time.deltaTime;
        //transform.position += transform.rotation * Vector3.up * moveSpeed*Time.deltaTime;
        //회전의 대한 값이 존재하지 않음
        //transform.rotation
        #endregion 
        transform.position += transform.up * moveSpeed * Time.deltaTime;
    }
    public void ShootPlayer() 
    {
        isSootEnemy= false;
    }
}
