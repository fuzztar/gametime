using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.UIElements;

public class HotbarSlot : MonoBehaviour
{
    public ItemData itemdata;
    public Sprite itemIcon;
    public ObjectHighlighter highlighterScript;
    public HotbarController hotbarController;
    private Sprite spriteComponent;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (highlighterScript.pickingUp)
        {
            ReplaceSprite();
            highlighterScript.pickingUp = false;
        }
    }

    void ReplaceSprite()
    {
        spriteComponent = gameObject.transform.GetChild(1).gameObject.GetComponent<Sprite>();
        spriteComponent = itemIcon;
    }
}
