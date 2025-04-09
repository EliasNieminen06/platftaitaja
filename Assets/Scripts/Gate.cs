using UnityEngine;

public class Gate : MonoBehaviour
{
    public GameObject openGateObj;
    public GameObject closedGateObj;
    public bool open = false;

    public void Open()
    {
        closedGateObj.SetActive(false);
        openGateObj.SetActive(true);
        open = true;
    }
}
