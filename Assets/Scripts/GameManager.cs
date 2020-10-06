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
    }

    public void GameContinue()
    {
        Magnet[] magnets = FindObjectsOfType<Magnet>();
        foreach (Magnet magnet in magnets)
            magnet.FindNextAim();
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
        if (currentLevel + 1 >= 0 && currentLevel + 1 <= SceneManager.sceneCount)
            SceneManager.LoadScene(currentLevel + 1);
    }

    public void LoadLevel(int aimLevel)
    {
        if (aimLevel <= SceneManager.sceneCount)
            SceneManager.LoadScene(aimLevel);
    }
}
