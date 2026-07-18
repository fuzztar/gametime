using System.Collections;
using UnityEngine;

public class MusicManager : MonoBehaviour
{
    public static MusicManager Instance { get; private set; }

    [Header("Audio")]
    [SerializeField] private AudioSource musicSource;

    [Header("Volume")]
    [SerializeField] private float normalVolume = 0.4f;
    [SerializeField] private float dialogueVolume = 0.1f;
    [SerializeField] private float fadeSpeed = 2f;

    private Coroutine fadeRoutine;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }

        Instance = this;

        DontDestroyOnLoad(gameObject);

        if (musicSource == null)
        {
            musicSource = GetComponent<AudioSource>();
        }
    }

    private void Start()
    {
        if (musicSource != null)
        {
            musicSource.volume = normalVolume;

            if (!musicSource.isPlaying)
            {
                musicSource.Play();
            }
        }
        else
        {
            Debug.LogError("MusicManager: No AudioSource assigned.");
        }
    }

    public void LowerMusic()
    {
        FadeTo(dialogueVolume);
    }

    public void RestoreMusic()
    {
        FadeTo(normalVolume);
    }

    private void FadeTo(float targetVolume)
    {
        if (musicSource == null)
            return;

        if (fadeRoutine != null)
        {
            StopCoroutine(fadeRoutine);
        }

        fadeRoutine = StartCoroutine(FadeVolume(targetVolume));
    }

    private IEnumerator FadeVolume(float targetVolume)
    {
        while (Mathf.Abs(musicSource.volume - targetVolume) > 0.01f)
        {
            musicSource.volume = Mathf.MoveTowards(
                musicSource.volume,
                targetVolume,
                fadeSpeed * Time.deltaTime
            );

            yield return null;
        }

        musicSource.volume = targetVolume;
    }
}