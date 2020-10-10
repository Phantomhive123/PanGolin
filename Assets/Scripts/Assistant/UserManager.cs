using System;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class UserManager : MonoBehaviour
{
    private InputField NameInput, PasswdInput;
    [SerializeField]
    private GameObject LogoPanel;
    private string UserName;
    public TMP_Text text;
    public GameObject commonPanel;

    private void Start()
    { 
        LogoPanel = GameObject.Find("LogoPanel");
    }
    public void OnLogin()
    {
        NameInput = GameObject.Find("LogInPanel/Panel/UserName").GetComponent<InputField>();
        PasswdInput = GameObject.Find("LogInPanel/Panel/Password").GetComponent<InputField>();
        string url = "http://81.71.17.48/user/login";
        UserName = NameInput.text;
        LoginRequest PostData = new LoginRequest(NameInput.text, PasswdInput.text);
        StartCoroutine(GameManager.Instance.SendRequest(url, JsonUtility.ToJson(PostData), RequestType.POST, OnLoginCallBack));
    }

    private void OnLoginCallBack(string RspDataString)
    {
        Debug.Log("recv msg:" + RspDataString);
        var RspData = JsonUtility.FromJson<ServerRsponse<string>>(RspDataString);

        if (RspData.Code != 0)
        {
            Debug.Log("登录失败:" + RspData.Message);
            commonPanel.SetActive(true);
            text.text = "登陆失败";
            return;
        }

        PlayerPrefs.SetString("user", UserName);
        GameManager.Instance.LoadLevel(1);
        Debug.Log("登录成功!");
    }

    public void OnRegister()
    {
        NameInput = GameObject.Find("SignUpPanel/Panel/UserName").GetComponent<InputField>();
        PasswdInput = GameObject.Find("SignUpPanel/Panel/Password").GetComponent<InputField>();
        string url = "http://81.71.17.48/user/register";
        RegisterRequest PostData = new RegisterRequest(NameInput.text, PasswdInput.text);
        StartCoroutine(GameManager.Instance.SendRequest(url, JsonUtility.ToJson(PostData), RequestType.POST, OnRegisterCallback));
    }

    private void OnRegisterCallback(string RspDataString)
    {
        Debug.Log("recv msg:" + RspDataString);
        var RspData = JsonUtility.FromJson<ServerRsponse<string>>(RspDataString);

        if (RspData.Code != 0)
        {
            Debug.Log("注册失败:" + RspData.Message);
            commonPanel.SetActive(true);
            text.text = "注册失败";
            return;
        }

        GameObject SingUpPanel = GameObject.Find("SignUpPanel");
        LogoPanel.SetActive(true);
        SingUpPanel.SetActive(false);
        Debug.Log("注册成功!");
        commonPanel.SetActive(true);
        text.text = "注册成功";
    }
}

