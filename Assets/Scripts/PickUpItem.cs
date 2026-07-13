using NUnit.Framework;
using UnityEngine;

public class PickUpItem : MonoBehaviour, IInteractable
{
    public ItemData itemData;
    public HotbarController hotbarController;

    public void Interact()
    {
        gameObject.SetActive(false);
    }
}
