using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelMove : MonoBehaviour
{
    public int sceneBuildIndex;

    // Level move zoned enter, if collider is a player
    // Move game to another scene

    private void OnTriggerEnter(Collider other)
    {
        print("Trigger Entered");

        if (other.gameObject.name.ToLower().Contains("kurt"))
        {
            print("Switching Scene to " + sceneBuildIndex);
            SceneManager.LoadScene(1); //load into scene index of floor 2
        }
    }
}
