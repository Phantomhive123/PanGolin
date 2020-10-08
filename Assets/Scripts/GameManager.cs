﻿using System.Collections;
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
    }

    public void GamePause()
    {
        Time.timeScale = 0;
    }

    public void GameWin()
    {
        gameWinPanel.SetActive(true);
        PostScore();
        GamePause();
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        GamePause();
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
        OnGetUserScore();
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

        NextLevel();
        Debug.Log("登录成功!");
    }

   public void OnRegister()
   {
        NameInput = GameObject.Find("SignUpPanel/Panel/UserName").GetComponent<InputField>();
        PasswdInput = GameObject.Find("SignUpPanel/Panel/Password").GetComponent<InputField>();
        EmailInput = GameObject.Find("SignUpPanel/Panel/Email").GetComponent<InputField>();
        string url = "http://81.71.17.48/user/register"; 
        RegisterRequest PostData = new RegisterRequest(NameInput.text, PasswdInput.text, EmailInput.text);
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

    public void OnGetUserScore()
    {
        string url = "http://81.71.17.48:80/user/get-score";
        GetScoreRequest PostData = new GetScoreRequest(UserName);
        StartCoroutine(SendRequest(url, JsonUtility.ToJson(PostData), RequestType.POST, GetUserScoreCallBack));
    }

    private void GetUserScoreCallBack(string RspDataString)
    {
        Debug.Log("recv msg:" + RspDataString);
        var RspData = JsonUtility.FromJson<ServerRsponse<GetScoreRsp>>(RspDataString);

        if (RspData.Code != 0)
        {
            Debug.Log("获取信息失败:" + RspData.Message);
            return;
        }

        //Debug.Log(RspData.Data);

        Debug.Log(RspData.Data.scores[0].score);
    }

    private int Score()
    {
        //todo
        return 5;
    }

    public void Quit()
    {
        Application.Quit();
    }

    public void RePlay()
    {
        SceneManager.LoadScene(currentLevel);
    }

    public void NextLevel()
    {
        if (currentLevel + 1 >= 0 && currentLevel + 1 <= SceneManager.sceneCount)
            SceneManager.LoadScene(currentLevel + 1);
    }

    public void LoadLevel(int aimLevel)
    {
        if (aimLevel <= SceneManager.sceneCount)
            SceneManager.LoadScene(aimLevel);
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
