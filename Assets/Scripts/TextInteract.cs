using UnityEngine;

public class TextInteract : MonoBehaviour, IInteractable
{
    public string[] text;
    public ScrollingText scrollingText;

    public void Interact()
    {
        if (text != null)
        {
            scrollingText.itemInfo = text;
            scrollingText.gameObject.SetActive(true);
        }
    }
}
