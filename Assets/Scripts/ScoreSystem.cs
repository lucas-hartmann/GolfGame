using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class ScoreSystem : MonoBehaviour
{
    public int score = 0;
    private int stroke = 0;

    public TMP_Text scoreBar;
    public TMP_Text strokesBar;
    public TMP_Text result;

    public GameObject gameVictoryUI;
    public GameObject gameOverUI;
    public GameObject buttonUI;



    void Start()
    {
        strokesBar.text = "STROKES: " + stroke + "/6";
        scoreBar.text = "SCORE:" + score;
    }


    public void AddScore(int amount)
        {
            score += amount;
            scoreBar.text = "SCORE:" + score;
        }

    public void AddStroke(int amount)
    {
        stroke += amount;
        strokesBar.text = "STROKES: " + stroke + "/6";
        if (stroke == 7){
            GameOver();
        }
    }

    public void GameOver(){
        gameOverUI.SetActive(true);
        buttonUI.SetActive(true);

    }

    public void EndGame(){
        gameVictoryUI.SetActive(true);
        buttonUI.SetActive(true);
        result.text = "In " + stroke + " stroke(s)!";

    }
}
