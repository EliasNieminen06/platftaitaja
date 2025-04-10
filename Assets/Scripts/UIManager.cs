using UnityEngine;
using TMPro;

public class UIManager : MonoBehaviour
{
    public TextMeshProUGUI timer;
    private void Update()
    {
        timer.text = GameManager.instance.formattedTime;
    }
}
