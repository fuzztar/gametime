using UnityEngine;

public class ResetDoors : MonoBehaviour
{
    [SerializeField] private DialogueTrigger dialogueTrigger;
    [SerializeField] private Transform door1;
    [SerializeField] private Transform door2;

    // Update is called once per frame
    void Update()
    {
        if (dialogueTrigger != null)
        {
            if (dialogueTrigger.triggered)
            {
                door1.localRotation = Quaternion.identity;
                door2.localRotation = Quaternion.identity;
                this.enabled = false;
            }
        }
    }
}
