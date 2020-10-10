using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
using UnityEngine.UI;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System;
using TMPro;


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
    private GameObject gameWinPanel;
    public int currentLevel;

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
    }

    public void GameContinue()
    {
        Time.timeScale = 1;
        Magnet[] magnets = FindObjectsOfType<Magnet>();
        foreach (Magnet mag in magnets)
        {
            mag.begins = true;
            //mag.FindNextAim();
        }
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

    public void PostScore()
    { 
        string url = "http://81.71.17.48:80/user/save-score";
        SetScoreRequest PostData = new SetScoreRequest(PlayerPrefs.GetString("user"), currentLevel, Score());
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

    public IEnumerator SendRequest(string url,string data,RequestType requestType,Action<string> CallBack)
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
