using System.Collections;
using UnityEngine;

public class Doorinteraction : MonoBehaviour, IInteractable
{
    [Header("Door Settings")]
    public float openAngle = 90f;
    public float openSpeed = 2f;

    public bool isOpen = false;

    [Header("Lock Settings")]
    public bool locked = true;

    [Header("Door Object")]
    public Transform door;


    private Quaternion _closedRotation;
    private Quaternion _openRotation;

    private Coroutine _currentCoroutine;


    void Start()
    {
        _closedRotation = door.rotation;

        _openRotation =
            Quaternion.Euler(
                door.eulerAngles +
                new Vector3(0, openAngle, 0)
            );
    }



    private IEnumerator ToggleDoor()
    {
        Quaternion targetRotation =
            isOpen ? _closedRotation : _openRotation;


        isOpen = !isOpen;


        while (Quaternion.Angle(
            door.rotation,
            targetRotation) > 0.01f)
        {
            door.rotation =
                Quaternion.Lerp(
                    door.rotation,
                    targetRotation,
                    Time.deltaTime * openSpeed
                );

            yield return null;
        }


        door.rotation = targetRotation;
    }



    public void Interact()
    {
        if (locked)
        {
            Debug.Log("Door is locked!");
            return;
        }


        if (_currentCoroutine != null)
        {
            StopCoroutine(_currentCoroutine);
        }


        _currentCoroutine =
            StartCoroutine(ToggleDoor());
    }



    public void UnlockDoor()
    {
        locked = false;

        Debug.Log("Door unlocked!");
    }
}