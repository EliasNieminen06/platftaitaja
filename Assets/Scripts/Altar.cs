using UnityEngine;

public class Altar : MonoBehaviour, Interactable
{
    public string description;
    public string GetDescription()
    {
        return description;
    }

    public void Interact()
    {
        if (Inventory.Instance.gem1 && Inventory.Instance.gem2)
        {
            GameManager.instance.FinishGame();
        }
        else
        {
            Announcements.instance.Announce("Missing gems!", 2);
        }
    }
}
