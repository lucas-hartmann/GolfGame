using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenuScript : MonoBehaviour
{
    [SerializeField] GameObject pm;

    [SerializeField] GameObject pb;


    public void Pause(){
        pm.SetActive(true);
        Time.timeScale = 0;
        pb.SetActive(false);
    }

    // public void Restart(){
    //     SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    //     pb.SetActive(true);
    //     Time.timeScale = 1;
    //
    // }

    // public void RestartCurrentScene()
    // {
    //     Scene currentScene = SceneManager.GetActiveScene(); // Get the current scene
    //     SceneManager.LoadScene(currentScene.name); // Reload the scene by its name
    // }

    public void Home(){
        SceneManager.LoadScene("MainMenu");
        Time.timeScale = 1;
    }

    public void Resume(){
        pm.SetActive(false);
        Time.timeScale = 1;
        pb.SetActive(true);
    }
}
