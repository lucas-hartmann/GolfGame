using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class GameManager : MonoBehaviour
{
    [Header("Menus")]
    [SerializeField] GameObject PauseEndMenu;
    [SerializeField] GameObject WinMenu;
    [SerializeField] GameObject LoseMenu;

    [Header("Keine Ahnung wia i des denn nenn")]
    [SerializeField] public int maxShots = 6;
    [SerializeField] public int coinsCollected = 0;
    public int currentShots = 0;

    [Header("UI Stuff")]
    public TMP_Text shots;
    public TMP_Text coins;

    void Start(){
        shots.text = currentShots + "/" + maxShots;
        coins.text = coinsCollected.ToString();
    }
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
        SceneManager.LoadScene("MainMenu");
    }

    public void WinGame()
    {
        Debug.Log("WinMenu aufgerufen");
        Time.timeScale = 0f;
        WinMenu.SetActive(true);
    }

    public void LoseGame()
    {
        Debug.Log("LoseMenu aufgerufen");
        Time.timeScale = 0f;
        LoseMenu.SetActive(true);
    }

    public void RegisterShot()
    {
        currentShots++;
        Debug.Log($"Shots: {currentShots}/{maxShots}");
        shots.text = currentShots + "/" + maxShots;
    }


    public int GetCurrentShots()
    {
        return currentShots;
    }

    public void CoinCollected()
    {
        coinsCollected++;
        //Debug.Log($"Coins collected: {coinsCollected}/{amountToSpawn}");
        coins.text = coinsCollected.ToString();

    }
}
