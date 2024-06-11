using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System.IO;//C#,In put+out Put
using Newtonsoft.Json;
using UnityEngine.SceneManagement;//���� �̵��ϴ� ���

public class StartSceneManager : MonoBehaviour
{
    //�ν����͸� ���ؼ���ư�� �Ű������� �����Ҷ�
    //2���� �̻��� �Ű������� ����X

    [SerializeField] Button btnStart;
    [SerializeField] Button btnRanking;
    [SerializeField] Button btnExitRanking;
    [SerializeField] Button btnGameExit;
    [SerializeField] GameObject viewRank;

    [Header("��ũ ������")]
    [SerializeField] GameObject fadRank;
    [SerializeField] Transform contents;

    private void Awake()
    {

        Tool.isStating = true;
        #region �޳�
        //btnStart.onClick.AddListener(() => { FunctionStart(); });

        //btnStart.onClick.AddListener(() => 
        //{
        //    gameStart(1, 2, 3, 4, 5);
        //});

        //UnityAction<float> action = (float x)  => { Debug.Log(x); };
        //���ٽ�
        //�̸����� �Լ�
        //action.Invoke(0.1f);//���ٽ��� Ư�� �̺�Ʈ�� Invoke�� ���ؼ� ���డ��
        #endregion
        btnStart.onClick.AddListener(gameStart);
        btnRanking.onClick.AddListener(showRanking);
        btnExitRanking.onClick.AddListener(() => { viewRank.SetActive(false); });
        btnGameExit.onClick.AddListener(gameExit);
        #region json,Prefs �޸�
        //json
        //string ���ڿ� Ű�ͺ���
        //json = {key:value}

        //save ���,�Ű� ���� �̵��Ҷ� ������ �����ϴ� �����Ͱ� ������

        //1.�÷��̾� �ս��� ����� ����Ƽ�� ���� �ϴ� ���
        //PlayerPrefs//����Ƽ�� ������ �����͸� �����ϵ��� ����Ƽ ���ο� ����
        //�����͸� �������� �ʴ��� �����
        //set int, set flot
        //PlayerPrefs.SetInt("test",999);//������ ������ �ϸ� �ҷ��ü� ����

        //int value =PlayerPrefs.GetInt("test");//
        //Debug.Log(value);
        //PlayerPrefs.DeleteKey("test");//test��� �̸��� ��� �ִ� �����͸� ����
        //Debug.Log(value);//int�� ����Ʈ�� 0�� ����

        //PlayerPrefs.hasKey
        //PlayerPrefs.HasKey("test");
        #endregion
        #region Path,File�� ���� ���� ����

        //string path = Application.streamingAssetsPath;//os�� ���� �б� �������� ���� 
        //C:/Users/User/My project (3)/Assets/StreamingAssets

        //File.WriteAllText(path+"/abc.json","�׽�Ʈ");
        //�ش� ��ġ�� abc.json ���� ����
        //���� �̸��� ���� ��� ������ ������

        //File.Delete(path + "/abc.json");//�ش� ���� ����
        //string result = File.ReadAllText(path + "/abc.json");
        //Debug.Log(result);


        //string path = Application.persistentDataPath+"/Jsons";//R/W�б�,���Ⱑ ������ ���� ��ġ
        //C:/Users/User/AppData/LocalLow/DefaultCompany/My project (3)/Jsons

        //if (Directory.Exists(path) == false) 
        //{
        //    Directory.CreateDirectory(path);
        //}

        //if (File.Exists(path + "Test/abc.json") == true) 
        //{
        //    string result = File.ReadAllText(path + "Test/abc.json");
        //}
        //else//������ ���� ���� ����
        //{
        //    //���ο� ���� ��ġ�� �����͸� �������� ��

        //    File.Create(path + "/Test");
        //}
        #endregion
        #region JsonUtility
        //cUserDate cUserDate = new cUserDate();
        //cUserDate.Name = "������";
        //cUserDate.Score = 100;

        //cUserDate cUserDate2 = new cUserDate();
        //cUserDate2.Name = "�󸶹�";
        //cUserDate2.Score = 200;

        //List<cUserDate> listUserDate = new List<cUserDate>();
        //listUserDate.Add(cUserDate);
        //listUserDate.Add(cUserDate2);

        //string jsonConvert = JsonConvert.SerializeObject(cUserDate);
        //Debug.Log(jsonConvert);
        //"com.unity.nuget.newtonsoft-json": "3.2.1"
        //manifest.json ���ο� �������� ���α׷� ����� ������ ����

        //List<cUserDate> listUserDate3 = new List<cUserDate>();
        //listUserDate.Add(new );
        //listUserDate.Add(cUserDate2);

        //string jsonDate = JsonUtility.ToJson(listUserDate);
        //0

        // string jsonDate= JsonUtility.ToJson(cUserDate);//�ӵ��� ������
        // Debug.Log(jsonDate);
        //{"Name":"������","Score":100}

        // cUserDate user2 = new cUserDate();//�̷� ����� �����ϴ�
        //user2 = JsonUtility.FromJson<cUserDate>(jsonDate);
        //{"Name":"������","Score":100}


        //JsonUtility�� List�� json���� �����ϸ� Ʈ������ ������


        #endregion

        initRankView();
        viewRank.SetActive(false);
    }
    /// <summary>
    /// ��ũ�� ����Ǿ� �ִٸ� ����� ��ũ �����͸� �̿��ؼ� ��ũ�並 ������ְ�
    /// ��ũ�� ����Ǿ� ���� �ʴٸ� ����ִ� ��ũ�� ����� ��ũ�並 �������
    /// </summary>
    private void initRankView()
    {
        List<cUserDate> listUserData = null;
        clearRankView();
        if (PlayerPrefs.HasKey(Tool.rankKey) == true)//��ũ �����Ͱ� ������ �Ǿ��־��ٸ�
        {
            listUserData = JsonConvert.DeserializeObject<List<cUserDate>>(PlayerPrefs.GetString(Tool.rankKey));
        }
        else//��ũ�����Ͱ� ����Ǿ� ���� �ʾҴٸ�
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
        //SceneManager.LoadScene(0);//���ξ�//0->0(�ٽ� ���� �� ���)
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
        //�����Ϳ��� �÷��� ���� ���.������ ���� ���
        //���带 ���ؼ� ������ ������ �������� �ȵ�
        //��ó��,�ڵ尡 ���ǿ� ���ؼ� ������ ���°� ó�� ��������

#if UNITY_EDITOR//��ó���� ���� ǥ�ð� �Ⱥ���
       UnityEditor.EditorApplication.isPlaying = false;
#else//�����Ϳ��� ������� ������
 //���� ������ ��������
 Application.Quit();
#endif
        //���� ������ ��������
        //����Ƽ �����Ϳ����� ���ᰡ ����
  
    }
}
