using System.Diagnostics.CodeAnalysis;
using UnityEngine;

public class HotbarController : MonoBehaviour
{
    [HideInInspector] public HotbarSlot currentSlot;

    [HideInInspector] public int slotsFilled;
    public int totalSlots;
    public GameObject inventoryFullText;
    public GameObject grip;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PickUpItem.pickedUp += Collect;
    }

    void Collect(PickUpItem what)
    {
        if (!this) { PickUpItem.pickedUp -= Collect; return; }
        foreach (var slot in GetComponentsInChildren<HotbarSlot>())
        {
            if (slot.itemData == null)
            {
                RemoveSlot();
                slot.pickUpItem = what;
                slot.AttachItem(what.itemData);
                SetSlot(slot);
                break;
            }
        }
    }

    public void SetSlot(HotbarSlot slot)
    {
        currentSlot = slot;
        if (slot != null) { slot.highlighted = true; }
        if (currentSlot.pickUpItem != null)
        {
            currentSlot.pickUpItem.gameObject.SetActive(true);
        }
    }

    public void RemoveSlot()
    {
        if (currentSlot != null)
        {
            currentSlot.highlighted = false;
            if (currentSlot.pickUpItem != null)
            {
                currentSlot.pickUpItem.gameObject.SetActive(false);
            }
            currentSlot = null;
        }
    }

}
