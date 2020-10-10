using System.Collections.Generic;
using System;

[Serializable]
public class LoginRequest
{
    public string user_name;
    public string user_pass;
    public LoginRequest(string pName, string pPassword)
    {
        this.user_name = pName;
        this.user_pass = pPassword;
    }
}

[Serializable]
public class RegisterRequest
{
    public string user_name;
    public string user_pass;
    public RegisterRequest(string pName, string pPassword)
    {
        this.user_name = pName;
        this.user_pass = pPassword;
    }
}

[Serializable]
public class SetScoreRequest
{
    public string user_name;
    public int level;
    public int score;
    public SetScoreRequest(string pName, int pLevel, int pScore)
    {
        this.user_name = pName;
        this.level = pLevel;
        this.score = pScore;
    }
}

[Serializable]
public class GetScoreRequest
{
    public string user_name;
    public int level;
    public GetScoreRequest(string pName)
    {
        this.user_name = pName;
        this.level = -1;
    }
    public GetScoreRequest(string pName, int pLevel)
    {
        this.user_name = pName;
        this.level = pLevel;
    }
}

[Serializable]
public class ScoreItem
{
    public int level;
    public int score;
}

[Serializable]
public class GetScoreRsp
{
    public List<ScoreItem> scores;
}

[Serializable]
public class ServerRsponse<T>
{
    public int Code;
    public string Message;
    public T Data;
}

public enum RequestType
{
    GET,
    POST
}
