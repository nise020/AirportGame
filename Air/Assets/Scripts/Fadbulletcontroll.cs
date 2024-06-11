using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Fadbulletcontroll : MonoBehaviour
{
    private void Awake()
    {
        int count = transform.childCount;
        for (int iNum = 0; iNum < count; iNum++) 
        {
            GameObject go = transform.GetChild(iNum).gameObject;
            Bullet goSc = go.GetComponent<Bullet>();
            goSc.ShootPlayer();

            //transform.GetChild(iNum).gameObject.GetComponent<Bullet>().ShootPlayer();
            //이렇게 사용하면 기능은 하지만 문제 방생하면 머가 문제인지 알기 힘들다
        }
    }

    // Update is called once per frame
    void Update()
    {
        checkCild();
    }

    private void checkCild() 
    {
        //transform.GetChild(0);
        int count = transform.childCount;
        if (count == 0) 
        {
            Destroy(gameObject);
        
        }
    
    }
}
