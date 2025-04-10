using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public bool key1;
    public bool key2;
    public bool gem1;
    public bool gem2;
    public GameObject key1IMG;
    public GameObject key2IMG;
    public GameObject gem1IMG;
    public GameObject gem2IMG;

    private void Awake()
    {
        Instance = this;
    }

    private void Update()
    {
        if (key1)
        {
           key1IMG.SetActive(true); 
        }
        if (key2)
        {
            key2IMG.SetActive(true);
        }
        if (gem1)
        {
            gem1IMG.SetActive(true);
        }
        if (gem2)
        {
            gem2IMG.SetActive(true);
        }
    }
}
