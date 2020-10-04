using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class SelectLevel : MonoBehaviour
{
    public RectTransform levels;
    public int currentChoose = 1;

    public void LastLevel()
    {
        if (currentChoose - 1 > 0)
        {
            currentChoose--;
            Vector3 Pos = levels.localPosition;
            Pos.x = Pos.x + 500;
            levels.localPosition = Pos;
        }
    }

    public void NextLevel()
    {
        if (currentChoose + 1 <= 6)
        {
            currentChoose++;
            Vector3 Pos = levels.localPosition;
            Pos.x = Pos.x - 500;
            levels.localPosition = Pos;
        }
    }

    public void Begin()
    {
        Debug.Log("loadLevel:" + currentChoose);
        GameManager.Instance.LoadLevel(currentChoose + 1);
    }
}
