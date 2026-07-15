using UnityEngine;
using UI = UnityEngine.UI;

public class HotbarSlot : MonoBehaviour
{
    public ItemData itemData { get; private set; }
    [HideInInspector] public PickUpItem pickUpItem;
    public UI.Image icon;
    public Sprite blankIcon;
    public GameObject highlight;
    public string hotkey = "";
    internal static bool haskey;

    public bool highlighted {
        get => highlight.activeSelf;
        set {  highlight.SetActive(value); } 
    }

    public void AttachItem(ItemData item)
    {
        itemData = item;
        icon.sprite = itemData.itemIcon;
        pickUpItem.hotbarSlot = this;
        Collider objectCollider = pickUpItem.gameObject.GetComponent<Collider>();
        objectCollider.enabled = false;
    }

    public void UseItem(ItemData item)
    {
        itemData = item;
        icon.sprite = blankIcon;
        pickUpItem.hotbarSlot = null;
    }

    private void Update()
    {
        if (Input.GetKeyUp(hotkey))
        {
            if (GetComponentInParent<HotbarController>().currentSlot != this)
            {
                GetComponentInParent<HotbarController>().RemoveSlot();
                GetComponentInParent<HotbarController>().SetSlot(this);
            } else
            {
                GetComponentInParent<HotbarController>().RemoveSlot();
            }
            
        }
    }
}
