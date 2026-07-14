using UnityEngine;

public class LockpickMinigame : MonoBehaviour
{
    public RectTransform marker;
    public RectTransform backgroundBar;
    public RectTransform successZone;

    public float speed = 200f;

    private float direction = 1f;
    private bool isActive = false;


    void Update()
    {
        if (!isActive)
            return;

        MoveMarker();

        // SPACE attempts the lockpick
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Debug.Log("Space pressed!");
            CheckLock();
        }
    }


    public void StartMinigame()
    {
        isActive = true;

        RandomizeSuccessZone();

        Debug.Log("Minigame started!");
    }


    public void StopMinigame()
    {
        isActive = false;

        // Reset marker position
        marker.anchoredPosition = new Vector2(0, marker.anchoredPosition.y);

        // Reset direction
        direction = 1f;

        Debug.Log("Minigame stopped!");
    }


    void RandomizeSuccessZone()
    {
        float limit = backgroundBar.rect.width / 2f;

        // Keep the zone away from the very edges
        float padding = successZone.rect.width / 2f;

        float randomX = Random.Range(
            -limit + padding,
            limit - padding
        );

        successZone.anchoredPosition = new Vector2(
            randomX,
            successZone.anchoredPosition.y
        );

        Debug.Log("Success zone moved!");
    }


    void MoveMarker()
    {
        Vector2 position = marker.anchoredPosition;

        position.x += direction * speed * Time.deltaTime;

        float limit = backgroundBar.rect.width / 2f;

        if (position.x >= limit)
        {
            position.x = limit;
            direction = -1f;
        }

        if (position.x <= -limit)
        {
            position.x = -limit;
            direction = 1f;
        }

        marker.anchoredPosition = position;
    }


    void CheckLock()
    {
        float markerPosition = marker.anchoredPosition.x;
        float zonePosition = successZone.anchoredPosition.x;
        float zoneWidth = successZone.rect.width;

        float difference = Mathf.Abs(markerPosition - zonePosition);

        if (difference <= zoneWidth / 2)
        {
            Debug.Log("SUCCESS!");

            FindFirstObjectByType<LockpickManager>().CompleteLock();
        }
        else
        {
            Debug.Log("FAILED!");

            FindFirstObjectByType<LockpickManager>().StopLockpicking();
        }
    }
}