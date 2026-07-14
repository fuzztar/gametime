using UnityEngine;

public class Carry : MonoBehaviour
{
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        PickUpItem.pickedUp += Hold;
    }

    void Hold(PickUpItem what)
    {
        if (!this) { PickUpItem.pickedUp -= Hold; return; }
        what.transform.SetParent(transform);
        what.transform.localPosition = Vector3.zero;
    }


}
