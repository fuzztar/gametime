using UnityEngine;

public class HackingManager : MonoBehaviour
{
    public GameObject hackingCanvas;
    public HackingMinigame minigame;

    public PlayerMovement playerMovement;

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


        if (currentComputer != null)
        {
            currentComputer.HackComplete();
        }


        StopHacking();
    }



    public void StopHacking()
    {
        Debug.Log("Stopping hacking UI!");


        // Stop minigame
        minigame.StopMinigame();


        // Hide hacking UI
        hackingCanvas.SetActive(false);


        // Restore interaction prompts
        ObjectHighlighter highlighter =
            FindFirstObjectByType<ObjectHighlighter>();

        if (highlighter != null)
        {
            highlighter.uiOpen = false;
        }


        // Restore camera movement
        MouseLook mouseLook =
            FindFirstObjectByType<MouseLook>();

        if (mouseLook != null)
        {
            mouseLook.canLook = true;
        }


        // Lock cursor back to gameplay
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;


        // Restore player movement
        playerMovement.enabled = true;


        Debug.Log("Hacking stopped!");
    }
}