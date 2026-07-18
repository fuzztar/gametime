using System.Collections;
using Unity.Cinemachine;
using UnityEngine;
using UnityEngine.Playables;

public class CutsceneTrigger : MonoBehaviour
{
    [SerializeField] private PlayableDirector director;
    [SerializeField] private CinemachineCamera cutsceneCam;
    [SerializeField] private CutsceneManager cutsceneManager;
    [SerializeField] private GameObject player;

    [HideInInspector] public bool cutsceneFinished = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            if (cutsceneManager.isCutscenePlaying) {
                StartCoroutine(WaitUntilCutsceneFinish());
            } else
            {
                StartCutscene();
            }
        }
    }

    private void StartCutscene()
    {
        cutsceneManager.SwitchToCinemachine();
        cutsceneCam.Priority = 20;
        director.Play();
        director.stopped += OnCutsceneEnded;
        GetComponent<Collider>().enabled = false;
    }

    private void OnCutsceneEnded(PlayableDirector dir)
    {
        Debug.Log("Cutscene ended.");
        cutsceneCam.Priority = 0;
        cutsceneManager.SwitchToRegularCamera();
        cutsceneFinished = true;
        director.stopped -= OnCutsceneEnded;
    }
    public void ResetCamera()
    {
        player.transform.GetChild(0).GetChild(0).rotation = Quaternion.Euler(0f, 0f, 0f);

    }
    IEnumerator WaitUntilCutsceneFinish()
    {
        yield return new WaitUntil(() => !cutsceneManager.isCutscenePlaying);
        StartCutscene();
    }
}
