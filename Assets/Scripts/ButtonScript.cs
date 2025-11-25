using UnityEngine;
using UnityEngine.SceneManagement;

public class ReturnToLevelSelect : MonoBehaviour
{

    public void GoToLevelSelect()
    {
        SceneManager.LoadScene("Level Selector");
    }

    public void Restart()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().name);
        }
}