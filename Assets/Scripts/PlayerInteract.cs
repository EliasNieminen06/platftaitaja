using TMPro;
using UnityEngine;

public class PlayerInteract : MonoBehaviour
{
    public static PlayerInteract instance;
    public GameObject interactionUI;
    public TextMeshProUGUI interactionText;
    bool itemFound = false;
    Interactable interactable;

    private void Awake()
    {
        instance = this;
    }

    private void Update()
    {
        if (itemFound)
        {
            if (Input.GetKeyDown(KeyCode.E))
            {
                interactable.Interact();
            }
        }
    }

    public void hideUI()
    {
        itemFound = false;
        interactionUI.SetActive(itemFound);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            interactable = other.GetComponent<Interactable>();
            if (interactable != null)
            {
                itemFound = true;
                interactionText.text = interactable.GetDescription();
            }
        }

        interactionUI.SetActive(itemFound);
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Item"))
        {
            itemFound = false;
            interactionUI.SetActive(itemFound);
        }

    }
}
