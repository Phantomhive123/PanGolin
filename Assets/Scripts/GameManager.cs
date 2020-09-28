using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Networking;
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
    private GameObject gameWinPanel;
    public int currentLevel;

    private void Awake()
    {
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
    }

    public void GamePause()
    {
        Time.timeScale = 0;
    }

    public void GameWin()
    {
        gameWinPanel.SetActive(true);
        GamePause();
    }

    public void GameOver()
    {
        gameOverPanel.SetActive(true);
        GamePause();
    }

    public void SaveCurrentLevel()
    {
        Debug.Log("in click");
        string SaveFileName = "level" + Convert.ToString(currentLevel)+".dat";
        Debug.Log(SaveFileName);
        FileStream fs = new FileStream(SaveFileName, FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();
        List<SaveObject> SaveContent = CreateObj.GetUserObjects();
        formatter.Serialize(fs, SaveContent.Count);

        for (int i=0;i<SaveContent.Count;++i)
        {
            formatter.Serialize(fs, graph: SaveContent[i]);
        }

        fs.Flush();
        fs.Close();
    }

    public void LoadSaveFile()
    {
        Debug.Log("in load save file");
        string SaveFileName = "level" + Convert.ToString(currentLevel) + ".dat";
        Debug.Log("load file:" + SaveFileName);
        FileStream fs = new FileStream(SaveFileName, FileMode.OpenOrCreate);
        BinaryFormatter formatter = new BinaryFormatter();

        int count = (int)formatter.Deserialize(fs);

        for (int i=0;i<count;++i)
        {
            SaveObject FileObject = (SaveObject)formatter.Deserialize(fs);
            CreateObj.RenderSaveObject(FileObject.ObjType, new Vector3(FileObject.x, FileObject.y, 0));
        }

        fs.Close();
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
        Debug.Log(currentLevel + " " + SceneManager.sceneCount);
        if (currentLevel + 1 <= SceneManager.sceneCount)
            SceneManager.LoadScene(currentLevel + 1);
        CreateObj.ClearSaveInfo();
    }
}
