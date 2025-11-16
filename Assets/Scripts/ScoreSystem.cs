using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;


public class ScoreSystem : MonoBehaviour
{
    public int score = 0;
    private int stroke = 0;

    public TMP_Text scoreBar;
    public TMP_Text strokesBar;
    public TMP_Text endScreen;
    public TMP_Text result;


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
        endScreen.text = "GAME OVER!";

        //SceneManager.LoadScene("Level 1");
    }

    public void EndGame(){
        endScreen.text = "YOU WIN!";
        result.text = "In " + stroke + " stroke(s)!";

        //SceneManager.LoadScene("Level 2");
    }
}
