using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class ObjectHighlighter : MonoBehaviour
{
    [SerializeField] private float raycastDistance = 5f;
    [SerializeField] private Material outlineMaterial;

    private GameObject lastHighlightedObject;
    public GameObject crosshair;

    // Update is called once per frame
    void Update()
    {
        HighlightRaycastCheck();
    }

    void HighlightRaycastCheck()
    {
        Ray ray = new Ray(transform.position, transform.forward);

        if (Physics.Raycast(ray, out RaycastHit hit, raycastDistance))
        {
            if (hit.collider.TryGetComponent(out HighlightTarget target))
            {
                GameObject targetObject = target.gameObject;
                if (targetObject.CompareTag("Interactable"))
                {
                    if (lastHighlightedObject != targetObject)
                    {
                        ClearHighlight();
                        Renderer rend = targetObject.GetComponent<Renderer>();
                        List<Material> matArray = new(rend.materials);
                        matArray.Add(outlineMaterial);
                        rend.materials = matArray.ToArray();
                        lastHighlightedObject = targetObject;
                    }
                    return;
                }
            }
        }
        ClearHighlight();
    }

    void ClearHighlight()
    {
        if (lastHighlightedObject != null)
        {
            if (lastHighlightedObject.TryGetComponent(out HighlightTarget target))
            {
                Renderer rend = lastHighlightedObject.GetComponent<Renderer>();
                List<Material> mats = new(rend.materials);
                mats.RemoveAt(mats.Count - 1);
                rend.materials = mats.ToArray();
            }
            lastHighlightedObject = null;
        }
    }
}
