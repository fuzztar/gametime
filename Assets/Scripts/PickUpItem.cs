using NUnit.Framework;
using UnityEngine;

public class PickUpItem : MonoBehaviour, IInteractable
{
    public ItemData itemData;
    [HideInInspector] public HotbarSlot hotbarSlot;

    public void Interact()
    {
        pickedUp?.Invoke(this);
    }

    public delegate void Status(PickUpItem what);
    public static event Status pickedUp;
}
