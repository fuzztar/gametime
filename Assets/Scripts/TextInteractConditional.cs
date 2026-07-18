using UnityEngine;

public class TextInteractConditional : MonoBehaviour
{
    public string[] text;
    public ScrollingText scrollingText;
    [HideInInspector] public bool textBoxOn = false;

    public void StartText()
    {
        if (text != null)
        {
            textBoxOn = true;
            scrollingText.itemInfo = text;
            scrollingText.currentObject = this.gameObject;
            scrollingText.isConditional = true;
            scrollingText.gameObject.SetActive(true);
        }
    }
}
