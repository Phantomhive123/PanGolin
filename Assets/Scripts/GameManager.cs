using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
    }
}
