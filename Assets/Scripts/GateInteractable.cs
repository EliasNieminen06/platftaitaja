using UnityEngine;

public class GateInteractable : MonoBehaviour, Interactable
{
    public enum keys
    {
        key1,
        key2
    }
    public keys key;
    public string description;
    public string GetDescription()
    {
        return description;
    }

    public void Interact()
    {
        if (key == keys.key1 && Inventory.Instance.key1 == true || key == keys.key2 && Inventory.Instance.key2 == true)
        {
            Announcements.instance.Announce("Opened the gate!", 2);
            transform.parent.GetComponent<Gate>().Open();
            PlayerInteract.instance.hideUI();
        }
    }
}
