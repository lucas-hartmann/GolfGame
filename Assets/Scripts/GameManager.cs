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

    // Toggle Pause-Menü
    private void TogglePause()
    {
        bool isActive = PauseEndMenu.activeSelf;
        PauseEndMenu.SetActive(!isActive);
        Time.timeScale = !isActive ? 0f : 1f;
    }

    // Methoden für Buttons im Menü
    public void Resume()
    {
        PauseEndMenu.SetActive(false);
        Time.timeScale = 1f;
    }

    public void Exit()
    {
        Time.timeScale = 1f; // Zeit zurücksetzen
        SceneManager.LoadScene("LevelSelector");
        Debug.Log("peach");
    }

    public void RestartLevel()
    {
        Time.timeScale = 1f; // Zeit zurücksetzen
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
