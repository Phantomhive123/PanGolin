using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Networking;

public class ScoreManager : MonoBehaviour
{
    private string UserName;
    public RectTransform levels;

    void Start()
    {
        UserName = PlayerPrefs.GetString("user");
        GetUserScore();
    }

    public void GetUserScore()
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

        //todo 添加对各关星星的设置
        Debug.Log("get user score");
        foreach(ScoreItem s in RspData.Data.scores)
        {
            Transform level = levels.GetChild(s.level - 1);
            for (int i = 1; i <= s.score; i++)
            {
                level.Find("Star" + i).gameObject.SetActive(true);
            }
        }
    }

    private IEnumerator SendRequest(string url, string data, RequestType requestType, Action<string> CallBack)
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
            Debug.Log("http error:" + WebReq.error);
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

