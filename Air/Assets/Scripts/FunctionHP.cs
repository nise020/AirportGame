using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
/// <summary>
/// 플레이어의 HP가 변동이 생기면 HP게이지를 즉시 변경하고 Effect가 해당 게이지로 초당 변동하게 만들어 줍니다
/// </summary>
public class FunctionHP : MonoBehaviour
{
    //회사에서는 적단 프로그램을 사용 안한다
    //빌드할때 오류가 발생해서..
    //컨트롤+.

    [SerializeField] Image imgHp;
    [SerializeField] Image imgEffect;

    [SerializeField, Range(0.1f, 10f)] float effectTime = 1;//0은 나눌수 없으니 주의
    GameManager gameManager;

    bool isEnded=false;
    //Transform trsPosPlayer;

    private void Awake()
    {
        initHp();
    }
    private void Start()
    {
        gameManager = GameManager.Instance;
        //trsPosPlayer = GameManager.Instance._Player.transform;
    }
    private void initHp()
    {
        imgHp.fillAmount = 1;
        imgEffect.fillAmount = 1;
        //fillAmount 게이지를 1로 고정한다
    }
    void Update()
    {
        checkFillAmount();
        chasePlayer();
        chekedPlayerDestroy();
    }
    private void checkFillAmount()
    {
        if (imgHp.fillAmount == imgEffect.fillAmount)
        {
            return;
        }
        if (imgHp.fillAmount < imgEffect.fillAmount)
        {
            // (Time.deltaTime * 0.5f) 속도 조절
            imgEffect.fillAmount -= (Time.deltaTime / effectTime);
            if (imgHp.fillAmount > imgEffect.fillAmount)
            {
                imgEffect.fillAmount = imgHp.fillAmount;
            }
        }
        else if (imgHp.fillAmount > imgEffect.fillAmount)
        {
            imgEffect.fillAmount = imgHp.fillAmount;
        }
    }
    private void chasePlayer()
    {
        if (gameManager.GetPlayerPosition(out Vector3 pos) == true)
        {
            pos.y -= 0.7f;
            transform.position = pos;// = pos - new Vector3(0,0.7f,0);
        }
        //imgEffect.enabled = true; //껏다 켯다
    }
    public void SetHp(float _maxHp, float _curHp)//0~1
    {
        imgHp.fillAmount = _curHp / _maxHp;
    }

    private void chekedPlayerDestroy()
    {
        //isEnded = true;
        //if (isEnded = false && gameManager._Player == null)
        //{
        //    isEnded = true;
        //    Destroy();
        //}
        if (imgEffect.fillAmount == 0.1f) 
        {
            Destroy(gameObject);       
        }
    }
}
