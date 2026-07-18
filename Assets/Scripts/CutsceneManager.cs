using Unity.Cinemachine;
using UnityEngine;

public class CutsceneManager : MonoBehaviour
{
    [SerializeField] private CinemachineBrain cinemachineBrain;
    [HideInInspector] public bool isCutscenePlaying;

    [Header("Scripts")]
    [SerializeField] private MouseLook mouseLook;
    [SerializeField] private ObjectHighlighter objectHighlighter;
    [SerializeField] private PlayerMovement playerMovement;

    public void SwitchToRegularCamera()
    {
        cinemachineBrain.enabled = false;

        if (mouseLook != null) mouseLook.canLook = true;
        if (objectHighlighter != null) objectHighlighter.enabled = true;
        if (playerMovement != null) playerMovement.enabled = true;
        isCutscenePlaying = false;
    }

    public void SwitchToCinemachine()
    {
        if (mouseLook != null) mouseLook.canLook = false;
        if (objectHighlighter != null) objectHighlighter.enabled = false;
        if (playerMovement != null) playerMovement.enabled = false;
        isCutscenePlaying = true;

        cinemachineBrain.enabled = true;
    }
}
