using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;//C#,In put+out Put
using Newtonsoft.Json;
using UnityEngine.SceneManagement;//씬을 이동하는 기능

public class StartSceneManager : MonoBehaviour
{
    //인스팩터를 통해서버튼의 매개변수를 적용할때
    //2가지 이상의 매갸변수가 적용X

    [SerializeField] Button btnStart;
    [SerializeField] Button btnRanking;
    [SerializeField] Button btnExitRanking;
    [SerializeField] Button btnGameExit;
    [SerializeField] GameObject viewRank;

    [Header("랭크 프리팹")]
    [SerializeField] GameObject fadRank;
    [SerializeField] Transform contents;

    private void Awake()
    {

        Tool.isStating = true;
        #region 메노
        //btnStart.onClick.AddListener(() => { FunctionStart(); });

        //btnStart.onClick.AddListener(() => 
        //{
        //    gameStart(1, 2, 3, 4, 5);
        //});

        //UnityAction<float> action = (float x)  => { Debug.Log(x); };
        //람다식
        //이름없는 함수
        //action.Invoke(0.1f);//람다식은 특정 이벤트나 Invoke를 통해서 실행가능
        #endregion
        btnStart.onClick.AddListener(gameStart);
        btnRanking.onClick.AddListener(showRanking);
        btnExitRanking.onClick.AddListener(() => { viewRank.SetActive(false); });
        btnGameExit.onClick.AddListener(gameExit);
        #region json,Prefs 메모
        //json
        //string 문자열 키와벨류
        //json = {key:value}

        //save 기능,신과 신을 이동할때 가지고 가야하는 데이터가 있으면

        //1.플레이어 팹스를 사용해 유니티에 저장 하는 방법
        //PlayerPrefs//유니티가 꺼져도 데이터를 보관하도록 유니티 내부에 저장
        //데이터를 삭제하지 않는한 저장됨
        //set int, set flot
        //PlayerPrefs.SetInt("test",999);//게임을 삭제를 하면 불러올수 없음

        //int value =PlayerPrefs.GetInt("test");//
        //Debug.Log(value);
        //PlayerPrefs.DeleteKey("test");//test라는 이름에 들어 있는 데이터를 삭제
        //Debug.Log(value);//int에 디폴트인 0이 나옴

        //PlayerPrefs.hasKey
        //PlayerPrefs.HasKey("test");
        #endregion
        #region Path,File을 통한 파일 생성

        //string path = Application.streamingAssetsPath;//os에 따라 읽기 전용으로 사용됨 
        //C:/Users/User/My project (3)/Assets/StreamingAssets

        //File.WriteAllText(path+"/abc.json","테스트");
        //해당 위치에 abc.json 파일 생성
        //파일 이름이 같은 경우 내용이 수정됨

        //File.Delete(path + "/abc.json");//해당 파일 삭제
        //string result = File.ReadAllText(path + "/abc.json");
        //Debug.Log(result);


        //string path = Application.persistentDataPath+"/Jsons";//R/W읽기,쓰기가 가능한 폴더 위치
        //C:/Users/User/AppData/LocalLow/DefaultCompany/My project (3)/Jsons

        //if (Directory.Exists(path) == false) 
        //{
        //    Directory.CreateDirectory(path);
        //}

        //if (File.Exists(path + "Test/abc.json") == true) 
        //{
        //    string result = File.ReadAllText(path + "Test/abc.json");
        //}
        //else//저장한 파일 존재 ㄴㄴ
        //{
        //    //새로운 저장 위치와 데이터를 만들어줘야 함

        //    File.Create(path + "/Test");
        //}
        #endregion
        #region JsonUtility
        //cUserDate cUserDate = new cUserDate();
        //cUserDate.Name = "가나다";
        //cUserDate.Score = 100;

        //cUserDate cUserDate2 = new cUserDate();
        //cUserDate2.Name = "라마바";
        //cUserDate2.Score = 200;

        //List<cUserDate> listUserDate = new List<cUserDate>();
        //listUserDate.Add(cUserDate);
        //listUserDate.Add(cUserDate2);

        //string jsonConvert = JsonConvert.SerializeObject(cUserDate);
        //Debug.Log(jsonConvert);
        //"com.unity.nuget.newtonsoft-json": "3.2.1"
        //manifest.json 내부에 등록해줘야 프로그램 종료시 삭제를 방지

        //List<cUserDate> listUserDate3 = new List<cUserDate>();
        //listUserDate.Add(new );
        //listUserDate.Add(cUserDate2);

        //string jsonDate = JsonUtility.ToJson(listUserDate);
        //0

        // string jsonDate= JsonUtility.ToJson(cUserDate);//속도가 빠르다
        // Debug.Log(jsonDate);
        //{"Name":"가나다","Score":100}

        // cUserDate user2 = new cUserDate();//이런 방법도 가능하다
        //user2 = JsonUtility.FromJson<cUserDate>(jsonDate);
        //{"Name":"가나다","Score":100}


        //JsonUtility는 List를 json으로 변경하면 트러블이 존재함


        #endregion

        initRankView();
        viewRank.SetActive(false);
    }
    /// <summary>
    /// 랭크가 저장되어 있다면 저장된 랭크 데이터를 이용해서 랭크뷰를 만들어주고
    /// 랭크가 저장되어 있지 않다면 비어있는 랭크를 만들어 랭크뷰를 만들어줌
    /// </summary>
    private void initRankView()
    {
        List<cUserDate> listUserData = null;
        clearRankView();
        if (PlayerPrefs.HasKey(Tool.rankKey) == true)//랭크 데이터가 저장이 되어있었다면
        {
            listUserData = JsonConvert.DeserializeObject<List<cUserDate>>(PlayerPrefs.GetString(Tool.rankKey));
        }
        else//랭크데이터가 저장되어 있지 않았다면
        {
            listUserData = new List<cUserDate>();
            int rankCount = Tool.rankCount;
            for (int iNum = 0; iNum < rankCount; ++iNum)
            {
                listUserData.Add(new cUserDate() { Name = "None", Score = 0 });
            }
            string vale =JsonConvert.SerializeObject(listUserData);
            PlayerPrefs.GetString(Tool.rankKey,vale );

        }

        int count = listUserData.Count;
        for (int iNum = 0; iNum < count; ++iNum)
        {
            cUserDate data = listUserData[iNum];

            GameObject go = Instantiate(fadRank, contents);
            FabRanking goSc = go.GetComponent<FabRanking>();
            goSc.SetData((iNum + 1).ToString(), data.Name, data.Score);
        }
    }
    private void clearRankView() 
    {
        int count = contents.childCount;
        for (int iNum = count - 1; iNum > -1; --iNum)
        {
            Destroy(contents.GetChild(iNum).gameObject);
        }

    }

    //private void Function(int vale) 
    //{
    //}
    private void gameStart()
    {
        //SceneManager.LoadScene(0);//메인씬//0->0(다시 시작 할 경우)
        FunctionFade.Instance.ActiveFade(true, () =>
        {
            SceneManager.LoadScene(1);
            FunctionFade.Instance.ActiveFade(false,null);
        });


    }
    private void showRanking() 
    {
        viewRank.SetActive(true);
    }
    private void gameExit()
    {
        //에디터에서 플레이 끄는 방법.에디터 전용 기능
        //빌드를 통해서 밖으로 가지고 나가서는 안됨
        //전처리,코드가 조건에 의해서 본인이 없는것 처럼 동착해줌

#if UNITY_EDITOR//전처리는 에러 표시간 안보임
       UnityEditor.EditorApplication.isPlaying = false;
#else//에디터에서 실행되지 않을때
 //빌드 했을때 게임종료
 Application.Quit();
#endif
        //빌드 했을때 게임종료
        //유니티 에디터에서는 종료가 ㄴㄴ
  
    }
}
