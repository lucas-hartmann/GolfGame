using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [SerializeField] GameObject PauseEndMenu;

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            TogglePause();
        }
    }

    private void TogglePause()
    {
        bool isActive = PauseEndMenu.activeSelf;
        PauseEndMenu.SetActive(!isActive);
        Time.timeScale = !isActive ? 0f : 1f;
    }

    public void Resume()
    {
        PauseEndMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("Level Selector");
    }

    public void RestartLevel()
    {
        //Time.timeScale = 1f;
        Debug.Log("peach3");
        //Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene("Level 1");
    }
}
