using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HighlightTarget : MonoBehaviour
{
    [HideInInspector] public Material[] originalMaterials;

    void Awake()
    {
        Renderer rend = GetComponent<Renderer>();
        Material[] matArray = rend.materials;
        originalMaterials = matArray;
    }
}
