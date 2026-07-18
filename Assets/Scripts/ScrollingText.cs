using System.Collections;
using UnityEngine;
using TMPro;
using System;


public class ScrollingText : MonoBehaviour
{
    [Header("Text Settings")]
    [TextArea] public string[] itemInfo;
    [SerializeField] private float textSpeed = 0.01f;

    [Header("UI Elements")]
    [SerializeField] private TextMeshProUGUI itemInfoText;
    private int currentDisplayingText = 0;

    [HideInInspector] public GameObject currentObject;
    [HideInInspector] public bool isConditional;
    [HideInInspector] public bool isScrolling = false;
    [SerializeField] private GameObject player;

    private void OnEnable()
    {
        player.GetComponent<PlayerMovement>().enabled = false;
        player.GetComponentInChildren<MouseLook>().enabled = false;
        player.GetComponentInChildren<ObjectHighlighter>().uiOpen = true;
        currentDisplayingText = 0;
        ActivateText();
    }

    private void Update()
    {
        if (isScrolling == false)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                if (currentDisplayingText < itemInfo.Length - 1)
                {
                    isScrolling = true;
                    currentDisplayingText++;
                    ActivateText();
                } else
                {
                    isScrolling = false;
                    player.GetComponent<PlayerMovement>().enabled = true;
                    player.GetComponentInChildren<MouseLook>().enabled = true;
                    player.GetComponentInChildren<ObjectHighlighter>().uiOpen = false;
                    if (isConditional)
                    {
                        currentObject.GetComponent<TextInteractConditional>().textBoxOn = false;
                    }
                    gameObject.SetActive(false);
                }
            }
        } else
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                Debug.Log("Activated");
                StopAllCoroutines();
                itemInfoText.text = itemInfo[currentDisplayingText];
                isScrolling = false;
            }
        }
    }

    public void ActivateText()
    {
        StartCoroutine(AnimateText());
    }

    IEnumerator AnimateText()
    {
        isScrolling = true;
        for (int i = 0; i <= itemInfo[currentDisplayingText].Length; i++)
        {
            itemInfoText.text = itemInfo[currentDisplayingText].Substring(0, i);
            yield return new WaitForSeconds(textSpeed);
        }
        isScrolling = false;
    }
}
