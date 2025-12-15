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
    [SerializeField] private float par = 2;
    public int currentShots = 0;

    [Header("UI Stuff")]
    public TMP_Text shots;
    public TMP_Text coins;
    public TMP_Text result;

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

    public void Restart()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex);
    }

    public void Exit()
    {
        Time.timeScale = 1f;
        SceneManager.LoadScene("MainMenu");
    }

    public void NextLevel()
    {
        Time.timeScale = 1f;
        Scene currentScene = SceneManager.GetActiveScene();
        SceneManager.LoadScene(currentScene.buildIndex + 1);
    }

    public void WinGame()
    {
        Debug.Log("WinMenu aufgerufen");
        CalcResText();
        Time.timeScale = 0f;
        WinMenu.SetActive(true);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayWinSound();
        }
    }

    public void LoseGame()
    {
        Debug.Log("LoseMenu aufgerufen");
        Time.timeScale = 0f;
        LoseMenu.SetActive(true);
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayLoseSound();
        }
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
        if (SoundManager.Instance != null)
        {
            SoundManager.Instance.PlayCoinSound();
        }
    }
        public void CalcResText()
        {
            int diff = currentShots - (int)par;
            string resultText = "";

            if (currentShots == 1)
            {
                resultText = "Hole-in-One!";
            }
            else
            {
                switch (diff)
                {
                    case -2:
                        resultText = "Eagle!";
                        break;
                    case -1:
                        resultText = "Birdie!";
                        break;
                    case 0:
                        resultText = "Par!";
                        break;
                    case 1:
                        resultText = "Bogey!";
                        break;
                    default:
                        if (diff > 1)
                            resultText = diff + " over Par";
                        else
                            resultText = (-diff) + " under Par";
                        break;
                }
            }

            result.text = resultText;

        }

}
