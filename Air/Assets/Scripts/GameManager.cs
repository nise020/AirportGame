using Newtonsoft.Json;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;//싱글톤,null 채워줘야함
    [Header("적기들")]
    [SerializeField] List<GameObject> listEnemy;
    GameObject fabExplosion;
    [SerializeField] GameObject fabBoss;

    //매니저가 원본의 레퍼런스를 가지고 있다
    //실제 데이터를 가지고 있는 변수는 private를 유지한다
    [Header("적 생성여부")]
    [SerializeField] bool isSpawn = false;//보스가 등장하거나 원하는 사유가 있을때 이값을
    //true 로 변경하면 적들이 더이상 나오지 않게하는 용도로 활용
    [SerializeField] Color sliderDefaultColor;
    [SerializeField] Color sliderBossSpwanColor;

    //WaitForSeconds halfTime = new WaitForSeconds(0.5f);
    //이런식으로 사용 해야한다!!!

    bool isSpawnBoss = false;//보스가 현재 등장중인지 확인
    bool IsSpawnBoss 
    {
        set
        {
            isSpawnBoss = value;
            StartCoroutine(sliderColorChange(value));
        }
    }

    IEnumerator sliderColorChange(bool _spawnBoss)//true가 되면 보스가 출동해서 체력바로 사용할 용도
    {
        float timer = 0.0f;

        if (_spawnBoss == true) 
        {
            while (timer < 1.0f) //조건문이 참일때
            {
                timer += Time.deltaTime;
                if (timer > 1.0f)
                {
                    timer = 1.0f;
                }
                if (_spawnBoss == true)
                {
                    imgSliderFill.color = Color.Lerp(sliderDefaultColor, sliderBossSpwanColor, timer);
                }
                else
                {
                    imgSliderFill.color = Color.Lerp(sliderBossSpwanColor, sliderDefaultColor, timer);
                }
                yield return null;
            }
        }
        
        //필수로 적어야 하는 문구
        //yield return null;//양보하다
        //yield return halfTime;
        //yield return new WaitForSeconds(0.5f);//이렇게 새용하면 RAM에서 계속 동적할당을 받기 때문에 메모리가 부족해짐
        //가비지 컬렉터가 많이 사용되면 프레임이 떨어진다
    }

    [Header("적 생성시간")]
    float enemySpawnTimer = 0.0f;//0초에작되는 타이머
    [SerializeField,Range(0.1f,5f)] float spawnTime =1.0f;

    [Header("적 생성위치")]
    [SerializeField] Transform trsSpawnPosition;
    [SerializeField] Transform trsDynamicObject;

    [Header("드롭아이템")]
    [SerializeField] List<GameObject> listItem;

    [Header("드롭 확률")]
    [SerializeField, Range(0.01f, 100.0f)] float itemDropRate;

    [Header("체력 게이지")]
    [SerializeField] FunctionHP functionHP;
    [SerializeField] Slider slider;
    [SerializeField] Image imgSliderFill;

    [Header("보스 포지션")]
    [SerializeField] Transform trsBossPosition;
    public Transform TrsBossPosition => trsBossPosition;//get 기능


    Limiter limiter;
    public Limiter _Limiter
    {
        get { return limiter; }
        set { limiter = value; }
    }

    Player player;
    public Player _Player
    {
        get { return player; }
        set { player = value; }
    }



    [Header("보스출현 조건")]
    [SerializeField] int killCount = 100;
    [SerializeField] int curKillCount = 90;
    [SerializeField] TMP_Text textSlider;

    [SerializeField] float bossSpawnTime = 60;
    [SerializeField] float bossSpawnTimer = 0f;

    [Header("점수")]
    [SerializeField] TMP_Text textScore;
    private int score;

    private bool gameStart =false;

    [Header("스타트텍스트")]
    [SerializeField] TMP_Text textStart;

    [Header("게임오버메뉴")]
    [SerializeField] GameObject objGameOverMenu;
    [SerializeField] TMP_Text textGameOverMenuScore;
    [SerializeField] TMP_Text textGameOverMenuRank;
    [SerializeField] TMP_Text textGameOverMenuBtn;
    [SerializeField] TMP_InputField IFGameOverMenuRank;
    [SerializeField] Button btnGameOverMenu;






    public bool GetPlayerPosition(out Vector3 _pos) 
    {
        _pos = default;
        if (player == null)
        {
            return false;
        }
        else 
        {
            _pos = player.transform.position;
            return true;
        }

    }
    
    public GameObject FabExplosion//정보를 전달 혹은 가져와야할때만 함수로서 사용가능
    {
        get
        {
            return fabExplosion;
        }
        //set { fabExplosion = value; }
    }
    #region 또 다른 예시
    //public GameObject GetExplosionObject()//함수로 어떤 레퍼런스를 전달
    //{
    //    return fadExplosion;//기능만 전달
    //}
    //public void setExplosionObject(GameObject _value)//함수로 어떤 레퍼런스를 전달
    //{
    //    fadExplosion = _value;//전달만 기능
    //}
    #endregion

    //인스팩터 값이 변동이 있을떄 강제 호출
    //private void OnValidate()
    //{
    //    if (Application.isPlaying == false) return;

    //    if (spawnTime < 0.1f) 
    //    {
    //        spawnTime = 0.1f;
    //    }

    //}

    private void Awake()
    {
        if (Tool.isStating == false) 
        {
            UnityEngine.SceneManagement.SceneManager.LoadScene(0);
        }

        //1개만 존재해야함
        if (Instance == null)
        {
            Instance = this;//=GameManager가 된다
        }
        else//인스턴스가 이미 존재한다면 나는 지워져야함
        {
            //오브젝트가 지워지면서 스크립트도 같이 지워짐
            Destroy(gameObject);//삭제 기능
            //Destroy(this);//이러면 컴포넌트만 삭제됨
        }
        fabExplosion = Resources.Load<GameObject>("Effect/Test/fabExplosion");

        initSlider();


    }
    private void initSlider() 
    {

        //킬카운트 버전 
        //slider.minValue = 0;
        //slider.maxValue = killCount;
        //slider.value = 0;
        //textSlider.text = $"{curKillCount.ToString("d4")} / {killCount.ToString("d4")}";

        //타이머 버전
        slider.minValue = 0;
        slider.maxValue = bossSpawnTime;
        modifySlider();

    }

    void Start()
    {
        StartCoroutine(doStartText());
    }
    IEnumerator doStartText()
    {
        Color color = textStart.color;
        color.a = 0f;
        textStart.color = color;

        while (true)
        {
            color = textStart.color;
            color.a += Time.deltaTime;
            if (color.a > 1.0f)
            {
                color.a = 1.0f;
            }
            textStart.color = color;

            if (color.a == 1.0f)
            {
                break;
            }
            yield return null;
        }

        while (true)
        {
            color = textStart.color;
            color.a -= Time.deltaTime;
            if (color.a < 0.0f)
            {
                color.a = 0.0f;
            }
            textStart.color = color;

            if (color.a == 0.0f)
            {
                break;
            }
            yield return null;
        }

        Destroy(textStart.gameObject);

        gameStart = true;
        isSpawn = true;
    }

    void Update()//프레임당 한번 실행되는 함수
    {
        if (gameStart == false) return;
        createEnemy();
        checkTimer();
        
    }

    private void checkTimer()
    {
        if(isSpawnBoss==false) 
        {
            bossSpawnTimer += Time.deltaTime;
            modifySlider();
            if (bossSpawnTimer >= bossSpawnTime)//시간 변경이 완료 되고 보스 출현
            {
                checkSpawnBoss();
            }
        }
       
    }

    //public void

    private void createEnemy()
    {
        if (isSpawn == false) return;

        enemySpawnTimer += Time.deltaTime;
        if (enemySpawnTimer > spawnTime) 
        {
            //적을 생성
            int count = listEnemy.Count;//개의 적기 0~2
            int iRoad = Random.Range(0, count);//0, 3 =0,1,2(int)

            //float min,max 값 max까지 출력
            //float min,max 값 양수일때 max-1 음수일때 max+1까지 출력

            Vector3 defulatPos = trsSpawnPosition.position;//y => 7 
            float x = Random.Range(limiter.WorldPosLimitMin.x, limiter.WorldPosLimitMax.x);//x => -2.4 ~ 2.4
            defulatPos.x = x;

           GameObject go = Instantiate(listEnemy[iRoad], defulatPos, Quaternion.identity,trsDynamicObject);
            //생성할 위치,다이나믹 오브젝트 위치가 필요
            //게임 오브젝트를 밖으로 꺼낸다

            //주사위를 굴림
            float rate = Random.Range(0.0f, 100.0f);
            if (rate <= itemDropRate) 
            {
                //적기가 아이템을 가지고 있음
                Enemy goSc = go.GetComponent<Enemy>();
                goSc.SetItem();
            }
            enemySpawnTimer = 0.0f;
            #region 메모
            //Debug.Log(Random.Range(0, count));
            //float 데이터를 이용해서 랜덤을 사용시에는 0.0~1.0 까지 숫자중 래덤값을 리턴
            //int 데이터를 이용해서 랜덤을 사용 0 1입력시 맥스 값에서 -1한 데이터를 가지고 리턴
            //0 -10일 경우 +1처리함
            #endregion
        }
    }
    /// <summary>
    /// 아이템을 랜덤으로 생성
    /// </summary>
    /// <param name="_pos"></param>
    public void createItem(Vector3 _pos) 
    {
        int count = listItem.Count;
        int iRand = Random.Range(0, count);
        Instantiate(listItem[iRand], _pos, Quaternion.identity, trsDynamicObject);
    }

    public void CreateItem(Vector3 _pos, Item. eItemType _type) 
    {
        if (_type == Item.eItemType.None) return;
        Instantiate(listItem[(int)_type - 1], _pos, Quaternion.identity, trsDynamicObject);
    }
    public void SetHp(float _maxHp, float _curHp)
    {
        //펑션hp에게 알려줘야한다
        functionHP.SetHp(_maxHp, _curHp);
    }

    public void AddKillCount() 
    {
        curKillCount++;
        //modifySlider();
        //checkSpawnBoss();

    }

    public void AddScore(int _value) 
    {
        score += _value;
        textScore.text = $"{score.ToString("d8")}";
        //d8 = 00000000
    
    }
    private void modifySlider() 
    {
        //킬 카운트 버전
        //slider.value = curKillCount;
        //textSlider.text = $"{curKillCount.ToString("d4")} / {killCount.ToString("d4")}";

        //타이머 버전
        slider.value = bossSpawnTimer;
        textSlider.text = $"{((int)bossSpawnTimer).ToString("d4")} / {((int)bossSpawnTime).ToString("d4")}";//0000//정수부
        //float->(int)형변환
    }



    private void checkSpawnBoss() 
    {
        //킬 카운트 버전
        //if (isSpawnBoss == false && curKillCount >= killCount) 
        //{
        //    isSpawn = false;
        //    isSpawnBoss = true;//보스가 현재 등장중인지 확인

        //    GameObject go = Instantiate(fabBoss, trsSpawnPosition.position, Quaternion.identity, trsDynamicObject);
        //}
        //타이버 버전
        if (isSpawnBoss == false)//보스 출현
        {
            isSpawn = false;
            IsSpawnBoss = true;

            GameObject go = Instantiate(fabBoss, trsSpawnPosition.position, Quaternion.identity, trsDynamicObject);
            EnemyBoss goSc = go.GetComponent<EnemyBoss>();  
            //보스체력은 최대 몇으로 시작했는지
            setSliderBossType(goSc.Hp);

        }
    }

    private void setSliderBossType(float _maxHp) 
    {
        slider.maxValue = _maxHp;
        slider.value = _maxHp;
        textSlider.text = $"{(int)_maxHp} / {(int)_maxHp}";
    }
    public void modifyBossHp (float _hp)
    {
        slider.value = _hp;
        textSlider.text = $"{(int)_hp} / {(int)slider.maxValue}";
    }
    public void KillBoss() 
    {
        bossSpawnTimer = 0;
        bossSpawnTime += 10;
        //난이도 추가 기능

        isSpawn = true;
        initSlider();
        IsSpawnBoss=false;
        
    }
    public void GameOver() //여기 수정
    {
        List<cUserDate> listUserData =
           JsonConvert.DeserializeObject<List<cUserDate>>(PlayerPrefs.GetString(Tool.rankKey));

        int rank = -1;//0 이 1등
        int count = listUserData.Count;
        for (int iNum = 0; iNum < count; iNum++) 
        {
            cUserDate userDate = listUserData[iNum];
            if(userDate.Score < score) 
            {
                rank = iNum;
                break;
            }
        }
        textGameOverMenuScore.text = $"점수 :{score.ToString("d8")}";

        if (rank != 1) 
        {
            textGameOverMenuRank.text = $"랭킹 : {rank + 1}등";
            IFGameOverMenuRank.gameObject.SetActive(true);
            textGameOverMenuBtn.text = "등록";
        }
        else 
        {
            textGameOverMenuRank.text = "랭크인 하지 못했습니다";
            IFGameOverMenuRank.gameObject.SetActive(false);
            textGameOverMenuBtn.text = "메인메뉴로";
        }

        //textGameOverMenuRank.text = rank != -1 ? $"랭킹 : {rank + 1}등" : "랭크인 하지 못했습니다";
        //IFGameOverMenuRank.gameObject.SetActive(rank != -1);
        //textGameOverMenuBtn.text = rank != -1 ? "등록" : "메인메뉴로";

        btnGameOverMenu.onClick.AddListener(() =>
        {
            //랭크인을 했다면 랭크와 이름을 저장
            if (rank != -1)
            {
                string name = IFGameOverMenuRank.text;

                if (name == string.Empty)
                {
                    name = "AAA";
                }

                cUserDate newRank = new cUserDate();
                newRank.Score = score;
                newRank.Name = name;

                listUserData.Insert(rank, newRank);
                listUserData.RemoveAt(listUserData.Count - 1);

                string value = JsonConvert.SerializeObject(listUserData);
                PlayerPrefs.SetString((Tool.rankKey), value);
            }

            FunctionFade.Instance.ActiveFade(true, () =>
            {
                UnityEngine.SceneManagement.SceneManager.LoadScene(0);
                FunctionFade.Instance.ActiveFade(false,null);
            });
        });
        objGameOverMenu.SetActive(true);
    }


    
    
}

