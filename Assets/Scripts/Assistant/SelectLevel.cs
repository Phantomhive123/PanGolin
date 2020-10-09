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
            if (currentChoose > 1)
                levels.GetChild(currentChoose - 2).gameObject.SetActive(true);
            if (currentChoose < 7)    
                levels.GetChild(currentChoose + 1).gameObject.SetActive(false);
            Vector3 Pos = levels.localPosition;
            Pos.x = Pos.x + 500;
            levels.localPosition = Pos;
        }
    }

    public void NextLevel()
    {
        if (currentChoose + 1 <= 8)
        {
            currentChoose++;
            if (currentChoose < 8)
                levels.GetChild(currentChoose).gameObject.SetActive(true);
            if (currentChoose >= 3) 
                levels.GetChild(currentChoose - 3).gameObject.SetActive(false);
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
