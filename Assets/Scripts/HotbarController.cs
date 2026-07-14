using NUnit.Framework;
using System;
using UnityEngine;

public class HotbarController : MonoBehaviour
{
    [HideInInspector] public List slots = new List();

    public GameObject slot1;
    public GameObject slot2;
    public GameObject slot3;

    private GameObject highlight;

    private GameObject currentSlot;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKey(KeyCode.Alpha1))
        {
            if (currentSlot != slot1)
            {
                RemoveSlot();
                currentSlot = slot1;
                SetSlot();
            } else
            {
                RemoveSlot();
                currentSlot = null;
            }
        } else if (Input.GetKey(KeyCode.Alpha2))
        {
            if (currentSlot != slot2)
            {
                RemoveSlot();
                currentSlot = slot2;
                SetSlot();
            } else
            {
                RemoveSlot();
                currentSlot = null;
            }
        } else if (Input.GetKey(KeyCode.Alpha3))
        {
            if (currentSlot != slot3)
            {
                RemoveSlot();
                currentSlot = slot3;
                SetSlot();
            } else
            {
                RemoveSlot();
                currentSlot = null;
            }
        }

    }

    void SetSlot()
    {
        if (currentSlot != null)
        {
            highlight = currentSlot.transform.GetChild(0).gameObject;
            highlight.SetActive(true);
        }
        
    }

    void RemoveSlot()
    {
        if (highlight != null)
        {
            highlight.SetActive(false);
        }
        currentSlot = null;
    }

    void addItemToSlot()
    {

    }
}
