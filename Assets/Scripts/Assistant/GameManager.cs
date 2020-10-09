using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;


public class GameManager : MonoBehaviour
{
    private static GameManager instance;
    public static GameManager Instance
    {
        get { return instance; }
    }
    [SerializeField]
    private GameObject gameOverPanel;
    [SerializeField]
    private GameObject gameWinPanel, LogoPanel;
    public int currentLevel;
    private InputField NameInput, PasswdInput, EmailInput;
    private string UserName;

    public bool needPause = true;
    public RectTransform stars;
    public AudioClip winAudio;
    public AudioClip loseAudio;

    private void Awake()
    {
        if (needPause)
            GamePause();
    }

    private void Start()
    {
        if (instance != null)
            Destroy(gameObject);
        if (instance == null)
            instance = this;
        LogoPanel = GameObject.Find("LogoPanel");
    }

    public void GameContinue()
    {
        Time.timeScale = 1;
        Magnet[] magnets = FindObjectsOfType<Magnet>();
        foreach (Magnet mag in magnets)
            mag.FindNextAim();
    }

    public void GamePause()
    {
        Time.timeScale = 0;
    }

    public void GameWin()
    {
        GamePause();
        gameWinPanel.SetActive(true);
        int score = Score();
        for (int i = 0; i < score; i++)
            stars.GetChild(i).gameObject.SetActive(true);    
        PostScore();
        AudioSource.PlayClipAtPoint(winAudio, Camera.main.transform.position);
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        GamePause();
        AudioSource.PlayClipAtPoint(winAudio, Camera.main.transform.position);
    }

    public void OnSave()
    {
        //todo 获取文件名
        string SaveFileName = "level" + Convert.ToString(currentLevel) + ".dat";
        FileStream fs = new FileStream(SaveFileName, FileMode.OpenOrCreate);

        if (fs == null)
        {
            Debug.Log("fail to save file!");
            return;
        }

        SaveCurrentLevel(fs);
        return;
    }

    public void SaveCurrentLevel(FileStream fs)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        List<SaveObject> SaveContent = CreateObj.GetUserObjects();
        formatter.Serialize(fs, SaveContent.Count);
        Debug.Log(SaveContent.Count);

        for (int i=0;i<SaveContent.Count;++i)
        {
            formatter.Serialize(fs, graph: SaveContent[i]);
        }

       // formatter.Serialize(fs, currentLevel)
        fs.Flush();
        fs.Close();
    }

    public void LoadSaveFile()
    { 
        string SaveFileName = "level0.dat";
        //todo 从文件名获取关卡
        Debug.Log("load file:" + SaveFileName);
        PlayerPrefs.SetString("LoadFile", SaveFileName);
        SceneManager.LoadScene(1);
        
    }

    public void OnLogin()
    {
        NameInput = GameObject.Find("LogInPanel/Panel/UserName").GetComponent<InputField>();
        PasswdInput = GameObject.Find("LogInPanel/Panel/Password").GetComponent<InputField>();
        UserName = NameInput.text;
        Debug.Log(UserName);
        string url = "http://81.71.17.48/user/login";
        LoginRequest PostData =new LoginRequest(NameInput.text,PasswdInput.text);
        StartCoroutine(SendRequest(url, JsonUtility.ToJson(PostData), RequestType.POST, OnLoginCallBack));
    }

    private void OnLoginCallBack(string RspDataString)
    {
        Debug.Log("recv msg:" + RspDataString);
        var RspData = JsonUtility.FromJson<ServerRsponse<string>>(RspDataString);

        if (RspData.Code != 0)
        {
            Debug.Log("登录失败:" + RspData.Message);
            return;
        }

        PlayerPrefs.SetString("user", UserName);
        LoadLevel(1);
        Debug.Log("登录成功!");
    }

   public void OnRegister()
   {
        NameInput = GameObject.Find("SignUpPanel/Panel/UserName").GetComponent<InputField>();
        PasswdInput = GameObject.Find("SignUpPanel/Panel/Password").GetComponent<InputField>();
        string url = "http://81.71.17.48/user/register"; 
        RegisterRequest PostData = new RegisterRequest(NameInput.text, PasswdInput.text);
        StartCoroutine(SendRequest(url, JsonUtility.ToJson(PostData), RequestType.POST, OnRegisterCallback));
    }

    private void OnRegisterCallback(string RspDataString)
    {
        Debug.Log("recv msg:" + RspDataString);
        var RspData = JsonUtility.FromJson<ServerRsponse<string>>(RspDataString);

        if (RspData.Code != 0)
        {
            Debug.Log("注册失败:" + RspData.Message);
            return;
        }

        GameObject SingUpPanel = GameObject.Find("SignUpPanel");
        LogoPanel.SetActive(true);
        SingUpPanel.SetActive(false);
        Debug.Log("注册成功!");
    }

    public void PostScore()
    { 
        string url = "http://81.71.17.48:80/user/save-score";
        SetScoreRequest PostData = new SetScoreRequest(UserName, currentLevel, Score());
        StartCoroutine(SendRequest(url, JsonUtility.ToJson(PostData), RequestType.POST,null));
    }

    private int Score()
    {
        //todo
        int MaxCombo = ComboManager.Instance.GetMaxCombo();
        int RemainWood = CreateObj.Instance.GetRemainWood();
        int RemainStone = CreateObj.Instance.GetRemainStone();
        int FinalScore = 5 -  3 * (RemainWood + RemainStone)/2 + MaxCombo;
        if (currentLevel == 1 || currentLevel == 3 || currentLevel == 5)
            return 3;
        else if (currentLevel == 2)
        {
            if (FinalScore < 5)
                return 1;
            else if (FinalScore < 6)
                return 2;
            else
                return 3;
        }
        else if (currentLevel == 6)
        {
            if (FinalScore < 4)
                return 1;
            else if (FinalScore < 5)
                return 2;
            else
                return 3;
        }
        else if (currentLevel == 7)
        {
            if (FinalScore < 8)
                return 1;
            else if (FinalScore < 10)
                return 2;
            else
                return 3;
        }
        else
            return 3;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RePlay()
    {
        Time.timeScale = 1;
        SceneManager.LoadScene(currentLevel + 1);
    }

    public void NextLevel()
    {
        if (currentLevel + 2 >= 0 && currentLevel + 2 <= SceneManager.sceneCountInBuildSettings)
            SceneManager.LoadScene(currentLevel + 2);
    }

    public void LoadLevel(int aimLevel)
    {
        if (aimLevel <= SceneManager.sceneCountInBuildSettings)
        {
            Time.timeScale = 1;
            SceneManager.LoadScene(aimLevel);
        }
    }

    private IEnumerator SendRequest(string url,string data,RequestType requestType,Action<string> CallBack)
    {
        UnityWebRequest WebReq;
        if (requestType == RequestType.POST)
        {
            WebReq = UnityWebRequest.Put(url, data);
            WebReq.method = UnityWebRequest.kHttpVerbPOST;
            WebReq.SetRequestHeader("Content-Type", "application/json");
            Debug.Log(data);
        }
        else if (requestType == RequestType.GET)
        {
            WebReq = UnityWebRequest.Get(url);
        }
        else
        {
            Debug.Log("request type error");
            yield break;
        }

        yield return WebReq.SendWebRequest();

        if (WebReq.isHttpError || WebReq.isNetworkError)
        {
            Debug.Log("http error:"+WebReq.error);
            yield break;
        }

        if (CallBack == null)
        {
            Debug.Log("null callback");
            yield break;
        }

        Debug.Log(WebReq.downloadHandler.text);
        CallBack(WebReq.downloadHandler.text);
    }
}
