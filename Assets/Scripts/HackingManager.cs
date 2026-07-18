using System.Collections;
using UnityEngine;

public class HackingManager : MonoBehaviour
{
    [Header("Hacking UI")]
    public GameObject hackingCanvas;
    public HackingMinigame minigame;


    [Header("Player")]
    public PlayerMovement playerMovement;



    [Header("Completion Audio")]
    [SerializeField] private AudioSource hackingSoundSource;
    [SerializeField] private AudioSource hackingVoiceSource;

    [SerializeField] private AudioClip completionSound;
    [SerializeField] private AudioClip completionVoiceLine;

    [SerializeField] private float delayBeforeVoice = 0f;



    private ComputerInteractable currentComputer;



    public void StartHacking(ComputerInteractable computer)
    {
        currentComputer = computer;


        // Disable player movement
        playerMovement.enabled = false;


        // Disable interaction prompts
        ObjectHighlighter highlighter =
            FindFirstObjectByType<ObjectHighlighter>();

        if (highlighter != null)
        {
            highlighter.uiOpen = true;
        }


        // Disable camera movement
        MouseLook mouseLook =
            FindFirstObjectByType<MouseLook>();

        if (mouseLook != null)
        {
            mouseLook.canLook = false;
        }


        // Show hacking UI
        hackingCanvas.SetActive(true);


        // Unlock mouse for UI buttons
        Cursor.lockState = CursorLockMode.None;
        Cursor.visible = true;


        // Start minigame
        minigame.StartMinigame();


        Debug.Log("Hacking started!");
    }



    public void CompleteHack()
    {
        Debug.Log("Hack completed!");


        StartCoroutine(PlayCompletionSequence());


        if (currentComputer != null)
        {
            currentComputer.HackComplete();
        }


        StopHacking();
    }



    private IEnumerator PlayCompletionSequence()
    {
        // Play success sound
        if (completionSound != null &&
            hackingSoundSource != null)
        {
            hackingSoundSource.PlayOneShot(
                completionSound
            );
        }


        // Wait before voice line
        if (delayBeforeVoice > 0)
        {
            yield return new WaitForSeconds(
                delayBeforeVoice
            );
        }


        // Play voice line
        if (completionVoiceLine != null &&
            hackingVoiceSource != null)
        {
            hackingVoiceSource.PlayOneShot(
                completionVoiceLine
            );
        }
    }



    public void StopHacking()
    {
        Debug.Log("Stopping hacking UI!");


        if (minigame != null)
        {
            minigame.StopMinigame();
        }


        hackingCanvas.SetActive(false);



        ObjectHighlighter highlighter =
            FindFirstObjectByType<ObjectHighlighter>();

        if (highlighter != null)
        {
            highlighter.uiOpen = false;
        }



        MouseLook mouseLook =
            FindFirstObjectByType<MouseLook>();

        if (mouseLook != null)
        {
            mouseLook.canLook = true;
        }



        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;



        playerMovement.enabled = true;


        Debug.Log("Hacking stopped!");
    }
}