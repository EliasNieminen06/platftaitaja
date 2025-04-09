using UnityEngine;

public class Inventory : MonoBehaviour
{
    public static Inventory Instance;
    public bool key1;
    public bool key2;
    public bool gem1;
    public bool gem2;

    private void Awake()
    {
        Instance = this;
    }
}
