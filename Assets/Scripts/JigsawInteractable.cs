using UnityEngine;

public class JigsawInteractable : MonoBehaviour, IInteractable
{
    [Header("Puzzle Manager")]
    [SerializeField] private JigsawManager jigsawManager;

    public void Interact()
    {
        if (jigsawManager != null)
        {
            jigsawManager.OpenPuzzle();
        }
        else
        {
            Debug.LogWarning("No JigsawManager assigned!");
        }
    }
}