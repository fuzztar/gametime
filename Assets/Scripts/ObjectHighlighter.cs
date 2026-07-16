using System.Collections.Generic;
using UnityEngine;

public class ObjectHighlighter : MonoBehaviour
{
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private Material outlineMaterial;
    [SerializeField] private LayerMask layerMask;
    [SerializeField] private HotbarController hotbarController;

    private GameObject lastHighlightedObject;
    private Renderer lastRenderer;

    [HideInInspector] public bool isHighlighting = false;
    [HideInInspector] public bool uiOpen = false;

    public GameObject crosshair;
    public GameObject crosshairOutline;
    public GameObject eTipInteractable;
    public GameObject eTipItem;

    void Update()
    {
        if (uiOpen)
        {
            ClearHighlight();
            ResetUI();
            return;
        }

        HighlightRaycastCheck();

        if (isHighlighting)
        {
            TurnOnUI();

            if (lastHighlightedObject != null && Input.GetKeyDown(KeyCode.E))
            {
                if (lastHighlightedObject.TryGetComponent<IInteractable>(out IInteractable interactable))
                {
                    if (hotbarController.slotsFilled < hotbarController.totalSlots)
                    {
                        interactable.Interact();
                    }
                    else
                    {
                        if (hotbarController.inventoryFullText.activeSelf)
                        {
                            hotbarController.inventoryFullText.SetActive(false);
                            hotbarController.inventoryFullText.SetActive(true);
                        }
                        else
                        {
                            hotbarController.inventoryFullText.SetActive(true);
                        }
                    }
                }
                else
                {
                    Debug.Log("No IInteractable script found.");
                }
            }
        }
        else
        {
            ResetUI();
        }
    }

    void HighlightRaycastCheck()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance, layerMask))
        {
            GameObject targetObject = hit.collider.gameObject;

            if (targetObject.CompareTag("Interactable") || targetObject.CompareTag("Item"))
            {
                if (lastHighlightedObject != targetObject)
                {
                    ClearHighlight();
                    AddHighlight(targetObject);
                }

                return;
            }
        }

        ClearHighlight();
    }

    void AddHighlight(GameObject targetObject)
    {
        Renderer rend = targetObject.GetComponent<Renderer>();

        if (rend == null)
        {
            rend = targetObject.GetComponentInChildren<Renderer>();
        }

        if (rend == null)
            return;

        List<Material> mats = new(rend.materials);

        mats.Add(outlineMaterial);

        rend.materials = mats.ToArray();

        lastRenderer = rend;

        lastHighlightedObject = targetObject;
        isHighlighting = true;
    }
    void ClearHighlight()
    {
        if (lastHighlightedObject != null)
        {
            if (lastHighlightedObject.CompareTag("Interactable") || lastHighlightedObject.CompareTag("Item"))
            {
                Renderer rend = lastHighlightedObject.GetComponent<Renderer>();

                if (rend != null)
                {
                    List<Material> mats = new(rend.materials);
                    if (mats.Count > 1)
                    {
                        mats.RemoveAt(mats.Count - 1);
                        rend.materials = mats.ToArray();
                    }
                }
            }

            lastHighlightedObject = null;
        }
        isHighlighting = false;
        // Turn off interaction UI immediately
        ResetUI();
    }

    void TurnOnUI()
    {
        if (lastHighlightedObject == null)
        {
            ResetUI();
            return;
        }
        if (!crosshairOutline.activeSelf)
        {
            crosshairOutline.SetActive(true);
        }

        if (lastHighlightedObject == null)
            return;

        if (lastHighlightedObject.CompareTag("Interactable"))
        {
            if (!eTipInteractable.activeSelf)
            {
                eTipInteractable.SetActive(true);
            }
        }
        else if (lastHighlightedObject.CompareTag("Item"))
        {
            if (!eTipItem.activeSelf)
            {
                eTipItem.SetActive(true);
            }
        }
    }

    void ResetUI()
    {
        crosshairOutline.SetActive(false);
        eTipInteractable.SetActive(false);
        eTipItem.SetActive(false);
    }
}