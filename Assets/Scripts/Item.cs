using UnityEngine;

public class Item : MonoBehaviour, Interactable
{
    public enum items
    {
        key1,
        key2,
        gem1,
        gem2
    }
    public items item;
    public string description;
    public string GetDescription()
    {
        return description;
    }

    public void Interact()
    {
        if (item == items.key1)
        {
            Inventory.Instance.key1 = true;
        }
        else if (item == items.key2)
        {
            Inventory.Instance.key2 = true;
        }
        else if (item == items.gem1)
        {
            Inventory.Instance.gem1 = true;
        }
        else if (item == items.gem2)
        {
            Inventory.Instance.gem2 = true;
        }
        Destroy(gameObject);
    }
}
