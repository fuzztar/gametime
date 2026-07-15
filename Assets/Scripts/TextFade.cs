using TMPro;
using UnityEngine;

public class TextFade : MonoBehaviour
{
    public float fadeTime;

    private float currentFadeTime;
    private TextMeshProUGUI fadeAwayText;
    private float alphaValue;
    private float fadeAwayPerSecond;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Awake()
    {
        fadeAwayText = GetComponent<TextMeshProUGUI>();
        fadeAwayPerSecond = 1 / fadeTime;
    }
    void OnEnable()
    {
        currentFadeTime = fadeTime;
        alphaValue = 1;
    }

    // Update is called once per frame
    void Update()
    {
        if (currentFadeTime > 0)
        {
            alphaValue -= fadeAwayPerSecond * Time.deltaTime;
            fadeAwayText.color = new Color(fadeAwayText.color.r, fadeAwayText.color.g, fadeAwayText.color.b, alphaValue);
            currentFadeTime -= Time.deltaTime;
        } else
        {
            gameObject.SetActive(false);
        }
    }
}
