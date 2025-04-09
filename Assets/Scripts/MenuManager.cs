using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class MenuManager : MonoBehaviour
{
    public TextMeshProUGUI time;

    private void Start()
    {
        time.text = "RECORD TIME: " + PlayerPrefs.GetString("formattedTime");
    }

    public void Play()
    {
        SceneManager.LoadScene("GameScene");
    }
}
