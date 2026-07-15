using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HackingMinigame : MonoBehaviour
{
    private enum ColorButton
    {
        Red,
        Blue,
        Green,
        Yellow
    }


    [Header("Buttons")]
    public Button redButton;
    public Button blueButton;
    public Button greenButton;
    public Button yellowButton;


    [Header("UI")]
    public TMP_Text statusText;


    [Header("Difficulty Settings")]
    public int sequenceLength = 6;
    public float flashTime = 0.7f;
    public float delayBetweenFlashes = 0.6f;


    private bool isActive = false;
    private bool acceptingInput = false;


    // Computer generated sequence
    private List<ColorButton> sequence = new List<ColorButton>();

    // Player input sequence
    private List<ColorButton> playerSequence = new List<ColorButton>();


    // Color -> Button reference
    private Dictionary<ColorButton, Button> buttonMap;


    // Original button sizes
    private Dictionary<Button, Vector3> originalScales =
        new Dictionary<Button, Vector3>();



    private void Awake()
    {
        buttonMap = new Dictionary<ColorButton, Button>()
        {
            { ColorButton.Red, redButton },
            { ColorButton.Blue, blueButton },
            { ColorButton.Green, greenButton },
            { ColorButton.Yellow, yellowButton }
        };


        foreach (Button button in buttonMap.Values)
        {
            originalScales[button] =
                button.transform.localScale;
        }


        redButton.onClick.AddListener(() =>
            ButtonPressed(ColorButton.Red));

        blueButton.onClick.AddListener(() =>
            ButtonPressed(ColorButton.Blue));

        greenButton.onClick.AddListener(() =>
            ButtonPressed(ColorButton.Green));

        yellowButton.onClick.AddListener(() =>
            ButtonPressed(ColorButton.Yellow));
    }



    public void StartMinigame()
    {
        isActive = true;
        acceptingInput = false;


        sequence.Clear();
        playerSequence.Clear();


        for (int i = 0; i < sequenceLength; i++)
        {
            ColorButton randomColor =
                (ColorButton)Random.Range(0, 4);

            sequence.Add(randomColor);
        }


        Debug.Log("Hacking minigame started!");

        DisplaySequenceInConsole();

        StartCoroutine(PlaySequence());
    }



    public void StopMinigame()
    {
        isActive = false;
        acceptingInput = false;


        sequence.Clear();
        playerSequence.Clear();


        StopAllCoroutines();


        foreach (Button button in originalScales.Keys)
        {
            button.transform.localScale =
                originalScales[button];
        }


        if (statusText != null)
        {
            statusText.text = "WAITING...";
        }


        Debug.Log("Hacking minigame stopped!");
    }



    private IEnumerator PlaySequence()
    {
        statusText.text = "MEMORIZE THE SEQUENCE";


        yield return new WaitForSeconds(1f);



        foreach (ColorButton color in sequence)
        {
            Button button = GetButton(color);


            button.transform.localScale =
                originalScales[button] * 1.25f;


            yield return new WaitForSeconds(flashTime);


            button.transform.localScale =
                originalScales[button];


            yield return new WaitForSeconds(delayBetweenFlashes);
        }



        statusText.text =
            "REPEAT THE SEQUENCE";


        acceptingInput = true;
    }



    private void ButtonPressed(ColorButton color)
    {
        if (!isActive || !acceptingInput)
            return;


        playerSequence.Add(color);


        Debug.Log(
            "Player pressed: " +
            GetColorLetter(color)
        );


        StartCoroutine(
            FlashButton(GetButton(color))
        );


        CheckPlayerInput();
    }



    private IEnumerator FlashButton(Button button)
    {
        button.transform.localScale =
            originalScales[button] * 1.25f;


        yield return new WaitForSeconds(0.2f);


        button.transform.localScale =
            originalScales[button];
    }



    private void CheckPlayerInput()
    {
        int currentIndex =
            playerSequence.Count - 1;



        if (playerSequence[currentIndex] !=
            sequence[currentIndex])
        {
            Debug.Log("FAILED HACK!");

            StartCoroutine(FailedHack());

            return;
        }



        if (playerSequence.Count ==
            sequence.Count)
        {
            Debug.Log("SUCCESSFUL HACK!");

            StartCoroutine(SuccessfulHack());
        }
    }



    private IEnumerator FailedHack()
    {
        acceptingInput = false;


        statusText.text =
            "ACCESS DENIED";


        yield return new WaitForSeconds(1.5f);


        FindFirstObjectByType<HackingManager>()
            .StopHacking();
    }



    private IEnumerator SuccessfulHack()
    {
        acceptingInput = false;


        statusText.text =
            "ACCESS GRANTED";


        yield return new WaitForSeconds(1.5f);


        FindFirstObjectByType<HackingManager>()
            .CompleteHack();
    }



    private void DisplaySequenceInConsole()
    {
        string debugSequence = "";


        foreach (ColorButton color in sequence)
        {
            debugSequence +=
                GetColorLetter(color);
        }


        Debug.Log(
            "Sequence: " +
            debugSequence
        );
    }



    private string GetColorLetter(ColorButton color)
    {
        switch (color)
        {
            case ColorButton.Red:
                return "R";

            case ColorButton.Blue:
                return "B";

            case ColorButton.Green:
                return "G";

            case ColorButton.Yellow:
                return "Y";

            default:
                return "?";
        }
    }



    private Button GetButton(ColorButton color)
    {
        return buttonMap[color];
    }
}