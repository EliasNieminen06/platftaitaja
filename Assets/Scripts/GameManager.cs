using UnityEngine;
using UnityEngine.SceneManagement;
public class GameManager : MonoBehaviour
{
    public static GameManager instance;
    public float timer;
    public bool gameOn;
    public string formattedTime;

    private void Awake()
    {
        instance = this;
    }

    private void Start()
    {
        StartGame();
    }

    private void Update()
    {
        if (gameOn)
        {
            timer += Time.deltaTime;
            int minutes = Mathf.FloorToInt(timer / 60);
            int seconds = Mathf.FloorToInt(timer % 60);
            int milliseconds = Mathf.FloorToInt((timer * 1000) % 1000);
            formattedTime = string.Format("{0:00}:{1:00}:{2:00}", minutes, seconds, milliseconds);
        }
    }

    public void StartGame()
    {
        gameOn = true;
    }

    public void FinishGame()
    {
        gameOn = false;
        PlayerPrefs.SetString("formattedTime", formattedTime);
        SceneManager.LoadScene("MainMenu");
    }
}
