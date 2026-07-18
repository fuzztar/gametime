using UnityEngine;

public class PlayerAbilities : MonoBehaviour
{
    public static PlayerAbilities Instance { get; private set; }


    [Header("Abilities")]
    public bool HasHeardLockpickHint { get; private set; } = false;

    public bool CanLockpick { get; private set; } = false;



    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
            return;
        }


        Instance = this;
    }



    public void UnlockLockpickBox()
    {
        HasHeardLockpickHint = true;

        Debug.Log(
            "Lockpick box is now available."
        );
    }



    public void UnlockLockpicking()
    {
        CanLockpick = true;

        Debug.Log(
            "Player can now lockpick."
        );
    }
}